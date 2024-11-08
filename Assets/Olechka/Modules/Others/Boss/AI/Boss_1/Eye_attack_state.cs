using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Olechka
{
    public class Eye_attack_state : State_AI_abstract
    {
        ///////////////////////////////////////////////////////////////////////////////////////////////////
        #region Variables

        [SerializeField]
        Gun_abstract[] Weapon_array = new Gun_abstract[0];

        [SerializeField]
        Gun_controller[] Gun_controller_array = new Gun_controller[0];

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
            //throw new System.NotImplementedException();
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
            Brain_SM_script.Anim.CrossFade("Attack_1", 0, 0, 0);
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
        public void Attack()
        {

            foreach (Gun_controller gun in Gun_controller_array)
            {
                gun.Rotation_towards_target(Brain_SM_script.Target.transform.position);
            }

            foreach (Gun_abstract gun in Weapon_array)
            {
                gun.Activation();
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





    }
}