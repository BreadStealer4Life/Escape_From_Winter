using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace Olechka
{
    [AddComponentMenu("Olechka scripts / Game / Base / Damage zone")]
    [DisallowMultipleComponent]
    public class Damage_zone : MonoBehaviour
    {
        [Tooltip("��������� ����")]
        [SerializeField]
        int Damage = 1;

        [Tooltip("�� ����� �������� ���� ���������.")]
        [SerializeField]
        bool No_Damage_yourself_bool = false;

        [ShowIf(nameof(No_Damage_yourself_bool))]
        [Tooltip("�������� ���������")]
        [SerializeField]
        Health My_health = null;

        private void OnTriggerEnter(Collider other)
        {
            
            if(other.TryGetComponent (out Health _health))
            {
                if (My_health != _health || !No_Damage_yourself_bool)
                {
                    _health.Add_damage(Damage, null);
                }
                
            }
        }
    }
}