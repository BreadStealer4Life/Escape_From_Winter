using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

namespace Olechka
{
    public class Visible_handler : MonoBehaviour
    {
        #region Variables

        public bool Active_bool = false;

        [Foldout("Ивенты")]
        [SerializeField]
        UnityEvent On_vision_event = new UnityEvent();

        [Foldout("Ивенты")]
        [SerializeField]
        UnityEvent Off_vision_event = new UnityEvent();

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////

        #region MonoBehaviour Methods
        private void OnBecameVisible()
        {
            if (Active_bool)
            {
                On_vision_event.Invoke();
            }
        }

        private void OnBecameInvisible()
        {
            if (Active_bool )
            {
                Off_vision_event.Invoke();
            }
        }
        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////

        #region Methods

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////

        #region Public Methods
        public void Activity(bool _active)
        {
            Active_bool = _active;
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