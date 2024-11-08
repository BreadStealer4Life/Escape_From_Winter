using Lean.Pool;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.Rendering.DebugUI;

namespace Olechka
{
    public abstract class Gun_abstract : Weapon_abstract
    {
        [Min(0)]
        [Tooltip("Урон оружия")]
        [SerializeField]
        int Damage = 1;

        [Tooltip("Нету патронов")]
        [SerializeField]
        bool No_ammo = false;

        [Min(0)]
        [HideIf(nameof(No_ammo))]
        [Tooltip("Количество")]
        [SerializeField]
        int Ammo = 6;

        int Max_ammo = 0;

        [Min(0)]
        [Tooltip("Время между выстрелами")]
        [SerializeField]
        float Fire_rate = 0.1f;

        bool Fire_rate_bool = false;

        [Min(1)]
        [Tooltip("Размер очереди (сколько пуль выстреливает за одну активацию)")]
        [SerializeField]
        int Gun_burst = 1;

        [Min(0)]
        [Tooltip("Время между выстрелами в очереди")]
        [SerializeField]
        float Gun_burst_Fire_rate = 0.1f;

        [Tooltip("Автоматическая стрельба")]
        [SerializeField]
        bool Automatic_shooting_bool = false;

        bool Pressed_activity_bool = false;

        [Tooltip("Время перезарядки")]
        [SerializeField]
        float Reload_time = 4;

        bool Reload_bool = false;

        [Tooltip("Точки для спавна снаряда")]
        [SerializeField]
        Transform Firepoint = null;

        [Tooltip("Префаб пули")]
        [SerializeField]
        Bullet Bullet_prefab = null;

        [Tooltip("Ивент говорящий, что стреляем")]
        [SerializeField]
        UnityEvent Attack_event = new UnityEvent();

        private void Start()
        {
            Max_ammo = Ammo;
        }

        private void Update()
        {
            if (Pressed_activity_bool)
                Activation();
        }

        IEnumerator Coroutine_fire_rate_time()
        {
            Fire_rate_bool = true;

            for(int x = 0; x < Gun_burst; x++)
            {
                Bullet bulet = LeanPool.Spawn(Bullet_prefab, Firepoint.position, Firepoint.rotation);

                bulet.Set_preparation(Damage, Owner, Friendly_fire_bool);
                
                yield return new WaitForSecondsRealtime(Gun_burst_Fire_rate);
            }

            yield return new WaitForSecondsRealtime(Fire_rate);

            Fire_rate_bool = false;
        }



        public void Activation()
        {
            if (Ammo > 0 && !Fire_rate_bool)
            {
                Attack_event.Invoke();

                StartCoroutine(Coroutine_fire_rate_time());

                if (!No_ammo)
                    Ammo--;
            }
        }

        public void Activation (bool _active)
        {
            if(Automatic_shooting_bool)
                Pressed_activity_bool = _active;
            else if(_active)
                Activation();
        }

        public void Add_ammo()
        {
                Add_ammo(1);
        }

        public void Add_ammo(int _value)
        {
            if (Ammo < Max_ammo)
            {
                Ammo += _value;

                if (Ammo > Max_ammo)
                    Ammo = Max_ammo;
            }
                
        }

        public void Start_reload()
        {
            if (!Reload_bool)
            {
                Reload_bool = true;
                Fin_reload();
            }
            
        }

        public void Fin_reload()
        {
            Reload_bool = false;
            Ammo = Max_ammo;
        }
    }
}