using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Olechka
{
    public class Brain_SM_Player_abstract : Abstract_Brain_SM
    {
        #region Variables

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////

        #region MonoBehaviour Methods

        protected override void Start()
        {
            base.Start();
        }

        protected override void Update()
        {
            base.Update();
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////

        #region Core Methods

        protected override IEnumerator Coroutine_Initialized_State_machine()
        {
            if (State_array.Length > 0)
            {
                for (int x = 0; x < State_array.Length; x++)
                {
                    State_Player_abstract state = (State_Player_abstract)State_array[x];
                    state.Start_preparation_SM(Main_character, State_Machine, this);
                }

                yield return new WaitForSeconds(1.1f);

                if (Start_zero_state)
                    Change_state(0);
            }
            else
            {
                if (Main_character != null)
                    Debug.LogError("Не заданы состояния поведения! " + "(примерно объект называется  " + Main_character.gameObject.name + "  )");
                else
                    Debug.LogError("Не указан скрипт самого персонажа!");
            }

            yield return base.Coroutine_Initialized_State_machine();
        }

        protected override void Slow_update()
        {
            base.Slow_update();
        }

        protected override void Slow_update_2()
        {
            base.Slow_update_2();
        }


        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////

        #region Methods

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