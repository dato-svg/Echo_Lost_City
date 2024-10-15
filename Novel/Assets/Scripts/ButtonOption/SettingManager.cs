using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SettingManager : MonoBehaviour
{
    public List<SettingChild>  settingChildren = new List<SettingChild>();
    public SettingParent settingParent;


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
