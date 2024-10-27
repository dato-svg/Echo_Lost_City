using System.Collections;
using System.Collections.Generic;
using Dialoges;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI dialogueTextUI;
    public TextMeshProUGUI dialogueNameUI;
    public Transform optionsContainer;
    public GameObject optionPrefab;
    public SettingManager settingManager;
    [Space]
    [Header("КНОПКИ")][Space]
    public Button historyButton;
    public Button continueButton;
    public Button nextButton;
    public Button autoButton;
    public Button settingButton;

    [Header("обеькты")][Space]
    public GameObject dialogueNameObject;

    [Header("история")][Space]
    public GameObject ContentParent;
    public GameObject dialoguePrefab;

    [Header("картинки")] [Space]
    public Image characterImageLeft;
    public Image characterImageRight;
    public Image characterImageCenter;

    public Dialogue currentDialogue;

    [Header("Числа")][Space]
    public float textSpeed = 0.05f;
    public float defaultSpeed = 0.05f;
    public float defaultAutoSpeed = 1;
    public float fadeInDuration = 1f;
    public float autoModeDelay = 2f;

    [Header("Слайдер")]
    [Space]
    public Slider textSpeedSlider;
    public Slider AwtoTextSpeedSlider;

    private bool isTyping;
    private bool isAutoMode = false;

    private Coroutine typingCoroutine;

    private void Start()
    {
        
        characterImageLeft.gameObject.SetActive(false);
        characterImageRight.gameObject.SetActive(false);
        characterImageCenter.gameObject.SetActive(false);
        continueButton.gameObject.SetActive(false);

        historyButton.onClick.AddListener(() => settingManager.ActiveCurrentChild(0));
        settingButton.onClick.AddListener(() => settingManager.ActiveCurrentChild(1));
        autoButton.onClick.AddListener(ToggleAutoMode);
        ShowDialogue(currentDialogue);

        textSpeed = PlayerPrefs.GetFloat("TextSpeed");
        autoModeDelay = PlayerPrefs.GetFloat("AutoSpeed");

        textSpeedSlider.value = textSpeed;
        AwtoTextSpeedSlider.value = autoModeDelay;
    }

    public void StartDialogue(Dialogue startingDialogue)
    {
        currentDialogue = startingDialogue;
        ShowDialogue(currentDialogue);
    }

    private void ShowDialogue(Dialogue dialogue)
    {
        isTyping = true;
        StopAllCoroutines();
        typingCoroutine = StartCoroutine(TypeDialogue(dialogue.GetDialogueText()));
        GameObject historiObj = Instantiate(dialoguePrefab);
        historiObj.transform.parent = ContentParent.transform;
        historiObj.GetComponent<ButtonHistory>().name = dialogue.name;
        historiObj.GetComponent<ButtonHistory>().text = dialogue.dialogueText;
        currentDialogue = dialogue;

        if (string.IsNullOrEmpty(dialogue.name))
        {
            dialogueNameObject.SetActive(false);
        }
        else
        {
            dialogueNameObject.SetActive(true);
            dialogueNameUI.text = dialogue.name.ToString();
        }

        foreach (Transform child in optionsContainer)
        {
            Destroy(child.gameObject);
        }

        bool hasOptions = false;

        foreach (DialogueOption option in dialogue.GetDialogueOptions())
        {
            option.onOptionSelected?.Invoke();
            if (!string.IsNullOrEmpty(option.optionText))
            {
                hasOptions = true;

                GameObject newOption = Instantiate(optionPrefab, optionsContainer);
                TextMeshProUGUI optionText = newOption.GetComponentInChildren<TextMeshProUGUI>();
                optionText.text = option.optionText;

                Button optionButton = newOption.GetComponent<Button>();
                optionButton.onClick.AddListener(() => OnOptionSelected(option));
            }
        }

        if (!hasOptions)
        {
            continueButton.gameObject.SetActive(true);
            nextButton.onClick.RemoveAllListeners();
            continueButton.onClick.RemoveAllListeners();
            continueButton.onClick.AddListener(() => ContinueDialogue(dialogue));
            nextButton.onClick.AddListener(() => ContinueDialogue(dialogue));

            if (isAutoMode)
            {
                StartCoroutine(AutoContinueDialogue(dialogue));
            }
        }

        else
        {
            continueButton.gameObject.SetActive(false);
            nextButton.onClick.RemoveAllListeners();
        }
    }

    private IEnumerator TypeDialogue(string dialogueText)
    {
        dialogueTextUI.text = "";
        foreach (char letter in dialogueText.ToCharArray())
        {
            dialogueTextUI.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }
        isTyping = false;
    }

    private IEnumerator FadeInCharacterImage(Image image, Dialogue dialogue)
    {
        Color color = image.color;
        if (dialogue.hasSprite)
        {
            color.a = 1f;
            image.color = color;
            yield break;
        }

        color.a = 0f;
        image.color = color;

        float elapsedTime = 0f;

        while (elapsedTime < fadeInDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / fadeInDuration);
            color.a = alpha;
            image.color = color;
            yield return null;
        }
    }

    private IEnumerator AutoContinueDialogue(Dialogue currentDialogue)
    {
       
        while (isAutoMode)
        {
            if (!isTyping && currentDialogue != null)
            {
                yield return new WaitForSeconds(autoModeDelay);
                ContinueDialogue(currentDialogue);
                yield break;
            }
            yield return null;
        }
       
    }


    private void ContinueDialogue(Dialogue currentDialogue)
    {
        if (isTyping)
        {
            StopCoroutine(typingCoroutine);
            dialogueTextUI.text = currentDialogue.GetDialogueText();
            isTyping = false;
        }
        else
        {
            DialogueOption nextOption = currentDialogue.GetDialogueOptions()[0];
            if (nextOption.nextDialogue != null)
            {
                ShowDialogue(nextOption.nextDialogue);
            }
            else
            {
                EndDialogue();
            }
            isTyping = true;
        }

        if (isAutoMode)
        {
            StartCoroutine(AutoContinueDialogue(currentDialogue));
        }
    }

    private void OnOptionSelected(DialogueOption selectedOption)
    {
        selectedOption.onOptionSelected?.Invoke();

        if (selectedOption.nextDialogue != null)
        {
            ShowDialogue(selectedOption.nextDialogue);
        }
        else
        {
            EndDialogue();
        }
    }

    private void EndDialogue()
    {
        dialogueTextUI.text = "";
        foreach (Transform child in optionsContainer)
        {
            Destroy(child.gameObject);
        }

        continueButton.gameObject.SetActive(false);
    }

    private void ToggleAutoMode()
    {
       
        if(isAutoMode == false)
        {
            isAutoMode = true;
        }
        else
        {
            isAutoMode = false;
        }

        if (isAutoMode)
        {
            Debug.Log("Auto mode ON");

            if (currentDialogue != null)
            {
                StartCoroutine(AutoContinueDialogue(currentDialogue));
                Debug.Log("AutoContinue Dialogue");
            }
        }

        else
        {
            Debug.Log("Auto mode OFF");
        }
    }

    public void ChangeTextSpeed()
    {
        textSpeed = textSpeedSlider.value;
        PlayerPrefs.SetFloat("TextSpeed", textSpeed);
        Debug.Log("ChangeText//" + textSpeed);
    }

    public void ChangeAwtoSpeed()
    {
        autoModeDelay = AwtoTextSpeedSlider.value;
        PlayerPrefs.SetFloat("AutoSpeed",autoModeDelay);
        Debug.Log("ChangeAutoText//" + autoModeDelay);
    }

    public void DefaultSlider()
    {
        textSpeedSlider.value = defaultSpeed;
        AwtoTextSpeedSlider.value = defaultAutoSpeed;
    }


   

}
