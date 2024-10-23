using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class ScreenshotHandler : MonoBehaviour
{
    public GameObject Setting;
    public Image iconSlotSave;
    public Image iconSlotLoad;

    public SettingManager settingManager;
    public Button button;

    private void Start()
    {
        Setting = GameObject.Find("Settings");
        iconSlotSave = GetComponent<Image>();
        button = GetComponent<Button>();
        button.onClick.AddListener(ChangeIcon);
        settingManager = GameObject.Find("TesterDialoge").GetComponent<SettingManager>();
    }

    
    public void ChangeIcon()
    {
        StartCoroutine(CaptureScreenshot());
    }

    private IEnumerator CaptureScreenshot()
    {
        
        string fileName = $"Screenshot_{System.DateTime.Now:yyyy-MM-dd_HH-mm-ss}.png";
        string filePath = Path.Combine(Application.persistentDataPath, fileName);

        settingManager.DisableMenu();
        yield return new WaitForSeconds(0.00001f);
        ScreenCapture.CaptureScreenshot(filePath);
        Debug.Log($"Скриншот сохранен как {filePath}");
        yield return new WaitForSeconds(0.00001f);

        yield return new WaitForEndOfFrame();

        Texture2D texture = LoadTexture(filePath);


        if (texture != null)
        {
            iconSlotSave.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            iconSlotLoad.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }
        else
        {
            Debug.LogError("Не удалось загрузить текстуру.");
        }
        settingManager.ActiveCurrentChild(2);
    }

    private Texture2D LoadTexture(string filePath)
    {
        if (File.Exists(filePath))
        {
            byte[] fileData = File.ReadAllBytes(filePath);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(fileData); 
            return texture;
        }
        return null;
    }
}
