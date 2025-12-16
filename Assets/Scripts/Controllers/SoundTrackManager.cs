using UnityEngine;

public class SoundTrackManager : Singleton<SoundTrackManager>
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private float _volume = 1f;

    public void PlaySoundTrack(AudioClip clip, bool loop)
    {
        SoundManager.Instance.PlayMusic(clip, _volume, loop, _audioSource);
    }
}
