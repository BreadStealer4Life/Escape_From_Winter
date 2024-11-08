using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Olechka
{
    public class Trigger_handler : MonoBehaviour
    {
        [Tooltip("Тег для взаимодействия")]
        [SerializeField]
        string Tag = "Тег";

        [Tooltip("Ивент срабатывания триггера по входу")]
        [SerializeField]
        UnityEvent Trigger_enter_event = new UnityEvent();

        [Tooltip("Ивент срабатывания триггера по выходу")]
        [SerializeField]
        UnityEvent Trigger_exit_event = new UnityEvent();

        [Tooltip("Ивент передающий, кто вошёл в триггер")]
        [SerializeField]
        UnityEvent<Transform> Obj_detect = new UnityEvent<Transform>();

        private void OnTriggerEnter(Collider other)
        {
            if(other.tag == Tag)
            {
                Trigger_enter_event.Invoke();

                Obj_detect.Invoke(other.transform);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag == Tag)
            {
                Trigger_exit_event.Invoke();
            }
        }
    }
}