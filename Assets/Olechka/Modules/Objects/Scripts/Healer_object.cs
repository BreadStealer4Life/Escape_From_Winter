using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Olechka
{
    public class Healer_object : Objects_pick_up_abstract
    {
        #region Variables

        [Tooltip("Сколько лечит")]
        [SerializeField]
        int Heal = 1;

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////

        #region MonoBehaviour Methods

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////

        #region Methods
        protected override void Detect_character(GameObject _character)
        {
            if (_character.TryGetComponent(out Health _health))
            {
                if (!_health.Get_maximum_health_bool)
                {
                    _health.Add_heal(Heal);

                    Destroy_this();
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

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////

    }
}