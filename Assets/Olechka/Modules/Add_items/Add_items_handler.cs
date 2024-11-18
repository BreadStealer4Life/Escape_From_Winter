using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Olechka
{
    public class Add_items_handler : MonoBehaviour
    {
        #region Variables

        [Tooltip("Предметы на которые будет реагировать")]
        [SerializeField]
        Item_class[] Items_array = new Item_class[0];

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////

        #region MonoBehaviour Methods

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////

        #region Methods

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////

        #region Public Methods
        public void Add_item(string _name_item)
        {
            foreach (Item_class item in Items_array)
            {
                if (item.Name == _name_item)
                {
                    item.Add_item_event.Invoke();
                }
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

        [System.Serializable]
        class Item_class
        {
            [Tooltip("Имя (id) предмета на который будет реагировать")]
            public string Name;

            [Tooltip("Ивент когда подобран предмет")]
            public UnityEvent Add_item_event = new UnityEvent();
        }
    }
}