using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Olechka
{
    public class AI_attack_state : State_AI_abstract
    {
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Variables

        [Tooltip("Скрипт передвижения")]
        [SerializeField]
        Move_character Move_script = null;

        [Tooltip("Дистанция атаки")]
        [SerializeField]
        float Distance_attack = 20f;

        [Tooltip("Смещение")]
        [SerializeField]
        Vector3 Offset = Vector3.zero;

        [Tooltip("Ивент атаки")]
        [SerializeField]
        UnityEvent Attack_event = new UnityEvent();

        [Tooltip("Ивент остановки атаки")]
        [SerializeField]
        UnityEvent Stop_Attack_event = new UnityEvent();

        bool Attack_bool = false;


        [Space(20)]
        [Tooltip("Режим отображения гизмоса")]
        [SerializeField]
        bool Gizmos_mode = false;

        [ShowIf(nameof(Gizmos_mode))]
        [Tooltip("Цвет гизмоса")]
        [SerializeField]
        Color Color_gizmos = new Color(1f, 0f, 0f, 0.3f);

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////

        #region MonoBehaviour Methods

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////

        #region Core Methods

        protected override void Preparation()
        {
            base.Preparation();
        }

        protected override void Initialized_stats()
        {

        }

        public override void Logic_Update()
        {
            base.Logic_Update();

            if (Brain_SM_script.Target == null)
                Brain_SM_script.Change_state(0);
            else
                Move();
        }

        public override void Physics_Update()
        {
            base.Physics_Update();
        }

        public override void Slow_Update()
        {
            base.Slow_Update();
        }

        public override void Slow_Update_2()
        {
            base.Slow_Update_2();
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////

        #region State Method
        public override void Enter_state()
        {
            base.Enter_state();

            Move_script.Input_change(Vector2.zero);
        }

        public override void Exit_state()
        {
            base.Exit_state();
        }


        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////

        #region Methods
        void Move()
        {

            Vector2 direction = (Brain_SM_script.Target.transform.position.x - transform.position.x) > 0 ? Vector2.right : Vector2.left;

            if (Vector3.Distance(Brain_SM_script.Target.transform.position, transform.position + Offset) < Distance_attack)
            {
                

                //Move_script.Rotation_direction(direction);

                if (!Attack_bool)
                {
                    Attack_bool = true;
                    Attack_event.Invoke();
                }
                
            }
            else
            {
                if (Attack_bool)
                {
                    Attack_bool = false;
                    Stop_Attack_event.Invoke();
                }
                else
                {
                    Move_script.Input_change(direction);
                }
            }
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////

        #region Public Methods

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////

        #region Additionally

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////

        #region Gizmos
        private void OnDrawGizmosSelected()
        {
            if (Gizmos_mode)
            {
                Gizmos.color = Color_gizmos;
                Gizmos.DrawSphere(transform.position + Offset, Distance_attack);
            }
        }
        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////





    }
}