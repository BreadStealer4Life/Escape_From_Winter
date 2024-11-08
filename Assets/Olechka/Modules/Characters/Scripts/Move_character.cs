using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Olechka
{
    public class Move_character : MonoBehaviour
    {

        [Tooltip("Скорость передвижения")]
        [SerializeField]
        float Walk_speed = 1.1f;

        [Tooltip("Скорость передвижения при беге")]
        [SerializeField]
        float Run_speed = 5.4f;

        [Tooltip("Скорость поворота меша")]
        [SerializeField]
        float Speed_rotation = 30f;

        [Tooltip("Автоматически поворачивать меш в сторону движения")]
        [SerializeField]
        bool Automatic_turn_in_direction_movement_bool = false;

        [Tooltip("Высота прыжка")]
        [SerializeField]
        float Jump_height = 4f;

        [Tooltip("Сила гравитации")]
        [SerializeField]
        float Gravity_value = -60f;

        [Tooltip("Время Койота (сколько даётся игроку времени на возможность отпрыгнуть от воздуха)")]
        [SerializeField]
        float Сoyote_time = 0.1f;

        [Tooltip("Меш персонажа")]
        [SerializeField]
        Transform Mesh = null;

        [Tooltip("Компонент CharacterController")]
        [SerializeField]
        CharacterController CharacterController_component = null;

        [Tooltip("Заблокировать движение по одной из осей")]
        [SerializeField]
        bool Lock_axis_X = false, Lock_axis_Y = false, Lock_axis_Z = false;

        Vector3 Move_direction = Vector2.zero;

        Vector3 Direct_last = Vector3.zero;

        bool Grounded_bool = false;

        bool Jump_bool = true;

        float Gravity_velocity = 0;

        Coroutine Rotation_mesh_coroutine = null;

        Coroutine Coyote_time_coroutine = null;

        bool Coyote_time_bool = false;

        bool Control_bool = true;

        bool Run_bool = false;



        [Space(15)]
        [Tooltip("Радиус детектора земли в ногах")]
        [SerializeField]
        float Radius_down = 0.45f;

        bool Fall_bool = false;

        [Tooltip("Смещение детектора земли в ногах")]
        [SerializeField]
        Vector3 Offset_down = new Vector3(0f, -0.65f, 0f);

        [Tooltip("Радиус детектора земли над головой")]
        [SerializeField]
        float Radius_up = 0.45f;

        [Tooltip("Смещение детектора земли над головой")]
        [SerializeField]
        Vector3 Offset_up = new Vector3(0f, 0.65f, 0f);

        [Tooltip("Слой для взаимодействия")]
        [SerializeField]
        LayerMask Mask_ground = 3;

        [Tooltip("Слой для объектов к которым будет прилипать игрок (например платформы которые двигаются)")]
        [SerializeField]
        LayerMask Mask_platform_connect = 3;

        [Tooltip("Режим отображения гизмоса")]
        [SerializeField]
        bool Gizmos_mode = false;

        [ShowIf(nameof(Gizmos_mode))]
        [Tooltip("Цвет гизмоса")]
        [SerializeField]
        Color Color_gizmos = new Color(0f, 1f, 0f, 0.3f);

        bool Move_bool = false;

        //[Space(20)]

        //[Tooltip("Слой для взаимодействия с платформами")]
        //[SerializeField]
        //LayerMask Mask_platform = 8;





        [Space (20)]
        [Header("Ивенты для всякого")]

        //[Tooltip("Ивент говорящий когда на земле или нет")]
        //[SerializeField]
        //UnityEvent<bool> Grounded_bool_event = new UnityEvent<bool>();

        [Foldout("Ивенты")]
        [Tooltip("Ивент говорящий с какой скоростью передвигаемся")]
        [SerializeField]
        UnityEvent<float> Speed_move_event = new UnityEvent<float>();

        [Foldout("Ивенты")]
        [Tooltip("Ивент движемся")]
        [SerializeField]
        UnityEvent Move_event = new UnityEvent();

        [Foldout("Ивенты")]
        [Tooltip("Ивент сигналищий, что прыгаем")]
        [SerializeField]
        UnityEvent Jump_event = new UnityEvent();

        [Foldout("Ивенты")]
        [Tooltip("Ивент когда приземлился на землю")]
        [SerializeField]
        UnityEvent Grounded_event = new UnityEvent();

        [Tooltip("Время между срабатыванием ивента, что бы сам ивент не срабатывал от каждой малейшей неровности")]
        [SerializeField]
        float Damper_event_grounded = 0.1f;

        bool Damper_event_grounded_bool = false;

        bool Damper_event_grounded_bool_2 = false;

        Coroutine Damper_event_grounded_coroutine = null;

        Vector3 Start_pos = Vector3.zero;



        private void Start()
        {
            Start_pos = CharacterController_component.transform.position;
        }

        void Update()
        {
            if (Control_bool)
            { 
                Move();

                Detect_down_ground();

                //Connected_platform();
            }
                
        }

        void Detect_down_ground()
        {
            Detect_up_ground();

            bool grounded_bool = Physics.CheckSphere(transform.position + Offset_down, Radius_down, Mask_ground, QueryTriggerInteraction.Ignore);
            

            if (grounded_bool)
            {
                
                if (!Grounded_bool)
                {

                    if (!Damper_event_grounded_bool || Jump_bool)
                    {
                        Grounded_event.Invoke();
                        //print(0);
                    }


                    Coyote_time_bool = false;
                    Gravity_velocity = 0f;
                    Jump_bool = false;
                }

            }
            else
            {
                if (!Jump_bool)
                {

                    if (Coyote_time_coroutine != null)
                        StopCoroutine(Coyote_time_coroutine);

                    StartCoroutine(Coroutine_Coyote_time());
                }
            }

            Grounded_bool = grounded_bool;
        }

        IEnumerator Coroutine_damper_event_grounded()
        {
            yield return new WaitForSecondsRealtime(Damper_event_grounded);
            
            Damper_event_grounded_bool = false;
            
        }


        void Detect_up_ground()
        {
            if (Jump_bool && !Fall_bool)
            {
                bool detect_ground_top = Physics.CheckSphere(transform.position + Offset_up, Radius_up, Mask_ground, QueryTriggerInteraction.Ignore);

                if (detect_ground_top)
                {
                    Gravity_velocity = 0f;
                    Fall_bool = true;
                }

                
            }
        }

        void Move()
        {
            if (!Grounded_bool)
            {
                Gravity_velocity += Gravity_value * Time.deltaTime;

                if(!Damper_event_grounded_bool && !Damper_event_grounded_bool_2 && Gravity_velocity != 0)
                {
                    Damper_event_grounded_bool = true;
                    Damper_event_grounded_bool_2 = true;

                    if (Damper_event_grounded_coroutine != null)
                    {
                        StopCoroutine(Damper_event_grounded_coroutine);
                    }
                    //print(2);
                    Damper_event_grounded_coroutine = StartCoroutine(Coroutine_damper_event_grounded());
                }

            }
            else if (Damper_event_grounded_bool || Damper_event_grounded_bool_2 && Gravity_velocity == 0)
            {
                if (Damper_event_grounded_coroutine != null)
                {
                    StopCoroutine(Damper_event_grounded_coroutine);
                }
                //print(1);
                Damper_event_grounded_bool_2 = false;
                Damper_event_grounded_bool = false;
            }

            Move_bool = true;
            
            float speed_walk = Run_bool ? Run_speed : Walk_speed;

            Vector3 move_direction = Move_direction * speed_walk;
            
            Speed_move_event.Invoke(move_direction.magnitude);

            if(move_direction.magnitude != 0)
                Move_event.Invoke();

            Vector3 direction_move = new Vector3(move_direction.x, Gravity_velocity, move_direction.z) * Time.deltaTime;

            if(CharacterController_component.enabled)
                CharacterController_component.Move(direction_move);

            Vector3 pos = CharacterController_component.transform.position;

            if (Lock_axis_X)
                pos.x = Start_pos.x;
            if (Lock_axis_Y)
                pos.y = Start_pos.y;
            if (Lock_axis_Z)
                pos.z = Start_pos.z;

            CharacterController_component.transform.position = pos;

            Move_direction = Vector2.zero;
        }

        IEnumerator Coroutine_rotation_mesh()
        {
            while (true)
            {
                yield return new WaitForFixedUpdate();

                Vector3 fin_direct = Direct_last;

                fin_direct = Mesh.position + fin_direct * 2f;

                Quaternion fin_rotation = Quaternion.LookRotation(fin_direct - Mesh.position);

                Mesh.rotation = Quaternion.RotateTowards(Mesh.rotation, fin_rotation, Speed_rotation);

                if (Mesh.rotation == fin_rotation)
                    break;
            }

            Rotation_mesh_coroutine = null;
        }

        IEnumerator Coroutine_Coyote_time()
        {
            Coyote_time_bool = true;

            yield return new WaitForSecondsRealtime(Сoyote_time);

            Coyote_time_bool = false;

        }

        public void Run_mode(bool _change)
        {
            Run_bool = _change;
        }

        public void Rotation_direction(Vector3 _direction)
        {
            if (Direct_last != _direction)
            {
                Direct_last = _direction;

                if (Rotation_mesh_coroutine != null)
                {
                    StopCoroutine(Rotation_mesh_coroutine);
                    Rotation_mesh_coroutine = null;
                }

                StartCoroutine(Coroutine_rotation_mesh());

            }
        }

        public void Right_direction(bool _right_direction)
        {
            Vector3 _direction = _right_direction ? Vector3.right : Vector3.left;

            Rotation_direction(_direction);
        }

        public void Input_change(Vector3 _direction)
        {
            if (Control_bool)
            {
                Move_direction = _direction;

                if(Automatic_turn_in_direction_movement_bool && _direction != Vector3.zero)
                    Rotation_direction(_direction);
            }
            else if(Move_bool)
            {
                Move_bool = false;
                Speed_move_event.Invoke(0);
            }
                



        }

        public void Control_change(bool _change)
        {
            Control_bool = _change;
        }

        public void Jump()
        {
            
            if ((Grounded_bool || Coyote_time_bool) && Jump_bool == false || Gravity_velocity == 0)
            {
                Gravity_velocity = 0f;
                
                Gravity_velocity += Mathf.Sqrt(Jump_height * -3.0f * Gravity_value);

                Jump_bool = true;
                
                Jump_event.Invoke();

                Fall_bool = false;

                CharacterController_component.Move(Vector3.up * 0.1f);
            }

        }


        private void OnDrawGizmosSelected()
        {
            if (Gizmos_mode)
            {
                Gizmos.color = Color_gizmos;

                Gizmos.DrawSphere(transform.position + Offset_up, Radius_up);

                Gizmos.DrawSphere(transform.position + Offset_down, Radius_down);
            }
        }

    }
}