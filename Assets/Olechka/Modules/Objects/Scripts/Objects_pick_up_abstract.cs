using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Olechka
{
    public abstract class Objects_pick_up_abstract : MonoBehaviour
    {
        #region Variables

        [Tooltip("Основной объект")]
        [SerializeField]
        GameObject Main_obj = null;

        [Tooltip("Эффект после подбора")]
        [SerializeField]
        ParticleSystem Pick_up_PS = null;

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////

        #region MonoBehaviour Methods
        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.GetComponent<Input_player>())
            {
                Detect_character(other.gameObject);
            }
        }
        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////

        #region Methods
        protected abstract void Detect_character(GameObject _character);

        protected virtual void Destroy_this()
        {
            if(Pick_up_PS)
            LeanPool.Spawn(Pick_up_PS);

            LeanPool.Despawn(Main_obj);
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