using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Olechka
{
    public class Gun_controller : MonoBehaviour
    {
        #region Variables

        [Tooltip("Поворачивается")]
        [SerializeField]
        protected bool Roration_bool = true;

        [ShowIf(nameof(Roration_bool))]
        [Tooltip("Точка вращения")]
        [SerializeField]
        Transform Point_rotation = null;

        [ShowIf(nameof(Roration_bool))]
        [Tooltip("Заблокировать поворот по оси")]
        [SerializeField]
        bool Lock_X = false, Lock_Y = false, Lock_Z = false;

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////

        #region MonoBehaviour Methods

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////

        #region Methods


        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////

        #region Public Methods
        public void Rotation_towards_target(Transform _tagert)
        {
            Rotation_towards_target(_tagert.position);
        }

        public void Rotation_towards_target(Vector3 _tagert)
        {

            if (Lock_X)
            {
                _tagert.x = Point_rotation.position.x;
            }

            if (Lock_Y)
            {
                _tagert.y = Point_rotation.position.y;
            }

            if (Lock_Z)
            {
                _tagert.z = Point_rotation.position.z;
            }

            Point_rotation.transform.LookAt(_tagert);
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