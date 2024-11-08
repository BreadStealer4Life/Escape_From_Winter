using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Olechka
{
    public class Rocket_projectile : Bullet
    {
        #region Variables

        [Tooltip("Таймер до взрыва")]
        [SerializeField]
        float Timer = 3f;

        [Tooltip("Радиус взрыва")]
        [SerializeField]
        float Radius_explosion = 5f;

        [Tooltip("Эффект взрыва")]
        [SerializeField]
        ParticleSystem PS_explosion = null;

        [Tooltip("Слой для взаимодействия")]
        [SerializeField]
        LayerMask Explosion_mask = 1;

        Coroutine Timer_coroutine = null;

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////

        #region MonoBehaviour Methods
        void OnEnable()
        {
            if (Timer_coroutine != null)
                StopCoroutine(Timer_coroutine);

            Timer_coroutine = StartCoroutine(Coroutine_Timer());
        }
        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////

        #region Methods

        IEnumerator Coroutine_Timer()
        {
            yield return new WaitForSecondsRealtime(Timer);
            Explosion();
        }

        protected override void Detect_object(Transform _obj)
        {
            Explosion();
        }

        void Explosion()
        {
            if (Timer_coroutine != null)
                StopCoroutine(Timer_coroutine);

            Collider[] obj_array = Physics.OverlapSphere(transform.position, Radius_explosion, Explosion_mask, QueryTriggerInteraction.Ignore);

            for (int x = 0; x < obj_array.Length; x++)
            {
                if (obj_array[x].TryGetComponent(out I_damage _health))
                {
                    _health.Add_Damage(Damage, Owner);
                }
            }

            var ps_main = LeanPool.Spawn(PS_explosion, transform.position, Quaternion.identity).main;

            ps_main.startSize = Radius_explosion;

            LeanPool.Despawn(gameObject);
        }

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