using UnityEngine;
using Winter.Assets.Project.Scripts.Runtime.Services.GamePause;

namespace Winter.Assets.Project.Scripts.Runtime.Core.FreezeSystem
{
    public class FreezeController : MonoBehaviour, IPauseGameListener, IResumeGameListener
    {
        [SerializeField] private float _timeToFreeze;

        private float _currentFreezeTime;
        private bool _isActive;
        public FreezeModel model;

        public void Init(FreezeModel freezeModel)
        {
            model = freezeModel;
            _currentFreezeTime = 0;
        }

        public void OnPauseGame() => _isActive = false;
        public void OnResumeGame() => _isActive = true;

        private void Update()
        {
            if (_isActive)
                UpdateFreezeTime();
        }

        private void UpdateFreezeTime()
        {
            _currentFreezeTime += Time.deltaTime;
            float value = Mathf.Lerp(0, 1, _currentFreezeTime / _timeToFreeze);

            model.SetFreezeValue(value);

            if (value >= 1)
                _isActive = false;
        }
    }
}
