using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.LowLevel;

namespace Olechka
{
    [AddComponentMenu("Olechka scripts / Other / Timer")]
    public class Timer : MonoBehaviour
    {
        #region Variables

        [Tooltip("��������� ������ �����")]
        [SerializeField]
        bool Start_bool = false;

        [Tooltip("��������� ������")]
        [SerializeField]
        bool Loop_bool = false;

        [Tooltip("������ ��������")]
        [SerializeField]
        Timer_class[] Timer_array = new Timer_class[0];

        bool Active_bool = false;

        int Active_id_timer = 0;//����� ������ ������ ��������

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////

        #region MonoBehaviour Methods
        private void Start()
        {
            if (Start_bool)
            {
                StartCoroutine(Timer_coroutine());
            }
        }
        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////

        #region Methods
        IEnumerator Timer_coroutine()
        {
            Active_bool = true;

            while (true)
            {
                for (int x = 0; x < Timer_array.Length; x++)
                {
                    Timer_array[x].Start_event.Invoke();

                    Active_id_timer = x;

                    yield return new WaitForSeconds(Timer_array[x].Time);

                    Timer_array[x].End_event.Invoke();
                }

                if (!Loop_bool)
                {
                    Active_bool = false;
                    break;
                }
                
            }
        }
        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////

        #region Public Methods

        public void Stop_timer()
        {
            Active_bool = false;
            StopAllCoroutines();
        }

        public void Activation()
        {
            if (!Active_bool)
            {
                StartCoroutine(Timer_coroutine());
            }
        }
        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////

        #region Additionally

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////

        #region Gizmos

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////

        [System.Serializable]
        class Timer_class
        {
            [Tooltip("����� ������ �������")]
            [SerializeField]
            public float Time = 10f;

            [Tooltip("����� ������ �������")]
            [SerializeField]
            public UnityEvent Start_event = new UnityEvent();

            [Tooltip("����� ����� �������")]
            [SerializeField]
            public UnityEvent End_event = new UnityEvent();
        }
    }
}