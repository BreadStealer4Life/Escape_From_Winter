using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Olechka
{
    public class Pick_up_item : MonoBehaviour
    {
        #region Variables

        [Tooltip("Название (id) предмета")]
        [SerializeField]
        string Name_item = "Name item";

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////

        #region MonoBehaviour Methods
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Add_items_handler _add_items_handler))
            {
                _add_items_handler.Add_item(Name_item);

                gameObject.SetActive(false);
            }
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