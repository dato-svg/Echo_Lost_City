using UnityEngine;

namespace Dialoges
{
    [System.Serializable]
    public class DialogueOption
    {
        [TextArea(2, 5)] public string optionText;
        public Dialogue nextDialogue;
        public UnityEngine.Events.UnityEvent onOptionSelected;
    }
}