using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ScreenshotHandler : MonoBehaviour
{
    public Image iconSlotSave;
    public Image iconSlotLoad;

    public SettingManager settingManager;
    public Button button;

    public SaveLoadManager saveload;

    private string screenshotPath;
    
    public string gameObjectName;
    


    private  void Start()
    {
        gameObjectName = gameObject.name;
        iconSlotSave = GetComponent<Image>();
        button = GetComponent<Button>();
        button.onClick.AddListener(ChangeIcon);
        settingManager = GameObject.Find("TesterDialoge").GetComponent<SettingManager>();
        saveload = settingManager.GetComponent<SaveLoadManager>();

        
        LoadIcon();
    }

    public void ChangeIcon()
    {
     
        StartCoroutine(CaptureScreenshot());
    }

    private IEnumerator CaptureScreenshot()
    {
        saveload.cunnentSave = GetComponent<ScreenshotHandler>();
        yield return new WaitForSeconds(0.5f);
        if (saveload.canChange)
        {
            string fileName = $"Screenshot_{System.DateTime.Now:yyyy-MM-dd_HH-mm-ss}.png";
            screenshotPath = Path.Combine(Application.persistentDataPath, fileName);

            settingManager.DisableMenu();
            yield return new WaitForSeconds(0.00001f);
            ScreenCapture.CaptureScreenshot(screenshotPath);
            Debug.Log($"Скриншот сохранен как {screenshotPath}");

            yield return new WaitForSeconds(0.1f);
            Texture2D texture = LoadTexture(screenshotPath);
            yield return new WaitForSeconds(0.1f);

            if (texture != null)
            {
                Sprite screenshotSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                iconSlotSave.sprite = screenshotSprite;
                iconSlotLoad.sprite = screenshotSprite;


                PlayerPrefs.SetString("ScreenshotPath1" + gameObjectName, screenshotPath);
                PlayerPrefs.Save();
            }
            else
            {
                Debug.Log("Не удалось загрузить текстуру.");
            }
            saveload.cunnentSave = null;
        }

        else 
        {
            Debug.Log("nothing");
        }
        
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

    
    private void LoadIcon()
    {
        if (PlayerPrefs.HasKey("ScreenshotPath1"+ gameObjectName))
        {
            screenshotPath = PlayerPrefs.GetString("ScreenshotPath1" + gameObjectName);

            if (File.Exists(screenshotPath))
            {
                Texture2D texture = LoadTexture(screenshotPath);
                if (texture != null)
                {
                    Sprite loadedSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                    iconSlotSave.sprite = loadedSprite;
                    iconSlotLoad.sprite = loadedSprite;
                }
                else
                {
                    Debug.Log("Не удалось загрузить текстуру при восстановлении.");
                }
            }
            else
            {
                Debug.Log("Сохраненный скриншот не найден.");
            }
        }
    }
}
