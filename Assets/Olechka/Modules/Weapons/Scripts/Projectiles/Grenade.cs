using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using Lean.Pool;

namespace Olechka
{
    public class Grenade : Projectiles_abstract
    {

        [Tooltip("Начать работу гранаты при спавне")]
        [SerializeField]
        bool Start_activation_bool = false;

        [Tooltip("Радиус взрыва")]
        [SerializeField]
        float Radius_explosion = 5f;

        [Tooltip("Таймер до взрыва")]
        [SerializeField]
        float Timer = 3f;

        [Tooltip("Слой для взаимодействия")]
        [SerializeField]
        LayerMask Mask = 1;

        [Tooltip("Эффект взрыва")]
        [SerializeField]
        ParticleSystem PS_explosion = null;

        Coroutine Timer_coroutine = null;

        [field: Tooltip("Физика")]
        [field: SerializeField]
        public Rigidbody Body {  get; private set; } = null;

        [Tooltip("Отображать гизмос")]
        [SerializeField]
        bool Gizmos_mode = false;

        [ShowIf(nameof(Gizmos_mode))]
        [Tooltip("Цвет гизмоса для триггера срабатывания")]
        [SerializeField]
        Color Gizmos_trigger_color = new Color(1f, 0f, 0f, 0.3f);

        [ShowIf(nameof(Gizmos_mode))]
        [Tooltip("Цвет гизмоса для радиуса взрыва")]
        [SerializeField]
        Color Gizmos_explosion_color = new Color(1f, 0f, 0f, 0.3f);

        private void Start()
        {
            if (Start_activation_bool)
                Activation();
        }

        protected override void Detect_object(Transform _obj)
        {
            if (_obj.TryGetComponent(out I_damage _health))
            {
                Explosion();
            }
        }

        public void Activation()
        {
            if (Timer_coroutine != null)
                StopCoroutine(Timer_coroutine);

            Timer_coroutine = StartCoroutine(Coroutine_Timer());
        }


        IEnumerator Coroutine_Timer()
        {
            yield return new WaitForSecondsRealtime(Timer);
            Explosion();
        }

        void Explosion()
        {
            if (Timer_coroutine != null)
                StopCoroutine(Timer_coroutine);

            Collider[] obj_array = Physics.OverlapSphere(transform.position, Radius_explosion, Mask, QueryTriggerInteraction.Ignore);

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

        private void OnDrawGizmos()
        {
            if (Gizmos_mode)
            {
                Gizmos.color = Gizmos_explosion_color;

                Gizmos.DrawSphere(transform.position, Radius_explosion);
            }
        }
    }
}