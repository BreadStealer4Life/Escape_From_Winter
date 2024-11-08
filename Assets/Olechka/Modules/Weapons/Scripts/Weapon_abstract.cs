using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Olechka
{
    public abstract class Weapon_abstract : MonoBehaviour
    {
        [Tooltip("��������")]
        [SerializeField]
        protected Character_abstract Owner = null;

        [field: Tooltip("������������� �����")]
        [field: SerializeField]
        protected bool Friendly_fire_bool = false;
    }
}