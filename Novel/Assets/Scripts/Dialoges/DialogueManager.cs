using System.Collections;
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
    public Button continueButton;
    public GameObject dialogueNameObject;

    public Image characterImageLeft;
    public Image characterImageRight;
    public Image characterImageCenter;

    public Dialogue currentDialogue;

    public float textSpeed = 0.05f;
    public float fadeInDuration = 1f;

    public bool isTyping;
    private Coroutine typingCoroutine;


    private void Start()
    {
        characterImageLeft.gameObject.SetActive(false);
        characterImageRight.gameObject.SetActive(false);
        characterImageCenter.gameObject.SetActive(false);
        continueButton.gameObject.SetActive(false);
        ShowDialogue(currentDialogue);
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

        if (dialogue.characterSpriteLeft != null && dialogue.characterSpriteRight != null)
        {
            characterImageCenter.gameObject.SetActive(false);
            characterImageLeft.gameObject.SetActive(true);
            characterImageRight.gameObject.SetActive(true);
            characterImageLeft.sprite = dialogue.characterSpriteLeft;
            characterImageRight.sprite = dialogue.characterSpriteRight;


            StartCoroutine(FadeInCharacterImage(characterImageLeft, dialogue));
            StartCoroutine(FadeInCharacterImage(characterImageRight, dialogue));
        }
        else if (dialogue.characterSpriteLeft != null || dialogue.characterSpriteRight != null)
        {   
            characterImageLeft.gameObject.SetActive(false);
            characterImageRight.gameObject.SetActive(false);
            characterImageCenter.gameObject.SetActive(true);

            if (dialogue.characterSpriteLeft != null)
            {
                characterImageCenter.sprite = dialogue.characterSpriteLeft;
            }
            else
            {
                characterImageCenter.sprite = dialogue.characterSpriteRight;
            }

            StartCoroutine(FadeInCharacterImage(characterImageCenter, dialogue));
        }
        else
        {
            characterImageLeft.gameObject.SetActive(false);
            characterImageRight.gameObject.SetActive(false);
            characterImageCenter.gameObject.SetActive(false);
        }

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
            continueButton.onClick.RemoveAllListeners();
            continueButton.onClick.AddListener(() => ContinueDialogue(dialogue));
        }
        else
        {
            continueButton.gameObject.SetActive(false);
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
}

