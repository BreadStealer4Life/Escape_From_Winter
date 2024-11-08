using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

namespace Olechka
{
    public class AI_brain_shooter : Brain_SM_AI_abstract
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
        public void Detect_player(GameObject[] _target)
        {
            for(int x = 0; x < _target.Length; x++)
            {
                if (_target[x].GetComponent<Character_abstract>()) 
                {
                    Target = _target[x];

                    Change_state("Attack");
                }
                else
                {
                    Change_state("Patrol");
                }
            }
        }

        public void Detect_player(Character_abstract _target)
        {
            Target = _target.gameObject;

            Change_state("Attack");
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