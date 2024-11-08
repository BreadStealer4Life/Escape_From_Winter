using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace Olechka
{
    public class Detect_handler : MonoBehaviour
    {

        [Tooltip("Радиус срабатывания")]
        [SerializeField]
        float Radius = 1f;

        [Tooltip("Смещение")]
        [SerializeField]
        Vector3 Offset = Vector3.zero;

        [Tooltip("Слой для взаимодействия")]
        [SerializeField]
        LayerMask Mask = 1;

        [Tooltip("Как часто будет срабатывать")]
        [SerializeField]
        float Time_update = 0.4f;

        [Tooltip("Ивент когда найдено")]
        [SerializeField]
        UnityEvent Detect_enter_event = new UnityEvent();

        [Tooltip("Ивент когда найдено и передаёт данные чего нашёл")]
        [SerializeField]
        UnityEvent<GameObject[]> Detect_target_enter_event = new UnityEvent<GameObject[]>();

        [Tooltip("Режим отображения гизмоса")]
        [SerializeField]
        bool Gizmos_mode = false;

        [ShowIf(nameof(Gizmos_mode))]
        [Tooltip("Цвет гизмоса")]
        [SerializeField]
        Color Color_gizmos = new Color(0f, 1f, 0f, 0.3f);

        private void OnEnable()
        {
            StopAllCoroutines();

            StartCoroutine(Coroutine_update());
        }

        IEnumerator Coroutine_update()
        {
            while (true)
            {
                yield return new WaitForSecondsRealtime(Time_update);
                Detect();
            }
        }

        void Detect()
        {
            Collider[] derect_result_array = Physics.OverlapSphere(transform.position + Offset, Radius, Mask, QueryTriggerInteraction.Ignore);

            if (derect_result_array.Length != 0)
                Detect_enter_event.Invoke();

            GameObject[] obj_array = new GameObject[derect_result_array.Length];

            for (int x = 0; x < derect_result_array.Length; x++)
            {
                obj_array[x] = derect_result_array[x].gameObject;
            }

            Detect_target_enter_event.Invoke(obj_array);
        }

        private void OnDrawGizmosSelected()
        {
            if (Gizmos_mode)
            {
                Gizmos.color = Color_gizmos;
                Gizmos.DrawSphere(transform.position + Offset, Radius);
            }
        }
    }
}
