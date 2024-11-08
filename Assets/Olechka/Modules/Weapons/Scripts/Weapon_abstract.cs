using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Olechka
{
    public abstract class Weapon_abstract : MonoBehaviour
    {
        [Tooltip("Владелец")]
        [SerializeField]
        protected Character_abstract Owner = null;

        [field: Tooltip("Дружественный огонь")]
        [field: SerializeField]
        protected bool Friendly_fire_bool = false;
    }
}