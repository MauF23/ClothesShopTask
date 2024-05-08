using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Sirenix.OdinInspector;
using DG.Tweening;

public class SoundManager : MonoBehaviour
{
    public List<Sound> soundList;
    private const float defaultFadeTime = 0.5f;
    public static SoundManager instance;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }

        for(int i = 0; i < soundList.Count; i++)
        {
            Sound sound = soundList[i];
            sound.SetAudioSource(gameObject.AddComponent<AudioSource>());
            sound.source.clip = sound.audioClip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
            sound.source.playOnAwake = sound.playOnAwake;
        }
    }

    public void PlaySound(string name)
    {
        Sound sound = GetSound(name);
        if (sound != null)
        {
            sound.source.Play();
        }
    }

    public void StopSound(string name)
    {
        Sound sound = GetSound(name);
        if (sound != null)
        {
            sound.source.Stop();
        }
    }

    #region FadeFunctions
    public void FadeInSound(string name)
    {
        Sound sound = GetSound(name);

        if (sound != null)
        {
            TweenSoundVolume(sound, sound.volume, defaultFadeTime, true, null);
            sound.source.Play();
        }
    }

    public void FadeInSound(string name, float fadeTime)
    {
        Sound sound = GetSound(name);

        if (sound != null)
        {
            TweenSoundVolume(sound, sound.volume, fadeTime, true, null);
            sound.source.Play();
        }
    }

    public void FadeOutSound(string name)
    {
        Sound sound = GetSound(name);

        if (sound != null)
        {
            TweenSoundVolume(sound, 0, defaultFadeTime, false, sound.source.Stop);
        }
    }

    public void FadeOutSound(string name, float fadeTime)
    {
        Sound sound = GetSound(name);

        if (sound != null)
        {
            TweenSoundVolume(sound, 0, fadeTime, false, sound.source.Stop);
        }
    }
    #endregion

    #region Utilities
    private Sound GetSound(string name)
    {
        Sound sound = soundList.FirstOrDefault(sound => sound.audioName == name);

        if(sound == null)
        {
            Debug.LogError($"<color=red>sound with name {name} does not exists, add it or check the spelling</color>");
            return null;
        }

        return sound;
    }

    private void TweenSoundVolume(Sound sound, float endVolume, float tweenTime, bool resetVolumeBeforeTween, Action callback)
    {
        if (resetVolumeBeforeTween)
        {
            sound.source.volume = 0;
        }
        sound.source.DOFade(endVolume, tweenTime).SetUpdate(UpdateType.Normal, true).OnComplete(delegate { callback.Invoke(); });
    }
    #endregion
}

[System.Serializable]
public class Sound
{
    public string audioName;

    public AudioClip audioClip;

    [Range(0, 1)]
    public float volume = 1;

    [Range(0, 3)]
    public float pitch = 1;

    public bool loop;

    public bool playOnAwake;

    [HideInInspector]
    public AudioSource source;

    public void SetAudioSource(AudioSource audioSource)
    {
        source = audioSource;
    }
}


