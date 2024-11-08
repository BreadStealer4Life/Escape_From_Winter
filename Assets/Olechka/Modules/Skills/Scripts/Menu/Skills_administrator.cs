using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Olechka
{
    public class Skills_administrator : MonoBehaviour, I_save_and_load
    {
        [Tooltip("Массив скиллов")]
        [SerializeField]
        Skill_class[] Skills_array = new Skill_class[0];

        [Tooltip("Управление игрока")]
        [SerializeField]
        Input_player Input_player_script = null;

        [Tooltip("Будет ли картинка заряда оставаться полной когда достигает максимального количества зарадов")]
        [SerializeField]
        public bool Max_change_image_stop_bool = false;

        void Awake()
        {
            

            if (Game_administrator.Singleton_Instance)
            {
                Game_administrator.Singleton_Instance.Start_game_event.AddListener(Load);

                Game_administrator.Singleton_Instance.End_game_event.AddListener(Save);
            }
            else
            {
                Debug.LogError("На сцене нету Game_administrator, нужно исправить и добавить на сцену!");
            }
        }

        void Start()
        {
            Skill_UI.Singleton_Instance.Preparation(Skills_array.Length, Input_player_script);

            for (int i = 0; i < Skills_array.Length; i++)
            {
                Skills_array[i].Preparation(i, Max_change_image_stop_bool);
            }
        }


        private void Update()
        {
            for (int i = 0; i < Skills_array.Length; i++)
            {
                Skills_array[i].Update_Time_reload();
            }
        }

        public void Activation_skill(int _id)
        {
            if(_id < Skills_array.Length)
                Skills_array[_id].Activation_skill();
        }

        public void Kill_enemy(Character_abstract _enemy)
        {
            Add_score_charge(_enemy.Health_script.Health_max);
        }

        public void Damage_enemy(Character_abstract _enemy)
        {
            Add_score_charge(_enemy.Health_script.Health_max);
        }

        public void Add_score_charge(int _value)
        {
            foreach (Skill_class skill in Skills_array)
            {
                if(!skill.Manual_charging_bool)
                skill.Add_score_charge(_value);
            }
        }

        public void Add_charging_skill(int _id, float _value)
        {
            Skills_array[_id].Add_score_charge(_value);
        }

        public void Add_charging_skill(int _id)
        {
            Skills_array[_id].Add_ammo(1);
        }

        public bool Check_full_ammo(int _id)
        {
            return Skills_array[_id].Check_full_ammo;
        }

        public void Save()
        {
            for(int x = 0; x < Skills_array.Length; x++)
            {
                Save_PlayerPrefs.Save_parameter(Type_parameter_value_int.Charged_skill_value, Skills_array[x].Skill_script.name, Skills_array[x].Ammo);
            } 
        }

        public void Load()
        {

            if (!Save_PlayerPrefs.Know_parameter(Type_parameter_bool.New_game))
            {
                for (int x = 0; x < Skills_array.Length; x++)
                {
                    Skills_array[x].Ammo = Save_PlayerPrefs.Know_parameter(Type_parameter_value_int.Charged_skill_value, Skills_array[x].Skill_script.name);
                    
                    Skills_array[x].Preparation_2();
                }
            }

        }

        [System.Serializable]
        class Skill_class
        {
            [Tooltip("Скилл")]
            [SerializeField]
            public Skill_abstract Skill_script = null;

            [Min(0)]
            [Tooltip("Количество зарядов")]
            [SerializeField]
            public int Ammo = 0;

            [Min(0)]
            [Tooltip("Максимальное количество зарядов")]
            [SerializeField]
            int Ammo_max = 0;

            [field: Tooltip("Не заряжается от урона по противнику или смерти противника")]
            [field: SerializeField]
            public bool Manual_charging_bool { get; private set; } = false;

            [Min(1)]
            [Tooltip("Сколько нужно заряжать")]
            [SerializeField]
            float Charge_value = 0;

            float Charge_max = 0;

            [Tooltip("Перезарядка по времени")]
            [SerializeField]
            bool Time_reload_bool = false;

            [Tooltip("Сколько времени будет перезаряжаться")]
            [SerializeField]
            float Time_reload = 1f;

            int Id = 0;

            bool Max_change_image_stop_bool = false;

            public void Preparation(int _id, bool _max_change_image_stop_bool)
            {
                Id = _id;

                Charge_max = Charge_value;

                Max_change_image_stop_bool = _max_change_image_stop_bool;

                Charge_value = Max_change_image_stop_bool && Ammo >= Ammo_max ? Charge_max : 0;

                UI_update();
            }

            public void Preparation_2()
            {
                if (Ammo_max > Ammo)
                    Charge_value = 0;

                UI_update();
            }

            public void Update_Time_reload()
            {
                if (Time_reload_bool && Ammo < Ammo_max)
                {
                    float value = 1 / Time_reload * Time.deltaTime;
                    Add_score_charge(value);
                }
                
            }

            public bool Check_full_ammo
            {
                get
                {
                    return Ammo >= Ammo_max ? true : false;
                }
            }

            void UI_update()
            {
                float charge_value = Charge_value;
                
                if (charge_value > 0)
                    charge_value =  Charge_value / Charge_max;

                Skill_UI.Singleton_Instance.Change_charge(Id, charge_value);
                Skill_UI.Singleton_Instance.Change_save_value(Id, Ammo);

                Skill_UI.Singleton_Instance.Active_change(Id, Ammo > 0 ? true : false);
            }

            /// <summary>
            /// Заполнить целыми зарядами
            /// </summary>
            /// <param name="_value"></param>
            public void Add_ammo(int _value)
            {
                Ammo += _value;

                if(Ammo > Ammo_max)
                    Ammo = Ammo_max;

                if (Ammo >= Ammo_max)
                {
                    Charge_value = Max_change_image_stop_bool ? Charge_max : 0;
                }

                UI_update();
            }

            /// <summary>
            /// Частичное заполнение заряда
            /// </summary>
            /// <param name="_value"></param>
            public void Add_score_charge(float _value)
            {
                if (Ammo < Ammo_max) 
                {
                    
                    Charge_value += _value;

                    int multiply = (int)Mathf.Floor((float)Charge_value / (float)Charge_max);

                    if (multiply < 1)
                    {
                        multiply = 1;
                    }

                    
                    if (Charge_value >= Charge_max)
                    {
                        Add_ammo(multiply);
                            
                        if(Ammo < Ammo_max)
                        {
                            float value_buffer = Charge_value - Charge_max * multiply;
                            Charge_value = value_buffer;
                        }

                    }

                    //print(Charge_value + "  d " + Charge_max + "  v  " + Ammo);

                    UI_update();
                }
            }

            public void Activation_skill()
            {
                if(Ammo > 0)
                {
                    if (Max_change_image_stop_bool && Ammo >= Ammo_max)
                        Charge_value = 0;


                    Skill_script.Activation();
                    Ammo--;
                    UI_update();
                }
                    
            }
        }
    }
}