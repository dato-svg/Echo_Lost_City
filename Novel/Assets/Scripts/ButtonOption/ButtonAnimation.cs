using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ButtonOption
{
    public class ButtonAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public float hoverOpacity = 1f;
        public float normalOpacity = 0.2f;
        public bool hasText;
        public bool hasScale;
        private Image buttonImage;
        private TextMeshProUGUI textMeshProUGUI;
     

        private void Start()
        {
            if (buttonImage == null)
            {
                buttonImage = GetComponent<Image>();
            }
            if(hasText)
            {
                textMeshProUGUI = GetComponent<TextMeshProUGUI>();
            }

            SetOpacity(normalOpacity);  
        }

        
        public void OnPointerEnter(PointerEventData eventData)
        {
            if(!hasScale)
            {
              SetOpacity(hoverOpacity);
            }
            else
            {
                SetScale(hoverOpacity);
            }
           
        }

        
        public void OnPointerExit(PointerEventData eventData)
        {
            if (!hasScale)
            {
                SetOpacity(normalOpacity);
            }
            else
            {
                SetScale(normalOpacity);
            }
        }

        
        private void SetOpacity(float opacity)
        {
            if(!hasText)
            {
                Color color = buttonImage.color;
                color.a = opacity;
                buttonImage.color = color;
            }
            else
            {
                Color color = textMeshProUGUI.color;
                color.a = opacity;
                textMeshProUGUI.color = color;
            }

          
           
        }


        private void SetScale(float scale)
        {
            if(hasScale)
            {
                buttonImage.transform.localScale = new Vector3(scale,scale,scale);
            }
        }
    }
}
