using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public AudioSource AudioSource;
    public List<AudioClip> Clip;
    public GameObject prefab;



    public void StartSong(string soundName)
    {
         Clip.Clear();
         var music = Resources.Load<AudioClip>("Sounds/" + soundName);
         Clip.Add(music);
         AudioSource.clip = music;
         var pref = Instantiate(prefab);
         pref.GetComponent<AudioSource>().Play();
         Debug.Log("Spawn Sound : " + music.name);
    }

}


