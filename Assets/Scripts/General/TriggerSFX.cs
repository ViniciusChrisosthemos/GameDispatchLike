using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSFX : MonoBehaviour
{
    [Header("SFX")]
    [SerializeField] private float _volume;
    [SerializeField] private List<AudioClip> _clip;

    [Header("Music")]
    [SerializeField] private bool _playMusicInLoop;

    public void PlaySFX()
    {
        var index = Random.Range(0, _clip.Count);
        var clip = _clip[index];

        SoundManager.Instance.PlaySFX(clip, _volume);
    }

    public void PlayMusic()
    {
        var index = Random.Range(0, _clip.Count);
        var clip = _clip[index];
        SoundManager.Instance.PlayMusic(clip, _volume, _playMusicInLoop);
    }
}