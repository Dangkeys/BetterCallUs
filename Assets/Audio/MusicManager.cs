using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    const string PLAYER_PREFS_MUSIC_VOLUME = "MusicVolume";
    public static MusicManager Instance {get; private set;} 
    AudioSource audioSource;
    void Awake()
    {
        Instance = this;
        audioSource = GetComponent<AudioSource>();
        volume = PlayerPrefs.GetFloat(PLAYER_PREFS_MUSIC_VOLUME, .3f);
        audioSource.volume = volume;
    }
    float volume = .3f;
    public void ChangeVolume()
    {
        volume += .1f;
        if(volume > 1f)
        {
            volume = 0f;
        }
        audioSource.volume = volume;
        PlayerPrefs.SetFloat(PLAYER_PREFS_MUSIC_VOLUME, volume);
        PlayerPrefs.Save();
    }
    public float GetVolume()
    {
        return volume;
    }
}
