using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;
using Unity.VisualScripting;

namespace Olechka
{
    public class Bullet : Projectiles_abstract
    {

        [Tooltip("Время жизни")]
        [SerializeField]
        float Time_life = 10;

        [Tooltip("Скорость снаряда")]
        [SerializeField]
        float Speed = 50f;

        [Tooltip("Маска взаимодействия")]
        [SerializeField]
        LayerMask Mask = 1;

        [Tooltip("След")]
        [SerializeField]
        TrailRenderer Trail = null;

        Coroutine Life_time_coroutine = null;

        [Tooltip("Дистанция пробрасывания луча")]
        [SerializeField]
        float Distance_ray_detect = 0.5f;

        [Tooltip("Отображать гизмос")]
        [SerializeField]
        bool Gizmos_mode_bool = false;

        private void OnEnable()
        {
            if (Trail)
            {
                Trail.Clear();
            }
                
        }

        private void Start()
        {
            if (Life_time_coroutine != null)
            {
                StopCoroutine(Life_time_coroutine);
                Life_time_coroutine = null;
            }
                
            Life_time_coroutine = StartCoroutine(Coroutine_life_time());
        }
        
        private void Update()
        {
            transform.position += transform.forward * Speed * Time.deltaTime;
            Detect_ray();
        }

        void Detect_ray()
        {
            RaycastHit hit;
            
            if(Physics.Raycast(transform.position, transform.forward, out hit, Distance_ray_detect, Mask, QueryTriggerInteraction.Ignore))
            {
                Detect_object(hit.transform);
            }
        }

        protected override void Detect_object(Transform _obj)
        {
            if (_obj.TryGetComponent(out I_damage health))
            {

                if(Friendly_fire_bool || !Owner || (!health.Main_health.Owner || Owner.Fraction != health.Main_health.Owner.Fraction))
                {

                    health.Add_Damage(Damage, Owner);
                    Destroy_bullet();
                }   
            }
            else
            {
                Destroy_bullet();
            }
            
        }

        IEnumerator Coroutine_life_time()
        {
            yield return new WaitForSecondsRealtime(Time_life);

            Destroy_bullet();
        }

        private void OnDrawGizmosSelected()
        {
            if (Gizmos_mode_bool)
            {
                Gizmos.color = Color.yellow;

                Gizmos.DrawRay(transform.position, transform.forward * Distance_ray_detect);
            }
        }


    }
}