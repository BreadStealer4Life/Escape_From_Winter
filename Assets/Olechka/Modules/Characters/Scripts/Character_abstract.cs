using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Olechka
{
    public enum Fraction_enum
    {
        Player,
        Enemy
    }

    public class Character_abstract : MonoBehaviour
    {
        [field:Tooltip("������ �� ��������")]
        [field:SerializeField]
        public Health Health_script { get; private set; } = null;

        [field: Tooltip("� ����� ������� ���������")]
        [field: SerializeField]
        public Fraction_enum Fraction { get; private set; } = Fraction_enum.Player;

        [Foldout("������")]
        [Tooltip("������� ����������")]
        [SerializeField]
        UnityEvent<Character_abstract> Kill_enemy_event = new UnityEvent<Character_abstract>();

        [Foldout("������")]
        [Tooltip("������ ���� ����������")]
        [SerializeField]
        UnityEvent<Character_abstract> Damage_enemy_event = new UnityEvent<Character_abstract>();


        public void Kill_enemy (Character_abstract _enemy)
        {
            Kill_enemy_event.Invoke(_enemy);
        }

        public void Damage_enemy(Character_abstract _enemy)
        {
            Damage_enemy_event.Invoke(_enemy);
        }
    }
}