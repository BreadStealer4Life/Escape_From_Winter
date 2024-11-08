using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using Unity.VisualScripting;

namespace Olechka
{
    public class Dash : Skill_abstract
    {

        [Tooltip("Дистанция деша")]
        [SerializeField]
        float Distance = 10f;

        [Tooltip("Скорность деша")]
        [SerializeField]
        float Speed = 20f;

        [Tooltip("Толщина передвигаемого объекта")]
        [SerializeField]
        float Thickness_target = 1f;

        [Tooltip("Маска взаимодействия")]
        [SerializeField]
        LayerMask Mask = 1;

        [Tooltip("Цель которую передвигаем")]
        [SerializeField]
        Transform Target_transform = null;

        [Tooltip("Объект указывающий направление")]
        [SerializeField]
        Transform Point_forward = null;

        [Tooltip("Заблокировать ось движения")]
        [SerializeField]
        bool Lock_X = false, Lock_Y = false, Lock_Z = false;

        Coroutine Dash_coroutine = null;

        [Tooltip("Видеть гизмос")]
        [SerializeField]
        bool Gizmos_mode = false;

        [ShowIf(nameof(Gizmos_mode))]
        [Tooltip("Цвет гизмоса")]
        [SerializeField]
        Color Color_gizmos = Color.red;

        //bool Active_bool = false;

        public override void Activation()
        {
            if(Dash_coroutine != null)
                StopCoroutine(Dash_coroutine);

            StartCoroutine(Coroutine_Dash());
        }

        Vector3 Get_direction
        {
            get
            {
                Vector3 direction = Point_forward.forward != null ? Point_forward.forward : Target_transform.forward;

                if (Lock_X)
                    direction.x = 0f;

                if (Lock_Y)
                    direction.y = 0f;

                if (Lock_Z)
                    direction.z = 0f;

                return direction;
            }
        }

        Vector3 Get_fin_position
        {
            get
            {
                Vector3 retult = Target_transform.position + Get_direction * Distance;

                RaycastHit hit;

                Ray ray = new Ray(Target_transform.position, Get_direction);

                if (Physics.Raycast(ray, out hit, Distance, Mask, QueryTriggerInteraction.Ignore))
                {
                    Vector3 direction = Target_transform.position - hit.point;
                    direction.Normalize();

                    retult = hit.point + direction * Thickness_target;
                }


                return retult;
            }
        }

        IEnumerator Coroutine_Dash()
        {
            //Active_bool = true;
            
            Start_skill();

            Vector3 fin_position = Get_fin_position;

            float time_end = Distance / Speed;

            while (true)
            {
                yield return new WaitForFixedUpdate();

                Target_transform.position = Vector3.MoveTowards(Target_transform.position, fin_position, Speed * Time.deltaTime);

                time_end -= Time.deltaTime;
                
                if (Target_transform.position == fin_position || time_end <= 0)
                {
                    break;
                }
            }

            End_skill();

            Dash_coroutine = null;
            //Active_bool = false;
        }

        private void OnDrawGizmosSelected()
        {
            if (Gizmos_mode)
            {
                Vector3 fin_position = Get_fin_position;

                Gizmos.color = Color_gizmos;

                Gizmos.DrawLine(Target_transform.position, fin_position);

                Gizmos.DrawSphere(fin_position, 0.4f);
            }
        }
    }
}