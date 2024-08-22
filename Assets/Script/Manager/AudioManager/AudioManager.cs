using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{

    public static AudioManager instance;
    public Sound[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource;

    public float musicVolume = 1f;
    public float sfxVolume = 1f;
    public bool musicMuted = false;
    public bool sfxMuted = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            musicSource = gameObject.AddComponent<AudioSource>();
            sfxSource = gameObject.AddComponent<AudioSource>();

            LoadSettings();
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        
    }

    private void Start()
    {
        
        PlayMusicForCurrentScene();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayMusicForCurrentScene();
    }

    private void PlayMusicForCurrentScene()
    {
        if (TileManager.currentScene == "Menu")
        {
            PlayMusic("Menu Theme");
        }
        else
        {
            PlayMusic("Main Theme");
        }
    }

    public void StopMusic(string name)
    {
        Sound s = Array.Find(musicSounds, x => x.name == name);

        if (s == null)
        {
            Debug.LogError("Sound not found");
        }
        else
        {
            musicSource.clip = s.clip;
            musicSource.Stop();
        }
    }
    public void PlayMusic(string name)
    {
        Sound s = Array.Find(musicSounds, x => x.name == name);

        if (s == null)
        {
            Debug.LogError("Sound not found");
        }
        else
        {
            musicSource.clip = s.clip;
            musicSource.loop = s.loop;
            musicSource.Play();
        }

        
    }
    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == name);

        if (s == null)
        {
            Debug.LogError("Sound not found");
        }
        else
        {
            sfxSource.PlayOneShot(s.clip);
        }
    }
    public void ToggleMusic()
    {
        musicMuted = !musicMuted;
        musicSource.mute = musicMuted;
        SaveSettings();
    }
    public void ToggleFSX()
    {
        sfxMuted = !sfxMuted;
        sfxSource.mute = sfxMuted;
        SaveSettings();
    }
    public void MucsicVolume(float volume)
    {
        musicVolume = volume;
        musicSource.volume = volume;
        SaveSettings();
    }
    public void SFXVolume(float volume)
    {
        sfxVolume = volume;
        sfxSource.volume = volume;
        SaveSettings();
    }

    private void SaveSettings()
    {
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        PlayerPrefs.SetInt("MusicMuted", musicMuted ? 1 : 0);
        PlayerPrefs.SetInt("SFXMuted", sfxMuted ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void LoadSettings()
    {
        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            musicVolume = PlayerPrefs.GetFloat("MusicVolume");
            sfxVolume = PlayerPrefs.GetFloat("SFXVolume");
            musicMuted = PlayerPrefs.GetInt("MusicMuted") == 1;
            sfxMuted = PlayerPrefs.GetInt("SFXMuted") == 1;
        }
    }
}
