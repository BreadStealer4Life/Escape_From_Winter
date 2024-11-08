using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Olechka
{
    public class State_AI_abstract : State_abstract
    {
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Variables
        protected Brain_SM_AI_abstract Brain_SM_script;

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////

        #region MonoBehaviour Methods

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////

        #region Core Methods

        public void Start_preparation_SM(Character_abstract _character, StateMachine _stateMachine, Brain_SM_AI_abstract _brain)
        {
            Character_script = _character;
            State_Machine_script = _stateMachine;
            Brain_SM_script = _brain;
        }

        protected override void Preparation()
        {
            base.Preparation();
        }

        protected override void Initialized_stats()
        {
            throw new System.NotImplementedException();
        }

        public override void Logic_Update()
        {
                base.Logic_Update();
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