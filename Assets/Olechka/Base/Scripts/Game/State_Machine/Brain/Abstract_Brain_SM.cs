//Отвечает за смену состояний поведения
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
        #region Переменные
        [field: Tooltip("Ссылка на основной скрипт персонажа (это может быть даже предмет!)")]
        [field: SerializeField]
        protected Character_abstract Main_character { get; private set; } = null;

        [Tooltip("Массив состояний поведения (инициализируется (запускается) всегда первым самое первое состояние)")]
        [SerializeField]
        protected State_abstract[] State_array = new State_abstract[0];

        protected StateMachine State_Machine { get; private set; } = null;

        [Tooltip("Время обновления для медленного Update")]
        [SerializeField]
        float Time_slow_update = 0.1f;

        [Tooltip("Время обновления для медленного Update_2 (второй)")]
        [SerializeField]
        float Time_slow_update_2 = 0.5f;

        Coroutine Slow_update_coroutine = null;
        Coroutine Slow_update_coroutine_2 = null;

        protected bool Active_bool = true;

        [Tooltip("Активировать при старте сцены")]
        [SerializeField]
        bool Start_bool = true;

        [Tooltip("Активировать 0-е состояние (самое начальное)")]
        [SerializeField]
        protected bool Start_zero_state = true;

        [Foldout("Ивенты")]
        [Tooltip("Ивент активации")]
        [SerializeField]
        UnityEvent Activation_event = new UnityEvent();

        [Foldout("Ивенты")]
        [Tooltip("Ивент при выключение состояний")]
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


        #region Методы


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


        #region Управляющие методы
        /// <summary>
        /// Изменить состояние поведения по имени
        /// </summary>
        /// <param name="_name">Имя состояния</param>
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
            Debug.LogError("Состояние поведения по имени  " + _name + "  не найдено!");
        }

        /// <summary>
        /// Изменить состояние поведения по id
        /// </summary>
        /// <param name="_name">Имя состояния</param>
        public void Change_state(int _id)
        {
            if(State_array.Length - 1 < _id || _id < 0)
            Debug.LogError("Состояние поведения по id  " + _id + "  не существует!");
            
            if (State_Machine.Current_State != State_array[_id])
            {
                //print(_id);
                State_Machine.Change_State(State_array[_id]);
                
            }
            
        }


        /// <summary>
        /// Выключить состояния
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

                //print("Мозг всё.");
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
