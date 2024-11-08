using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;

namespace Olechka
{
    public class Throwing_grenade : Skill_abstract
    {

        [Tooltip("Объект который будем кидать")]
        [SerializeField]
        Grenade Object_prefab = null;

        [Tooltip("Сила броска")]
        [SerializeField]
        float Force = 15f;

        [Tooltip("Урон")]
        [SerializeField]
        int Damage = 5;

        [field: Tooltip("Дружественный огонь")]
        [field: SerializeField]
        protected bool Friendly_fire_bool = false;

        [Tooltip("Точка спавна")]
        [SerializeField]
        Transform Point_spawn = null;

        [Tooltip("Объект задающий направление")]
        [SerializeField]
        Transform Point_forward = null;

        [Tooltip("Корректировка направление броска")]
        [SerializeField]
        Vector3 Offset_direction = new Vector3 (0f, 1f, 0f);

        public override void Activation()
        {
            Start_skill();

            Vector3 direction = Point_forward.forward;

            direction += Point_forward.forward * Offset_direction.z;

            direction += Point_forward.up * Offset_direction.y;

            direction += Point_forward.right * Offset_direction.x;

            Grenade grenade = LeanPool.Spawn(Object_prefab, Point_spawn.position, Quaternion.identity);

            grenade.Set_preparation(Damage, Owner, Friendly_fire_bool);

            grenade.Body.velocity = Vector3.zero;
            grenade.Body.AddForce(direction * Force, ForceMode.Impulse);

            End_skill();
        }
    }
}