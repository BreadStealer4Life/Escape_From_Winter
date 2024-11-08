//�������� �� ����� ��������� ���������
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;
using UnityEngine.EventSystems;
using NaughtyAttributes;
using UnityEngine.Events;

namespace Olechka
{
    [AddComponentMenu("Sych scripts / Game / Base / Brain SM")]
    [DisallowMultipleComponent]
    public abstract class Abstract_Brain_SM : MonoBehaviour
    {
        #region ����������
        [field: Tooltip("������ �� �������� ������ ��������� (��� ����� ���� ���� �������!)")]
        [field: SerializeField]
        protected Character_abstract Main_character { get; private set; } = null;

        [Tooltip("������ ��������� ��������� (���������������� (�����������) ������ ������ ����� ������ ���������)")]
        [SerializeField]
        protected State_abstract[] State_array = new State_abstract[0];

        protected StateMachine State_Machine { get; private set; } = null;

        [Tooltip("����� ���������� ��� ���������� Update")]
        [SerializeField]
        float Time_slow_update = 0.1f;

        [Tooltip("����� ���������� ��� ���������� Update_2 (������)")]
        [SerializeField]
        float Time_slow_update_2 = 0.5f;

        Coroutine Slow_update_coroutine = null;
        Coroutine Slow_update_coroutine_2 = null;

        protected bool Active_bool = true;

        [Tooltip("������������ ��� ������ �����")]
        [SerializeField]
        bool Start_bool = true;

        [Tooltip("������������ 0-� ��������� (����� ���������)")]
        [SerializeField]
        protected bool Start_zero_state = true;

        [Foldout("������")]
        [Tooltip("����� ���������")]
        [SerializeField]
        UnityEvent Activation_event = new UnityEvent();

        [Foldout("������")]
        [Tooltip("����� ��� ���������� ���������")]
        [SerializeField]
        UnityEvent Off_states_event = new UnityEvent();

        bool Initialization_bool = false;

        #endregion


        #region MonoBehaviour Callbacks
        protected virtual void Start()
        {
            State_Machine = new StateMachine();

            if(Start_bool)
                Activation();
        }


        protected virtual void Update()
        {
            if (Initialization_bool && Active_bool && State_Machine.Current_State != null)
            {
                State_Machine.Current_State.Logic_Update();
            }

        }

        protected virtual void FixedUpdate()
        {
            if (Initialization_bool && Active_bool && State_Machine.Current_State != null)
            {
                State_Machine.Current_State.Physics_Update();
            }
        }

        #endregion


        #region ������


        protected virtual IEnumerator Coroutine_Initialized_State_machine()
        {

            Slow_update_coroutine = StartCoroutine(Coroutine_Slow_update());
            Slow_update_coroutine_2 = StartCoroutine(Coroutine_Slow_update_2());
            
            yield return new WaitForSeconds(0);

            Initialization_bool = true;
        }


        IEnumerator Coroutine_Slow_update()
        {
            while (true)
            {
                Slow_update();

                yield return new WaitForSeconds(Time_slow_update);
            }

        }

        IEnumerator Coroutine_Slow_update_2()
        {
            while (true)
            {
                Slow_update_2();

                yield return new WaitForSeconds(Time_slow_update_2);
            }

        }

        protected virtual void Slow_update()
        {
            if (Active_bool && State_Machine.Current_State != null)
            {
                State_Machine.Current_State.Slow_Update();
            }
        }

        protected virtual void Slow_update_2()
        {
            if (Active_bool && State_Machine.Current_State != null)
            {
                State_Machine.Current_State.Slow_Update_2();
            }
        }

        #endregion


        #region ����������� ������
        /// <summary>
        /// �������� ��������� ��������� �� �����
        /// </summary>
        /// <param name="_name">��� ���������</param>
        public void Change_state(string _name)
        {
            bool result = false;
            
            for (int x = 0; x < State_array.Length; x++)
            {
                if (State_array[x].Name_state == _name)
                {
                    Change_state(x);

                    result = true;
                    
                    break;
                }
            }

            if(!result)
            Debug.LogError("��������� ��������� �� �����  " + _name + "  �� �������!");
        }

        /// <summary>
        /// �������� ��������� ��������� �� id
        /// </summary>
        /// <param name="_name">��� ���������</param>
        public void Change_state(int _id)
        {
            if(State_array.Length - 1 < _id || _id < 0)
            Debug.LogError("��������� ��������� �� id  " + _id + "  �� ����������!");
            
            if (State_Machine.Current_State != State_array[_id])
            {
                //print(_id);
                State_Machine.Change_State(State_array[_id]);
                
            }
            
        }


        /// <summary>
        /// ��������� ���������
        /// </summary>
        [ContextMenu(nameof(Off_state))]
        public virtual void Off_state()
        {
            State_Machine.Change_State(null);

            Off_states_event.Invoke();
        }


        public virtual void Change_activity(bool _active)
        {
            Active_bool = _active;

            if (!_active)
            {
                StopAllCoroutines();

                //print("���� ��.");
            }
            else
            {
                Reset();
            }


        }

        [ContextMenu(nameof(Activation))]
        public virtual void Activation()
        {
            Active_bool = true;

            if (Start_zero_state)
                Change_state(0);

            StartCoroutine(Coroutine_Initialized_State_machine());

            Activation_event.Invoke();
        }

        public void Reset()
        {
            //if (Initialization_bool)
            //{
            Activation();
            //}

        }
        #endregion
    }
}
