using System.Collections.Generic;
using UnityEngine;
  
namespace Dialoges
{
    [CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue System/Dialogue")]
    public class Dialogue : ScriptableObject
    {

        [TextArea(3, 10)] public string dialogueText;
        public string name;
        public Sprite characterSpriteLeft;
        public Sprite characterSpriteRight;
        public bool hasSprite;

        public List<DialogueOption> dialogueOptions;
        
        
        public string GetDialogueText()
        {
            return dialogueText;
        }

        public List<DialogueOption> GetDialogueOptions()
        {
            return dialogueOptions;
        }
    }
}  
