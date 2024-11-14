using UnityEngine;
using UnityEngine.Audio;
using Winter.Assets.Project.Scripts.Runtime.Services.Settings;

namespace Winter.Assets.Project.Scripts.Runtime.Services.Audio
{
    public class SoundsPlayer : MonoBehaviour
    {
        [SerializeField] private SurfaceSound _sound;
        [SerializeField] private AudioMixer _mixer;

        private void Start()
        {
            DataProvider.Instance.GameConfig.SoundsVolumeChanged += SetSoundsVolume;

            if(_mixer)
            _mixer.SetFloat("SoundsVolume", DataProvider.Instance.GameConfig.SoundsVolume);
        }

        private void OnDestroy()
        {
            DataProvider.Instance.GameConfig.SoundsVolumeChanged -= SetSoundsVolume;
        }

        public void PlayStepSound()
        {
            _sound.Play();
        }

        private void SetSoundsVolume()
        {
            if (_mixer)
                _mixer.SetFloat("SoundsVolume", DataProvider.Instance.GameConfig.SoundsVolume);
        }
    }
}
