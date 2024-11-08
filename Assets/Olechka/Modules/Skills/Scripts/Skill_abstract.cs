using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

namespace Olechka
{
    public abstract class Skill_abstract : MonoBehaviour
    {
        [Tooltip("��������")]
        [SerializeField]
        protected Character_abstract Owner = null;

        [Foldout("������")]
        [Tooltip("����� ��������� ������")]
        [SerializeField]
        UnityEvent Start_event = new UnityEvent();

        [Foldout("������")]
        [Tooltip("����� ��������� ������")]
        [SerializeField]
        UnityEvent End_event = new UnityEvent();

        public abstract void Activation();

        protected virtual void Start_skill()
        {
            Start_event.Invoke();
        }

        protected virtual void End_skill()
        {
            End_event.Invoke();
        }
    }
}