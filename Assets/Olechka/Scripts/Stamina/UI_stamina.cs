using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Olechka
{
    public class UI_stamina : Singleton<UI_stamina>
    {
        #region Variables

        [Tooltip("Визуальная часть")]
        [SerializeField]
        GameObject Visual = null;

        [Tooltip("Значение (полоска)")]
        [SerializeField]
        Image Value = null;

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////

        #region MonoBehaviour Methods

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////

        #region Methods

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////

        #region Public Methods
        public void Activity(bool _activity)
        {
            Visual.SetActive(_activity);
        }

        public void Change_value(float _value)
        {
            Value.fillAmount = _value;
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