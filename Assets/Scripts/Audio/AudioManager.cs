using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Audio;

public enum SoundType
{
    Music,
    SoundEffect
}

[System.Serializable]
public class Sound
{
    public string name;

    public string category;

    public AudioClip clip;

    public SoundType soundType;

    [Range(0f, 1f)]
    public float volume;

    public float pitch = 1f;

    public bool looping = true;

    public bool playOnStart = false;

    [HideInInspector]
    public AudioSource source;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance = null;

    public AudioMixerGroup musicMixer;
    public AudioMixerGroup sfxMixer;

    public Sound[] sounds;

    HashSet<string> enabledCategories = new HashSet<string>();
    HashSet<string> disabledCategories = new HashSet<string>();

    void Awake()
    {
        if(instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.looping;
            s.source.outputAudioMixerGroup = s.soundType == SoundType.Music ? musicMixer : sfxMixer;
            s.source.playOnAwake = s.playOnStart;

            enabledCategories.Add(s.category);
        }

        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        foreach (Sound s in sounds)
        {
            if (s.playOnStart)
            {
                Debug.Log(s.name);
                s.source.Play();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Play(string soundName)
    {
        Sound s = Array.Find(sounds, sound => sound.name == soundName && enabledCategories.Contains(sound.category));

        if (s == null)
        {
            Debug.LogError("Sound " + soundName + " does not exist");
            return;
        }

        if(disabledCategories.Contains(s.category))
        {
            return;
        }

        s.source.Play();
    }

    public void Stop(string soundName)
    {
        Sound s = Array.Find(sounds, sound => sound.name == soundName);

        if (s == null)
        {
            Debug.LogError("Sound " + soundName + " does not exist");
            return;
        }

        s.source.Stop();
    }

    public void UpdateSounds()
    {
        foreach (Sound s in sounds)
        {
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.looping;
        }
    }

    public Sound FindSoundByName(string soundName)
    {
        return Array.Find(sounds, sound => sound.name == soundName);
    }

    public bool IsPlaying(string soundName)
    {
        return FindSoundByName(soundName).source.isPlaying;
    }

    public void SetMasterVolume(float volume)
    {
        musicMixer.audioMixer.SetFloat("Volume", volume);
        sfxMixer.audioMixer.SetFloat("Volume", volume);
    }

    public void SetMusicVolume(float volume)
    {
        musicMixer.audioMixer.SetFloat("Volume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        sfxMixer.audioMixer.SetFloat("Volume", volume);
    }

    public void EnableCategory(string category)
    {
        disabledCategories.Remove(category);
        enabledCategories.Add(category);
    }

    public void DisableCategory(string category)
    {
        disabledCategories.Add(category);
        enabledCategories.Add(category);

        foreach (Sound s in sounds)
        {
            if(s.category == category && s.source.isPlaying)
            {
                s.source.Stop();
            }
        }
    }
}

