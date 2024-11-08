using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Olechka
{
    public class Rocket_launcher : Skill_abstract
    {
        #region Variables

        [Tooltip("���� ������")]
        [SerializeField]
        int Damage = 10;

        [Tooltip("���������� ��������")]
        [SerializeField]
        int Number_Projectiles = 5;

        [Tooltip("����� ����� ����������")]
        [SerializeField]
        float Fire_rate = 0.1f;

        [Tooltip("������� ��� ��������")]
        [SerializeField]
        float Spread = 10f;

        [Tooltip("����� ��� ������ �������")]
        [SerializeField]
        Transform Firepoint = null;

        [Tooltip("������ ����")]
        [SerializeField]
        Bullet Bullet_prefab = null;

        [field: Tooltip("������������� �����")]
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