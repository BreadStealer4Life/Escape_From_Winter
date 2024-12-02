using UnityEngine;
using Winter.Assets.Project.Scripts.Runtime.Services.GamePause;

namespace Winter.Assets.Project.Scripts.Runtime.Core.FreezeSystem
{
    public class FreezeController : MonoBehaviour, IPauseGameListener, IResumeGameListener
    {
        [SerializeField] private float _timeToFreeze;

        public FreezeModel model;
        public float timer;
        private bool _isActive;

        public void Init(FreezeModel freezeModel)
        {
            model = freezeModel;
            timer = 0;
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
            timer += Time.deltaTime;
            float value = Mathf.Lerp(0, 1, timer / _timeToFreeze);

            model.SetFreezeValue(value);

            if (value >= 1)
                _isActive = false;
        }
    }
}
