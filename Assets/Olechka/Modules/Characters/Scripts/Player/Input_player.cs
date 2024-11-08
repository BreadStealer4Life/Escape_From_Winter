using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using NaughtyAttributes;
using Unity.VisualScripting;

namespace Olechka
{
    enum Enum_direction
    {
        Global_direction,
        Camera_direction
    }

    public class Input_player : MonoBehaviour
    {

        [Tooltip("Тип направлений")]
        [SerializeField]
        Enum_direction Type_direction = Enum_direction.Global_direction;

        [ShowIf(nameof(Type_direction), Enum_direction.Camera_direction)]
        [Tooltip("Камера")]
        [SerializeField]
        Camera Cam = null;

        [Foldout("Кнопки управления")]
        [Tooltip("Кнопка Передвижение влево")]
        [SerializeField]
        KeyCode Left_move_input = KeyCode.A;

        [Foldout("Кнопки управления")]
        [Tooltip("Кнопка Передвижение вправо")]
        [SerializeField]
        KeyCode Right_move_input = KeyCode.D;

        [Foldout("Кнопки управления")]
        [Tooltip("Кнопка Передвижение вперёд")]
        [SerializeField]
        KeyCode Forward_move_input = KeyCode.W;

        [Foldout("Кнопки управления")]
        [Tooltip("Кнопка Передвижение назад")]
        [SerializeField]
        KeyCode Back_move_input = KeyCode.S;

        [Foldout("Кнопки управления")]
        [Tooltip("Кнопка Прыжка")]
        [SerializeField]
        KeyCode Jump_input = KeyCode.Space;

        [Foldout("Кнопки управления")]
        [Tooltip("Кнопка Выстрела")]
        [SerializeField]
        KeyCode Fire_input = KeyCode.Mouse0;

        [Foldout("Кнопки управления")]
        [Tooltip("Кнопка Перезарядки")]
        [SerializeField]
        KeyCode Reload_input = KeyCode.R;

        [SerializeField]
        UnityEvent Reload_event = new UnityEvent();

        [field: Foldout("Кнопки управления")]
        [field: Tooltip("Кнопки Способностей")]
        [field: SerializeField]
        public KeyCode[] Skill_input_array { get; private set; } = new KeyCode[0];



        [Space(20)]
        [Tooltip("Скрипт оружия")]
        [SerializeField]
        Gun Gun_script = null;

        [Tooltip("Скрипт движения персонажа")]
        [SerializeField]
        Move_character Player_controller_sctipt = null;

        [Tooltip("Администратор скиллов игрока")]
        [SerializeField]
        Skills_administrator Skills_administrator_script = null;

        bool Active_bool = true;

        private void Start()
        {
            if (Game_administrator.Singleton_Instance)
            {
                Game_administrator.Singleton_Instance.Start_game_event.AddListener(On_active);
                Game_administrator.Singleton_Instance.End_game_event.AddListener(Stop_active);
            }
                
        }

        void Update()
        {
            if (Active_bool)
            {
                if (Input.GetKeyDown(Reload_input))
                {
                    Reload_event.Invoke();
                    if(Gun_script)
                    Gun_script.Start_reload();
                    
                }
                    

                if (Input.GetKeyDown(Fire_input))
                    Gun_script.Activation(true);
                else if (Input.GetKeyUp(Fire_input))
                    Gun_script.Activation(false);

                if (Player_controller_sctipt)
                {
                    Vector2 direction = Vector2.zero;

                    if (Input.GetKey(Left_move_input))
                        direction.x = -1;
                    else if(Input.GetKey(Right_move_input))
                        direction.x = 1;

                    if (Input.GetKey(Forward_move_input))
                        direction.y = 1;
                    else if (Input.GetKey(Back_move_input))
                        direction.y = -1;

                    Vector3 finale_direction = Vector2.zero;

                    if (Type_direction == Enum_direction.Global_direction)
                        finale_direction = direction;
                    else if (Type_direction == Enum_direction.Camera_direction)
                    {
                        Vector3 forward_direction = Vector3.zero;

                        if(direction.y != 0)
                        {
                            forward_direction = Cam.transform.position + Cam.transform.forward * 10f;
                            forward_direction.y = Cam.transform.position.y;
                            forward_direction = (forward_direction - Cam.transform.position).normalized;
                            forward_direction *= direction.y;
                        }


                        Vector3 right_direction = Vector3.zero;

                        if (direction.x != 0) 
                        {
                            right_direction = Cam.transform.position + Cam.transform.right * 10f;
                            right_direction.y = Cam.transform.position.y;
                            right_direction = (right_direction - Cam.transform.position).normalized;
                            right_direction *= direction.x;
                        }

                        finale_direction = (forward_direction + right_direction).normalized;

                        //print(finale_direction);
                    }

                    Player_controller_sctipt.Input_change(finale_direction);

                    if (Input.GetKeyDown(Jump_input))
                        Player_controller_sctipt.Jump();
                }

                for (int x = 0; x < Skill_input_array.Length; x++)
                {
                    if (Input.GetKeyDown(Skill_input_array[x]))
                    {
                        Skills_administrator_script.Activation_skill(x);
                    }
                        
                }

                /*
                if (Input.GetKeyDown(KeyCode.Q))
                    Skills_administrator_script.Activation_skill(0);
                if (Input.GetKeyDown(KeyCode.E))
                    Skills_administrator_script.Activation_skill(1);
                if (Input.GetKeyDown(KeyCode.F))
                    Skills_administrator_script.Activation_skill(2);
                if (Input.GetKeyDown(KeyCode.Z))
                    Skills_administrator_script.Activation_skill(3);
                */
            }
        }

        void Stop_active()
        {
            Active_bool = false;
        }

        void On_active()
        {
            Active_bool = true;
        }
    }
}
