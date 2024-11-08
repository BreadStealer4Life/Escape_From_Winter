using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Olechka
{
    public class Rocket_launcher : Skill_abstract
    {
        #region Variables

        [Tooltip("Урон оружия")]
        [SerializeField]
        int Damage = 10;

        [Tooltip("Количество снарядов")]
        [SerializeField]
        int Number_Projectiles = 5;

        [Tooltip("Время между выстрелами")]
        [SerializeField]
        float Fire_rate = 0.1f;

        [Tooltip("Разброс при стрельбе")]
        [SerializeField]
        float Spread = 10f;

        [Tooltip("Точки для спавна снаряда")]
        [SerializeField]
        Transform Firepoint = null;

        [Tooltip("Префаб пули")]
        [SerializeField]
        Bullet Bullet_prefab = null;

        [field: Tooltip("Дружественный огонь")]
        [field: SerializeField]
        protected bool Friendly_fire_bool = false;

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////

        #region MonoBehaviour Methods

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////

        #region Methods
        IEnumerator Coroutine_fire_projectiles()
        {
            Start_skill();

            for(int x = 0; x < Number_Projectiles; x++)
            {
                Quaternion direction = Firepoint.rotation * Quaternion.Euler(new Vector3(Random.Range(-Spread, Spread), 0, 0));

                Bullet bulet = LeanPool.Spawn(Bullet_prefab, Firepoint.position, direction);

                bulet.Set_preparation(Damage, Owner, Friendly_fire_bool);

                yield return new WaitForSecondsRealtime(Fire_rate);
            }

            End_skill();
        }
        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////

        #region Public Methods
        public override void Activation()
        {
            StartCoroutine(Coroutine_fire_projectiles());
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