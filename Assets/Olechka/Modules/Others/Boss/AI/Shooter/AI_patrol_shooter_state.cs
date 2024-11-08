using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Olechka
{
    public class AI_patrol_shooter_state : State_AI_abstract
    {
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Variables

        [Tooltip("Точки патрулирования")]
        [SerializeField]
        Transform[] Point_way = new Transform[0];

        int Point_way_id_active = 0;

        [Tooltip("Дистанция для засчитывания того, что дошёл до точки патрулирования")]
        [SerializeField]
        float Distance_fin_way_point = 0.5f;

        [Tooltip("Сколько будет ждать прежде чем пойдёт дальше.")]
        [SerializeField]
        float Waiting_next_point = 2f;

        [Tooltip("Скрипт передвижения")]
        [SerializeField]
        Move_character Move_script = null;

        bool Waiting_bool = false;

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
            
            if(!Waiting_bool)
                Patrol();
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
        }

        public override void Exit_state()
        {
            base.Exit_state();
        }


        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////

        #region Methods
        void Patrol()
        {
            if (Vector3.Distance(Point_way[Point_way_id_active].position, transform.position) > Distance_fin_way_point)
            {
                Vector2 direction = (Point_way[Point_way_id_active].position.x - transform.position.x) > 0 ? Vector2.right : Vector2.left;

                Move_script.Input_change(direction);
            }
            else
            {
                Move_script.Input_change(Vector2.zero);
                StartCoroutine(Coroutine_waiting_next_point());
            }
        }

        void Next_point()
        {
            Point_way_id_active++;

            if (Point_way.Length <= Point_way_id_active)
                Point_way_id_active = 0;
        }

        IEnumerator Coroutine_waiting_next_point()
        {
            Waiting_bool = true;

            yield return new WaitForSecondsRealtime(Waiting_next_point);

            Next_point();

            Waiting_bool = false;

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

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////





    }
}