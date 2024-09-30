using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ButtonOption
{
    public class ButtonAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public Image buttonImage;
        public float hoverOpacity = 1f;
        public float normalOpacity = 0.5f; 
        
        private void Start()
        {
            if (buttonImage == null)
            {
                buttonImage = GetComponent<Image>();
            }

            SetOpacity(normalOpacity);  
        }

      
        public void OnPointerEnter(PointerEventData eventData)
        {
            SetOpacity(hoverOpacity);
        }

      
        public void OnPointerExit(PointerEventData eventData)
        {
            SetOpacity(normalOpacity);  
        }

       
        private void SetOpacity(float opacity)
        {
            Color color = buttonImage.color;
            color.a = opacity;
            buttonImage.color = color;
        }
    }
}
