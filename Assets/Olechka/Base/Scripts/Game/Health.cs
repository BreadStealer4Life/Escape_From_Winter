using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using NaughtyAttributes;

namespace Olechka
{
    [AddComponentMenu("Olechka scripts / Game / Base / Health")]
    [DisallowMultipleComponent]
    public class Health : MonoBehaviour
    {
        [field: Tooltip("Количество здоровья")]
        [field: SerializeField]
        public int Health_active { get; private set; } = 1;

        public int Health_max { get; private set; } = 0;

        [Tooltip("Время неуязвимости при получение урона")]
        [SerializeField]
        float Invulnerability_time = 0.1f;

        bool Invulnerability_bool = false;

        [Tooltip("Время до уничтожения объекта")]
        [SerializeField]
        float Time_destroy = 0f;

        [Tooltip("Партиклы при смерти")]
        [SerializeField]
        GameObject PS_dead = null;

        [Tooltip("Точка спавна партиклов смерти")]
        [SerializeField]
        Transform PS_point = null;

        public Character_abstract Owner = null;

        [Foldout("Ивенты")]
        [Tooltip("Ивент изменения здоровья")]
        [SerializeField]
        UnityEvent<int> Change_health_event = new UnityEvent<int>();

        [Foldout("Ивенты")]
        [Tooltip("Ивент изменения здоровья в значение от 0 до 1")]
        [SerializeField]
        UnityEvent<float> Value_health_event = new UnityEvent<float>();

        [Foldout("Ивенты")]
        [Tooltip("Ивент получения урона")]
        [SerializeField]
        UnityEvent Harm_event = new UnityEvent();

        [Foldout("Ивенты")]
        [Tooltip("Ивент показывающий кто атакует")]
        [SerializeField]
        UnityEvent<Character_abstract> Killer_detect_event = new UnityEvent<Character_abstract>();

        [Foldout("Ивенты")]
        [Tooltip("Ивент смерти")]
        [SerializeField]
        UnityEvent Dead_event = new UnityEvent();

        bool Alive_bool = true;


        private void Start()
        {
            Health_max = Health_active;

            Change_health(0);
        }

        void Dead()
        {
            Alive_bool = false;

            Dead_event.Invoke();

            Vector3 point_ps = PS_point != null ? PS_point.position : transform.position;

            if(PS_dead != null) 
            LeanPool.Spawn(PS_dead, point_ps, Quaternion.identity);

            Invoke(nameof(Destoroy_obj), Time_destroy);

            Alive_bool = false;
        }

        void Destoroy_obj()
        {
            LeanPool.Despawn(this);
        }

        IEnumerator Coroutine_invulnerability()
        {
            Invulnerability_bool = true;

            yield return new WaitForSecondsRealtime(Invulnerability_time);

            Invulnerability_bool = false;
        }

        public void Add_damage(int _damage, Character_abstract _killer)
        {
            if (!Invulnerability_bool)
                Change_health(-_damage);

            if (Health_active <= 0)
            {
                if (_killer != null && Owner != null)
                {
                    _killer.Kill_enemy(Owner);
                }

                Dead();
            }
            else 
            {
                Harm_event.Invoke();

                StartCoroutine(Coroutine_invulnerability());
                
                if(_killer != null)
                _killer.Damage_enemy(Owner);

                if (_killer != null)
                {
                    Killer_detect_event.Invoke(_killer);
                }
            }
        }

        public void Add_heal(int _heal)
        {
            Change_health(_heal);
        }

        void Change_health(int _value)
        {
            
            //print("s  " + Healtp_active);

            Health_active += _value;

            if (Health_active < 0)
                Health_active = 0;

            if (Health_active > Health_max)
                Health_active = Health_max;

            Change_health_event.Invoke(Health_active);

            Value_health_event.Invoke((float)Health_active / (float)Health_max);
        }

        public bool Get_alive()
        {
            return Alive_bool;
        }

        public bool Get_maximum_health_bool
        {
            get
            {
                bool result = false;

                if (Health_active == Health_max)
                    result = true;

                return result;
            }
        }

        public Health Main_health => throw new System.NotImplementedException();
    }
}
