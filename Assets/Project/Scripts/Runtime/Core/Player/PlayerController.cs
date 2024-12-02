using Shark.Systems.Checkpoints;
using System;
using UnityEngine;
using UnityEngine.UI;
using Winter.Assets.Project.Scripts.Runtime.Core.Player.Data;
using Winter.Assets.Project.Scripts.Runtime.Core.TriggerObservable;
using Winter.Assets.Project.Scripts.Runtime.Services.Audio;
using Winter.Assets.Project.Scripts.Runtime.Services.GamePause;
using Winter.Assets.Project.Scripts.Runtime.Services.Input;

namespace Winter.Assets.Project.Scripts.Runtime.Core.Player
{
    public class PlayerController : MonoBehaviour, IPauseGameListener, IResumeGameListener
    {
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private SlipperyTriggerObserver _slipperyTriggerObserver;
        [SerializeField] private ClimbingRockTriggerObserver _climbingRockTriggerObserver;
        [SerializeField] private Transform _bobCamera;
        [SerializeField] private Transform _motorCamera;

        [SerializeField] 
        private Transform MotorCamera_climbing ;

        [SerializeField]
        private GameObject Main_camera = null;

        bool Climbing_bool = false;

        private PlayerCameraService CameraController_climbing;

        [SerializeField] private Transform _motorObject;

        [Header("Hands")]
        [SerializeField] private GameObject _thermometer;
        [SerializeField] private GameObject _lighter;
        [SerializeField] private GameObject _leftIcePick;
        [SerializeField] private GameObject _rightIcePick;

        [SerializeField]
        Animator LeftIcePick_animator = null;

        [SerializeField]
        Animator RightIcePick_animator = null;
        
        bool Add_ice_axes_bool = false;

        int Id_active_IcePick = 0;

        //[Header("Temporary UI elements")]
        //[SerializeField] 
        //private Slider _enduranceSlider;

        private PlayerData _data;
        internal PlayerMotorService _motorController;
        private PlayerCameraService _cameraController;
        private PlayerHeadBobService _headBobController;
        private InputHandler _inputHandler;
        private SoundsPlayer _audioService;

        private bool _isPlayerOnSlipperySurface;
        internal bool _isPlayerOnClimbingRockSurface;
        private bool _isPlayerReadyToClimbing;
        private bool _isControllerActive;
        Vector3 _climbingRockWallNormal;

        [Header("Ограничение по карабканью")]
        [SerializeField]
        float Height_dist = 4f;

        [SerializeField]
        float Width_dist = 4f;

        [SerializeField]
        float Diagonally_dist = 4f;

        [SerializeField]
        float Radius_dist = 0.6f;

        [SerializeField]
        LayerMask Layer_climbing = 1;

        [SerializeField]
        Vector3 OffSet = Vector3.zero;

        [Tooltip("Включить отображения Gizmos")]
        [SerializeField]
        bool Gizmos_mode_bool = false;

        public void Init(InputHandler inputHandler, PlayerData playerData, SoundsPlayer audioService)
        {
            _data = playerData;
            _inputHandler = inputHandler;
            _audioService = audioService;

            _motorController = new PlayerMotorService(_characterController, _motorCamera, _data);
            _cameraController = new PlayerCameraService(_motorCamera, _motorObject, _data);

            CameraController_climbing = new PlayerCameraService(MotorCamera_climbing, MotorCamera_climbing.transform.parent, _data);

            _headBobController = new PlayerHeadBobService(_bobCamera, _data);

            _data.SmoothMoveDeltaTime = _data.DefaultSmoothMoveDeltaTime;
            
            _slipperyTriggerObserver.Enter += OnSlipperyTriggerEnter;
            _slipperyTriggerObserver.Exit += OnSlipperyTriggerExit;

            _climbingRockTriggerObserver.Enter += OnClimbingTriggerEnter;
            _climbingRockTriggerObserver.Exit += OnClimbingTriggerExit;

            _isControllerActive = true;
            _headBobController.PlayStepSound += _audioService.PlayStepSound;

            _inputHandler.InstrumentSwitched = OnInstrumentSwitched;
            _motorController.ClimbingEnduranceUpdated = UpdateClimbingEndurance;

            _motorController.ClimbingEnduranceUpdated += Start_Climbing;
        }

        public void OnPauseGame() => _isControllerActive = false;

        public void OnResumeGame() => _isControllerActive = true;

