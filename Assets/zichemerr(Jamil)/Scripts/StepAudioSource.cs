using System.Collections.Generic;
using UnityEngine;

public class StepAudioSource : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private StepSounds _stepSound;

    private Queue<AudioClip> _audioClips = new Queue<AudioClip>();

    public void InitAudioClips(StepSounds stepSounds)
    {
        _audioClips = new Queue<AudioClip>();

        for (int i = 0; i < stepSounds.ClipsCount; i++)
            _audioClips.Enqueue(stepSounds.GetClipByIndex(i));
    }

    private AudioClip GetAudioClip(StepSounds stepSounds)
    {
        if (_audioClips.Count > 0)
            return _audioClips.Dequeue();

        InitAudioClips(stepSounds);
        return _audioClips.Dequeue();
    }

    public virtual void Play()
    {
        PlayAndSetClip(_stepSound);
    }

    public void PlayAndSetClip(StepSounds stepSounds)
    {
        _audioSource.clip = GetAudioClip(stepSounds);
        _audioSource.Play();
    }
}
