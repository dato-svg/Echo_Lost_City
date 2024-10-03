using Dialoges;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

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
        
        StopAllCoroutines();
        StartCoroutine(TypeDialogue(dialogue.GetDialogueText()));

     
        if (dialogue.characterSpriteLeft != null && dialogue.characterSpriteRight != null)
        {
            
            characterImageCenter.gameObject.SetActive(false);
            characterImageLeft.gameObject.SetActive(true);
            characterImageRight.gameObject.SetActive(true);
            characterImageLeft.sprite = dialogue.characterSpriteLeft;
            characterImageRight.sprite = dialogue.characterSpriteRight;
        }
        else if (dialogue.characterSpriteLeft != null || dialogue.characterSpriteRight != null)
        {
         
            characterImageLeft.gameObject.SetActive(false);
            characterImageRight.gameObject.SetActive(false);
            characterImageCenter.gameObject.SetActive(true);

            
            if (dialogue.characterSpriteLeft != null)
                characterImageCenter.sprite = dialogue.characterSpriteLeft;
            else
                characterImageCenter.sprite = dialogue.characterSpriteRight;
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
    }

   
    private void ContinueDialogue(Dialogue currentDialogue)
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