        private void OnDestroy()
        {
            //_headBobController.PlayStepSound -= _audioService.PlayStepSound;

            _slipperyTriggerObserver.Enter -= OnSlipperyTriggerEnter;
            _slipperyTriggerObserver.Exit -= OnSlipperyTriggerExit;

            _climbingRockTriggerObserver.Enter -= OnClimbingTriggerEnter;
            _climbingRockTriggerObserver.Exit -= OnClimbingTriggerExit;
        }

        private void Update()
        {
            if (!_isControllerActive)
                return;


            if (_isPlayerOnClimbingRockSurface && _isPlayerReadyToClimbing)
            {

                if(!Check_obstacle_climbing(_inputHandler.MovementInput))
                    _motorController.Climbing(_inputHandler.MovementInput, _inputHandler.IcePickSwingingInput, _inputHandler.JumpState, _climbingRockWallNormal);
                else if (_inputHandler.JumpState)
                {
                    _motorController.Climbing(Vector2.down, 0, _inputHandler.JumpState, _climbingRockWallNormal);
                }

                if (Id_active_IcePick != (int)_inputHandler.IcePickSwingingInput) 
                {
                    if (_inputHandler.IcePickSwingingInput == 1)
                    {
                        LeftIcePick_animator.CrossFade("Active", 0, 0, 0);
                        Id_active_IcePick = (int)_inputHandler.IcePickSwingingInput;
                    }

                    else if (_inputHandler.IcePickSwingingInput == -1)
                    {
                        RightIcePick_animator.CrossFade("Active", 0, 0, 0);
                        Id_active_IcePick = (int)_inputHandler.IcePickSwingingInput;
                    }

                    
                }
            }
            else
            {
                _motorController.Move(_inputHandler.MovementInput, _inputHandler.JumpState);
                _motorController.SetCrouch(_inputHandler.CrouchState);
                _motorController.SetSprint(_inputHandler.SprintState);
            }

            if (!Climbing_bool)
                _cameraController.RotateCamera(_inputHandler.RotationInput);
            else
                CameraController_climbing.RotateCamera(_inputHandler.RotationInput);

            _headBobController.UpdateHeadBob(_inputHandler.MovementInput, _characterController.isGrounded, _isPlayerOnSlipperySurface);
        }

        bool Check_obstacle_climbing(Vector2 _direct)
        {

                bool result = false;

            RaycastHit hit;

            Vector3 direction = transform.right * _direct.x + transform.up * _direct.y;
            direction.Normalize();

            float dist = 0;

            if (_direct.x != 0 && _direct.y <= 0)
                dist = Width_dist;
            else if (_direct.x == 0 && _direct.y > 0)
                dist = Height_dist;
            else if (_direct.x != 0 && _direct.y > 0)
                dist = Diagonally_dist;

            if (direction.y >= 0)
            {
                if (Physics.SphereCast(transform.position + OffSet, Radius_dist, direction, out hit, dist, Layer_climbing, QueryTriggerInteraction.Ignore))
                {
                    result = true;
                }

            }
            else
            {
                result = true;
            }

            /*
                switch (_direct)
                {
                    case Vector2 v when v.Equals(Vector2.up):
                        if (Physics.SphereCast(transform.position, 0.3f, Vector3.up, out hit, Height_dist, Layer_climbing, QueryTriggerInteraction.Ignore))
                        {
                            result = true;
                        }

                        break;

                    case Vector2 v when v.Equals(Vector2.right):
                        if (Physics.SphereCast(transform.position, 0.3f, transform.right, out hit, Width_dist, Layer_climbing, QueryTriggerInteraction.Ignore))
                            result = true;
                        break;

                    case Vector2 v when v.Equals(Vector2.left):
                        if (Physics.SphereCast(transform.position, 0.3f, transform.right * -1, out hit, Width_dist, Layer_climbing, QueryTriggerInteraction.Ignore))
                            result = true;
                        break;

                    case Vector2 v when v.Equals(Vector2.left):
                        if (Physics.SphereCast(transform.position, 0.3f, transform.right * -1, out hit, Width_dist, Layer_climbing, QueryTriggerInteraction.Ignore))
                            result = true;
                        break;
                }
                */
            return result;
        }

        public void Add_Ice_axes()
        {
            Add_ice_axes_bool = true;

            OnInstrumentSwitched();
        }

        void Start_Climbing(float _value)
        {
            if (Climbing_bool == false && _value < 500)
            {
                Preparation_camera(true);
            }
        }

