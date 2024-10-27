using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{
    public List<SettingChild>  settingChildren = new List<SettingChild>();
    public SettingParent settingParent;
    [Space]
    public GameObject prefabsSound;
    public BackGroundMusicController musicController;

    public Slider soundSlider;
    public Slider musicSlider;

    public float defalut = 1;

    public void Start()
    {
        prefabsSound.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("ChangeSound");
        musicController.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("ChangeMusic");

        soundSlider.value = prefabsSound.GetComponent<AudioSource>().volume;
        musicSlider.value = musicController.GetComponent<AudioSource>().volume;
    }

    public void ChangeSound()
    {
        prefabsSound.GetComponent<AudioSource>().volume = soundSlider.value;
        PlayerPrefs.SetFloat("ChangeSound", prefabsSound.GetComponent<AudioSource>().volume);
    }

    public void ChangeBackgroundMusic()
    {
        musicController.GetComponent<AudioSource>().volume = musicSlider.value;
        PlayerPrefs.SetFloat("ChangeMusic", musicController.GetComponent<AudioSource>().volume);
    }


    public void DefaultSlider()
    {
        soundSlider.value = defalut;
        musicSlider.value = defalut;
        ChangeSound();
        ChangeBackgroundMusic();
    }

    public void ActiveCurrentChild(int index)
    {
        settingParent.transform.localScale = Vector3.one;

        foreach (var child in settingChildren)
        {
            child.transform.localScale = Vector3.zero;
        }
        settingChildren[index].transform.localScale = Vector3.one;
        Time.timeScale = 0.000001f;
    }

    public void OpenSettings()
    {
        settingChildren[0].transform.localScale = Vector3.zero;
        settingChildren[2].transform.localScale = Vector3.zero;
        settingChildren[3].transform.localScale = Vector3.zero;
        settingChildren[1].transform.localScale = Vector3.one;
    }

    public void DisableMenu()
    {
        foreach (var child in settingChildren)
        {
            child.transform.localScale = Vector3.zero;
        }

        settingParent.transform.localScale = Vector3.zero;
        Time.timeScale = 1f;
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); 
#endif
    }
}
