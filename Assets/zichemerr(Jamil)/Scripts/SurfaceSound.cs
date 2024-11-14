using System.Collections.Generic;
using UnityEngine;

public class SurfaceSound : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private StepSounds[] _stepSounds;

    [Space(5), Header("Raycast settings")]
    [SerializeField] private Transform _point;
    [SerializeField] private float _distance;

    private RaycastHit _hitInfo;
    private StepSounds _currentStepSounds;
    private Surface _currentSurface;
    private Queue<AudioClip> _audioClips;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        _audioClips = new Queue<AudioClip>();
    }

    private void InitAudioClips()
    {
        Init();

        for (int i = 0; i < _currentStepSounds.ClipsCount; i++)
            _audioClips.Enqueue(_currentStepSounds.GetClipByIndex(i));
    }

    private void Update()
    {
        _currentSurface = GetSurface();

        if (_currentSurface == null || _currentSurface.Type == _currentStepSounds.SurfaceType)
            return;

        foreach (var stepSounds in _stepSounds)
        {
            if (_currentSurface.Type == stepSounds.SurfaceType)
            {
                _currentStepSounds = stepSounds;
                InitAudioClips();
            }
        }
    }

    private Surface GetSurface()
    {
        if (Physics.Raycast(_point.position, -_point.up, out _hitInfo, _distance))
            if (_hitInfo.collider.TryGetComponent(out Surface surface))
                return surface;

        return null;
    }

    private AudioClip GetAudioClip()
    {
        if (_audioClips.Count > 0)
            return _audioClips.Dequeue();

        InitAudioClips();
        return _audioClips.Dequeue();
    }

    public void Play()
    {
        if (_currentSurface == null)
            return;

        _audioSource.clip = GetAudioClip();
        _audioSource.Play();
    }
}
