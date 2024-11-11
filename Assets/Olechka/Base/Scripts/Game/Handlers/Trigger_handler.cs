using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

namespace Olechka
{
    public class Trigger_handler : MonoBehaviour
    {
        [Tooltip("��� ��� ��������������")]
        [SerializeField]
        string Tag = "���";

        [Foldout("������")]
        [Tooltip("����� ������������ �������� �� �����")]
        [SerializeField]
        UnityEvent Trigger_enter_event = new UnityEvent();

        [Foldout("������")]
        [Tooltip("����� ������������ �������� �� ������")]
        [SerializeField]
        UnityEvent Trigger_exit_event = new UnityEvent();

        [Foldout("������")]
        [Tooltip("����� ����������, ��� ����� � �������")]
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