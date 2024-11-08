using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Olechka
{
    public class Shield : Skill_abstract
    {
        #region Variables

        [Tooltip("Визуальная часть щита")]
        [SerializeField]
        GameObject Visual_obj = null;

        [Tooltip("Время работы")]
        [SerializeField]
        float Time = 5f;

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////

        #region MonoBehaviour Methods

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////

        #region Methods
        IEnumerator Coroutine_active()
        {
            Start_skill();

            Visual_obj.SetActive(true);

            yield return new WaitForSecondsRealtime(Time);

            Visual_obj.SetActive(false);

            End_skill();
        }
        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////

        #region Public Methods
        public override void Activation()
        {
            StartCoroutine(Coroutine_active());
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