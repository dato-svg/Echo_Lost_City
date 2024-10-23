using TMPro;
using UnityEngine;

public class ButtonHistory : MonoBehaviour
{
    public TextMeshProUGUI namePRO;
    public TextMeshProUGUI textPRO;

    public string name;
    public string text;


   

    public void Start()
    {
        namePRO.text = name.ToString();
        textPRO.text = text.ToString();
        transform.localScale = Vector3.one;
    }

}