        void Preparation_camera(bool _climb)
        {
            //if (!_isPlayerOnClimbingRockSurface)
            if(!_climb)
            {
                if (Climbing_bool == true)
                {

                    MotorCamera_climbing.gameObject.SetActive(false);
                    Main_camera.SetActive(true);

                    _motorObject.rotation = MotorCamera_climbing.transform.parent.rotation;
                    _cameraController._yRotation = CameraController_climbing._yRotation;

                    Climbing_bool = false;
                }
            }

            else
            {
                if (Climbing_bool == false)
                {

                    Quaternion character_rotation = _motorObject.rotation;

                    _motorObject.rotation = Quaternion.LookRotation(_climbingRockWallNormal);//Quaternion.Euler(_climbingRockWallNormal.x, _climbingRockWallNormal.y + 180f, _climbingRockWallNormal.z);

                    _motorCamera.rotation = _motorObject.rotation;


                    MotorCamera_climbing.gameObject.SetActive(true);
                    Main_camera.SetActive(false);

                    MotorCamera_climbing.transform.parent.rotation = character_rotation;
                    CameraController_climbing._yRotation = _cameraController._yRotation;

                    Climbing_bool = true;
                }
            }
        }


        private void OnSlipperyTriggerEnter(Collider collider)
        {
            _isPlayerOnSlipperySurface = true;
            _data.SmoothMoveDeltaTime = _data.SlipperySmoothMoveDeltaTime;
        }

        private void OnSlipperyTriggerExit(Collider collider)
        {
            _isPlayerOnSlipperySurface = false;
            _data.SmoothMoveDeltaTime = _data.DefaultSmoothMoveDeltaTime;
        }

        private void OnInstrumentSwitched()
        {
            if (Add_ice_axes_bool) 
            {
                _isPlayerReadyToClimbing = !_isPlayerReadyToClimbing;

                if (_isPlayerOnClimbingRockSurface) _isPlayerReadyToClimbing = true;

                _leftIcePick.SetActive(_isPlayerReadyToClimbing);
                _rightIcePick.SetActive(!_isPlayerReadyToClimbing);

                _thermometer.SetActive(!_isPlayerReadyToClimbing);
                _lighter.SetActive(!_isPlayerReadyToClimbing);
            }
        }

        private void OnClimbingTriggerEnter(Collider collider)
        {
            _isPlayerOnClimbingRockSurface = true;
            _climbingRockWallNormal = collider.transform.forward;
        }

        private void OnClimbingTriggerExit(Collider collider)
        {
            _isPlayerOnClimbingRockSurface = false;
            _climbingRockWallNormal = Vector3.zero;

            Preparation_camera(false);
        }

        public void UpdateClimbingEndurance(float value)
        {
            //_enduranceSlider.maxValue = _data.ClimbingEnduranceMaximum;
            //_enduranceSlider.value = value;
        }

        void OnDrawGizmos()
        {
            if (Gizmos_mode_bool)
            {
                Vector3 start_point = transform.position;

                //Gizmos.DrawLine(start_point, start_point + Vector3.up * Height_dist);
                //Gizmos.DrawLine(start_point, start_point + transform.up * -1 * Height_dist);
                //Gizmos.DrawLine(start_point, start_point + transform.right * Width_dist);

                Vector3 direct = Vector3.zero;

                for (int x = 0; x < 5; x++)
                {
                    switch (x)
                    {
                        case 0:
                            direct = transform.right * 1 + transform.up * 0 * Width_dist;
                            break;

                        case 1:
                            direct = transform.right * -1 + transform.up * 0 * Width_dist;
                            break;

                        case 2:
                            direct = transform.right * 0 + transform.up * 1 * Height_dist;
                            break;

                        case 3:
                            direct = transform.right * 1 + transform.up * 1 * Diagonally_dist;
                            break;

                        case 4:
                            direct = transform.right * -1 + transform.up * 1 * Diagonally_dist;
                            break;
                    }

                    Gizmos.DrawLine(start_point, start_point + direct);
                    Gizmos.DrawSphere(start_point + OffSet + direct, Radius_dist);
                }
            }
        }

        internal void Spawn(Vector3 position, Quaternion quaternion)
        {
            _characterController.enabled = false;
            transform.SetPositionAndRotation(position, quaternion);
            _characterController.enabled = true;
        }
    }
}
