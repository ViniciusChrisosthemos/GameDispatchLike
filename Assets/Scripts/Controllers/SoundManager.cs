using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] private List<AudioSource> _sfxAudioSources;
    [SerializeField] private AudioSource _musicAudioSource;

    private int _currentSFXAudioSource = 0;

    public void PlaySFX(AudioClip clip, float volume)
    {
        _sfxAudioSources[_currentSFXAudioSource].clip = clip;
        _sfxAudioSources[_currentSFXAudioSource].volume = volume;
        _sfxAudioSources[_currentSFXAudioSource].Play();

        _currentSFXAudioSource = (_currentSFXAudioSource + 1) % _sfxAudioSources.Count;
    }

    public void PlayMusic(AudioClip battleTheme, float volume, bool loop)
    {
        PlayMusic(battleTheme, volume, loop, _musicAudioSource);
    }

    public void PlayMusic(AudioClip battleTheme, float volume, bool loop, AudioSource audioSource)
    {
        audioSource.clip = battleTheme;
        audioSource.loop = loop;
        audioSource.volume = volume;
        audioSource.Play();
    }

    internal void PlaySFX(AudioClip sFXAudio, object sFXVolume)
    {
        throw new NotImplementedException();
    }
}