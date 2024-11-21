using UnityEngine;

public class SurfaceSound : StepAudioSource
{
    [SerializeField] private StepSounds[] _stepSounds;

    private RaycastHit _hitInfo;
    private StepSounds _currentStepSounds;
    private Surface _currentSurface;

    private void Awake()
    {
        _currentStepSounds = _stepSounds[0];
    }

    private void SetCurrentStep()
    {
        _currentSurface = GetSurface();

        if (_currentSurface == null || _currentStepSounds.SurfaceType == _currentSurface.Type)
            return;

        foreach (var stepSounds in _stepSounds)
        {
            if (stepSounds.SurfaceType == _currentSurface.Type)
            {
                _currentStepSounds = stepSounds;
                InitAudioClips(_currentStepSounds);
            }
        }
    }

    private Surface GetSurface()
    {
        if (Physics.Raycast(transform.position, -transform.up, out _hitInfo, 5))
            if (_hitInfo.collider.TryGetComponent(out Surface surface))
                return surface;

        return null;
    }

    public override void Play()
    {
        SetCurrentStep();

        if (_currentSurface == null)
            return;

        PlayAndSetClip(_currentStepSounds);
    }
}