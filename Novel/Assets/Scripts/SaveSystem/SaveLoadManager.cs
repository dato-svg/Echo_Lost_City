using System.IO;
using UnityEngine;


public class SaveLoadManager : MonoBehaviour
{
    public DialogueManager dialogueManager; 
    public AudioSource backgroundMusic;   

    private string saveDirectoryPath;

    private void Start()
    {
       
        saveDirectoryPath = Path.Combine(Application.persistentDataPath, "Saves");
        if (!Directory.Exists(saveDirectoryPath))
        {
            Directory.CreateDirectory(saveDirectoryPath);
        }
    }

    
    public void SaveGame(int slot)
    {

        GameSaveDataInfo saveData = new GameSaveDataInfo
        {
            dialogueID = dialogueManager.currentDialogue,
            backgroundMusicTime = backgroundMusic.time,
            backgroundMusicID = backgroundMusic.clip.name
        };

        string json = JsonUtility.ToJson(saveData);
        string filePath = GetSaveFilePath(slot);

        File.WriteAllText(filePath, json);
        Debug.Log("Игра сохранена в слот " + slot);
    }

    
    public void LoadGame(int slot)
    {
        string filePath = GetSaveFilePath(slot);

        if (!File.Exists(filePath))
        {
            Debug.LogError("Нет сохранения в этом слоте!");
            return;
        }

        string json = File.ReadAllText(filePath);
        GameSaveDataInfo loadData = JsonUtility.FromJson<GameSaveDataInfo>(json);
        dialogueManager.StartDialogue(loadData.dialogueID);
        
     
        AudioClip clip = Resources.Load<AudioClip>(loadData.backgroundMusicID);
        if (clip != null)
        {
            backgroundMusic.clip = clip;
            backgroundMusic.time = loadData.backgroundMusicTime;
            backgroundMusic.Play();
        }

        Debug.Log("Игра загружена из слота " + slot);
    }

  
    private string GetSaveFilePath(int slot)
    {
        return Path.Combine(saveDirectoryPath, $"SaveSlot_{slot}.json");
    }


}
