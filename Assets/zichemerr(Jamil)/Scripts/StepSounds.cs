using System;
using UnityEngine;

[Serializable]
public struct StepSounds
{
    [SerializeField] private AudioClip[] _clips;
    [SerializeField] private SurfaceType _surfaceType;

    public int ClipsCount => _clips.Length;
    public SurfaceType SurfaceType => _surfaceType;

    public AudioClip GetClipByIndex(int index)
    {
        return _clips[index];
    }
}