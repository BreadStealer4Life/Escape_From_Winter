using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;
using Winter;
using Winter.Assets.Project.Scripts.Runtime.Core.Player;
using Winter.Assets.Project.Scripts.Runtime.Services.Input;

namespace Olechka
{
    public class Stamina : MonoBehaviour
    {
        #region Variables

        [Tooltip("Максимальное значение стамины")]
        [SerializeField]
        float Value = 10f;

        float Value_active = 0f;

        UI_stamina UI_stamina_script = null;

        [Tooltip("Скрипт лазанья на ледорубах")]
        [SerializeField]
        ClimbingRockTriggerObserver ClimbingRockTriggerObserver_script = null;

        [SerializeField]
        PlayerController PlayerController_script = null;

        [Foldout("Ивенты")]
        [Tooltip("Активируется стамина")]
        [SerializeField]
        UnityEvent Activation_event = new UnityEvent();

        //[Foldout("Ивенты")]
        //[Tooltip("Стамина закончилась")]
        //[SerializeField]
        //UnityEvent Off_event = new UnityEvent();

        bool Active_bool = false;

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region MonoBehaviour Methods
        private void Start()
        {
            Value_active = Value;

            if (ClimbingRockTriggerObserver_script)
            {
                ClimbingRockTriggerObserver_script.Enter += Activation;
                ClimbingRockTriggerObserver_script.Exit += Off;
            }

            if (UI_stamina.Singleton_Instance)
                UI_stamina_script = UI_stamina.Singleton_Instance;
        }

        private void Update()
        {
            if (Active_bool)
            {
                Down_value();
            }
        }
        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////

        #region Methods

        void Activation(Collider _col)
        {
            Activity(true);
        }

        void Off(Collider _col)
        {
            Activity(false);
        }

        void Down_value()
        {
            if(Value_active > 0)
            {
                Value_active -= Time.deltaTime;
                UI_stamina_script.Change_value(Value_active / Value);
            }
            else
            {
                //Off_event.Invoke();
                PlayerController_script._motorController.Climbing(Vector2.zero, 0, true, Vector3.zero);
                PlayerController_script._isPlayerOnClimbingRockSurface = false;
                //Active_bool = false;

            }
                
        }
        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////

        #region Public Methods
        public void Activity(bool _activity)
        {
                if (_activity)
                {
                    Activation_event.Invoke();
                    Value_active = Value;
                }

                UI_stamina_script.Activity(_activity);

                Active_bool = _activity;
        }
        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////

        #region Additionally

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////

        #region Gizmos

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////


    }
}