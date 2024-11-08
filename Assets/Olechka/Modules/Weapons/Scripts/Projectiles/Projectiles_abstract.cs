using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Olechka
{
    public abstract class Projectiles_abstract : MonoBehaviour
    {
        protected int Damage = 0;

        protected Character_abstract Owner = null;

        protected bool Friendly_fire_bool = false;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.isTrigger)
                Detect_object(other.transform);
        }

        protected void Destroy_bullet()
        {
            LeanPool.Despawn(gameObject);
        }

        public void Set_preparation(int _damage, Character_abstract _owner, bool _friendly_fire_bool)
        {
            Damage = _damage;
            Owner = _owner;
            Friendly_fire_bool |= _friendly_fire_bool;
        }

        protected abstract void Detect_object(Transform _obj);
    }
}