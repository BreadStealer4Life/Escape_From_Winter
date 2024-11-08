//������� ������� �������� ��������� ���� � ������� ��������� ������� ��������
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

namespace Olechka
{
    [AddComponentMenu("Olechka scripts / Game / Battle / HitBox")]
    [DisallowMultipleComponent]
    public class HitBox: MonoBehaviour, I_damage
    {

        [Tooltip("�������� ������ ��������")]
        [SerializeField]
        Health Main_health = null;

        [Tooltip("�������� ����")]
        [SerializeField]
        float Multiply_damage = 1.0f;

        public Character_abstract Character_script { get; private set; }  = null;


        [Foldout("������")]
        [Tooltip("���� ���������! ��� ����� ������ ? (����� ��� �������������� ��������, �������� ���� ���� ������ �������� �� ������)")]
        [SerializeField]
        UnityEvent Hit_event = new UnityEvent();

        Health I_damage.Main_health => Main_health;

        private void Start()
        {
            if(Main_health)
            Character_script = Main_health.Owner;
        }

        public void Add_Main_health(Health _health_script)//������� ��� ��������������� ������� ������� ������ �� ����� ����� ����������� (�������� Ragdoll_script)
        {
            if(!Main_health)
            Main_health = _health_script;
        }

        void I_damage.Add_damage(int _damage, Character_abstract _killer)
        {
            if (Main_health)
            {
                int damage = (int)math.ceil ((float)_damage * Multiply_damage);

                Main_health.Add_damage(damage, _killer);
            }
                

            Hit_event.Invoke();
        }
    }
}