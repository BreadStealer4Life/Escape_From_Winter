//Передвигает объект по точкам
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;
using UnityEngine.UIElements;
using UnityEditor;
using System.IO;

namespace Olechka
{
    enum Movement_enum
    {
        Last_end,//Дойти до последней точки и остановится
        Loop,//Зациклить движение от первой к последней и заново (перейдёт от последней к первой)
        Ping_pong//Зациклить передвижение от первой к последней и обратно
    }

    enum Point_enum
    {
        FreeMoveHandle,
        PositionHandle
    }

    enum Rotation_direct_way_enum
    {
        XYZ,
        XZ,
        XY
    }

    [AddComponentMenu("Olechka scripts / Game / Movement / Movement object to points")]
    [DisallowMultipleComponent]
    public class Movement_object_to_points : MonoBehaviour
    {

        [field: SerializeField]
        public UnityEngine.Color Main_color { get; private set; } = UnityEngine.Color.blue;

        /*
        [Tooltip("Вращать по физике?")]
        [SerializeField]
        bool Physics_bool = false;
        

        [ShowIf(nameof(Physics_bool))]
        [Tooltip("Перемещает по физике (если вдруг нужно)")]
        [SerializeField]
        Rigidbody Body = null;
        */

        [Min(0.1f)]
        [Tooltip("Плавность изгиба")]
        public float Smooth_bending = 1.1f;

        [field: Tooltip("Точки движения")]
        [field: SerializeField]
        public Point_class[] Points_array { get; private set; } = new Point_class[0];

        public Vector3[] Active_pos_points_array { get; private set; } = new Vector3[0];

        //-------------------------------------------------------------------------------------------------

        [Tooltip("Перенести сразу на позицию первой точки")]
        [SerializeField]
        bool Start_first_point_position_bool = false;

        //Vector3[] Points_vector_array = new Vector3[0];

        [Tooltip("Скорость")]
        [SerializeField]
        public float Speed = 1.1f;

        [Tooltip("Распределить все элементы равномерно по всей длине пути")]
        [SerializeField]
        bool Uniform_distribution_bool = false;

        [Tooltip("Поворачивает объект в сторону движения")]
        [SerializeField]
        bool Rotation_direct_way_bool = false;

        [ShowIf(nameof(Rotation_direct_way_bool))]
        [Tooltip("Скорость поворота в сторону движения")]
        [SerializeField]
        float Speed_rotation = 0.8f;

        [Tooltip("Тип манипуляторов для взаимодействия с точками")]
        [SerializeField]
        Point_enum Type_point = Point_enum.FreeMoveHandle;

        [Tooltip("Тип логики передвижения между точками")]
        [SerializeField]
        Movement_enum Type_movement = Movement_enum.Last_end;

        [Tooltip("Тип логики поворота по пути")]
        [SerializeField]
        Rotation_direct_way_enum Type_Rotation_direct_way = Rotation_direct_way_enum.XYZ;

        [Tooltip("Стартует сразу при запуске")]
        [SerializeField]
        bool Start_movement_bool = true;

        //-------------------------------------------------------------------------------------------------



        [Tooltip("Сегменты (если объект не цельный, а состоит из сегментов)")]
        [SerializeField]
        Transform[] Target_object_segments_array = new Transform[0];

        List <Point_setting_class> Point_setting_list = new List<Point_setting_class>();//Точки пути для сегментированного объекта

        List<float> Distance_between_segments_list = new List<float>();//Дистанция между сегментами

        //[Tooltip("Будет останавливаться каждый раз когда дойдёт до новой точки")]
        //[SerializeField]
        //bool Stop_next_point_bool = false;

        [Tooltip("Событие когда достиг конечной точки")]
        [SerializeField]
        UnityEvent End_point_event = new UnityEvent();

        //-------------------------------------------------------------------------------------------------

        [Foldout("Внутриние параметры(не трогать, если не нужно")]
        [Space(20)]


        [Tooltip("Увидеть больше")]
        [SerializeField]
        bool Gizmos_mode_bool = false;

        [ShowIf(EConditionOperator.And, nameof(Gizmos_mode_bool))]
        [Foldout("Внутриние параметры(не трогать, если не нужно")]
        [Tooltip("Размер точек")]
        [SerializeField]
        float Point_scalle = 0.2f;

        [ShowIf(EConditionOperator.And, nameof(Gizmos_mode_bool))]
        [Foldout("Внутриние параметры(не трогать, если не нужно")]
        [Tooltip("Цвет точек пути")]
        [SerializeField]
        UnityEngine.Color Point_way_color = UnityEngine.Color.red;

        [ShowIf(EConditionOperator.And, nameof(Gizmos_mode_bool))]
        [Foldout("Внутриние параметры(не трогать, если не нужно")]
        [Tooltip("Цвет точек Безье")]
        [SerializeField]
        UnityEngine.Color Point_bezier_color = UnityEngine.Color.yellow;

        
        [ShowIf(nameof(Gizmos_mode_bool))]
        [Foldout("Внутриние параметры(не трогать, если не нужно")]
        [Tooltip("Меш объекта")]
        [SerializeField]
        Mesh Object_mesh = null;

        [ShowIf(EConditionOperator.And, nameof(Gizmos_mode_bool))]
        [Foldout("Внутриние параметры(не трогать, если не нужно")]
        [Tooltip("Цвет отображамых мешей")]
        [SerializeField]
        UnityEngine.Color Color_mesh = new UnityEngine.Color(0f, 0f, 1f, 0.3f);

        Save_settings_gizmos_class Save_settings_gizmos = new Save_settings_gizmos_class();

        bool Active_bool = false;

        //**********************************************************************************************************


        #region Системные методы

        private void Start()
        {

            Preparation();

                if (Start_first_point_position_bool)
            {
                Target_object_segments_array[0].position = transform.position + Points_array[0].Pos_point;

                if (Uniform_distribution_bool)
                {
                    //Выясняем какое процентное соотношение растояния между платформами будете
                    float position_value = 1f / (float)Target_object_segments_array.Length;

                    Vector3[] data = Get_partial_path_point(position_value);

                    for (int x = 0; x < Target_object_segments_array.Length; x++)
                    {

                        Point_setting_list[x].Id_position_active = (int)data[x].y;

                        Vector3 new_position = Vector3.Lerp(Points_array[(int)data[x].x].Pos_point + transform.position, Points_array[(int)data[x].y].Pos_point + transform.position, data[x].z);
                        //print("p1 " + (Points_array[(int)data[x].x].Pos_point + transform.position) + " " + "p2 " + (Points_array[(int)data[x].y].Pos_point + transform.position) + " " + data[x].z);
                        //print(data[x]);
                        Target_object_segments_array[x].position = new_position;
                    }
                }
            }


            if (Start_movement_bool)
            {
                Button_start();
            }
        }

        

        private void Update()
        {
            if (Active_bool)
            {
                if (Target_object_segments_array.Length > 0)
                {
                    Move_segments_object();
                }
            }
        }

        #endregion


        //**********************************************************************************************************


        #region Методы

        //==============================================================================================

        #region Подготовительные методы

        /// <summary>
        /// Получить точку со всего пути в значение от 0 до 1
        /// </summary>
        /// <param name="_value"></param>
        Vector3[] Get_partial_path_point(float _value)
        {
            Vector3[] result = new Vector3[Target_object_segments_array.Length];
            //print(_value);
            float full_distance = 0f;

            Point_class[] active_points_array;

            for (int x = 1; x < Points_array.Length; x++)
            {
                full_distance += Vector3.Distance(Points_array[x - 1].Pos_point + transform.position, Points_array[x].Pos_point + transform.position);
            }

            if (Type_movement == Movement_enum.Loop)
            {
                full_distance += Vector3.Distance(Points_array[Points_array.Length - 1].Pos_point + transform.position, Points_array[0].Pos_point + transform.position);

                active_points_array = new Point_class[Points_array.Length + 1];

                for (int x = 0; x < Points_array.Length; x++)
                {
                    active_points_array[x] = Points_array[x];
                    //print("d  " + Points_array[x].Pos_point + transform.position);
                }

                active_points_array[active_points_array.Length - 1] = Points_array[0];

                //print("d  " + Points_array[0].Pos_point + transform.position);
            }
            else
            {
                active_points_array = new Point_class[Points_array.Length];

                for (int x = 0; x < Points_array.Length; x++)
                {
                    active_points_array[x] = Points_array[x];
                }
            }

            for (int i = 0; i < Target_object_segments_array.Length; i++)
            {
                float end_dist = full_distance * _value * i;

                float buffer_dist = end_dist;


                Vector2 id_points = Vector2.zero;

                float percent_point = 0;
                //print("buf  " + buffer_dist);
                for (int x = 1; x < active_points_array.Length; x++)
                {
                    float dist = Vector3.Distance(active_points_array[x - 1].Pos_point + transform.position, active_points_array[x].Pos_point + transform.position);

                    if (dist < buffer_dist)
                    {
                        buffer_dist -= dist;
                    }
                    else
                    {
                        id_points.x = x - 1;
                        id_points.x = x > Points_array.Length - 1 ? 0 : x - 1;
                        //print("razm  " + Points_array.Length + "iter  " + x);
                        //print("x  " + (x > Points_array.Length - 1 ? 0 : x));
                        id_points.y = x;

                        percent_point = buffer_dist / Vector3.Distance(active_points_array[x - 1].Pos_point + transform.position, active_points_array[x].Pos_point + transform.position);

                        result[i] = new Vector3(id_points.x, id_points.y, percent_point);
                        break;
                    }

                }
            }

            return result;
        }

        void Update_preparation()
        {
            if (!Check_old_way)
            {
                Active_pos_points_array = Get_Points_way();
            }
            
        }

        void Preparation()
        {
            if (Target_object_segments_array.Length > 1)
            {
                if (Distance_between_segments_list.Count == 0)
                {
                    for (int segment_id = 0; segment_id < Target_object_segments_array.Length - 1; segment_id++)
                    {
                        float distance = Vector3.Distance(Target_object_segments_array[segment_id].position, Target_object_segments_array[segment_id + 1].position);

                        Distance_between_segments_list.Add(distance);
                    }
                }
            }

            Praparation_points_way();


        }

        void Praparation_points_way()
        {

            Update_preparation();

            Point_setting_list.Clear();

            int number_segment = 1;

            number_segment = Target_object_segments_array.Length;


            for (int segment_id = 0; segment_id < number_segment; segment_id++)
            {
                Point_setting_list.Add(new Point_setting_class());

                for (int point_id = 0; point_id < Active_pos_points_array.Length; point_id++)
                {

                    int step = 0;

                    while (true)
                    {

                        Point_setting_list[segment_id].Position_list.Add(Active_pos_points_array[point_id]);
                        Point_setting_list[segment_id].Rotation_list.Add(Quaternion.identity);

                        Vector3 new_position = Point_setting_list[segment_id].Position_list[Point_setting_list[segment_id].Position_list.Count - 1];

                        if (Active_pos_points_array[point_id] == new_position)
                        {
                            break;
                        }
                        else if (step > 1000)
                        {
                            print("Более 1000 попыток.");
                            break;
                        }

                        step++;
                    }

                }
            }

                Preparation_segments(0);

        }

        void Preparation_segments(int _id_point)
        {
            Vector3 old_position = Vector3.zero;

            //Ставим на позицию сегменты
            Vector3 direction_start = Point_setting_list[0].Position_list[_id_point] - Point_setting_list[0].Position_list[_id_point + 1];
            direction_start.Normalize();


            for (int segment_id = 0; segment_id < Target_object_segments_array.Length; segment_id++)
            {
                Vector3 position_segment;

                if (segment_id == 0)
                {
                    position_segment = Point_setting_list[segment_id].Position_list[_id_point];
                    old_position = position_segment;
                }
                else
                {
                    position_segment = old_position + direction_start * Distance_between_segments_list[segment_id - 1];
                    old_position = position_segment;
                }

                //Target_object_segments_array[segment_id].transform.position = position_segment;
            }
        }

        void Preparation_Save_settings_gizmos()
        {
            Save_settings_gizmos.Old_Smooth_bending = Smooth_bending;

            Save_settings_gizmos.Type_movement = Type_movement;

            Save_settings_gizmos.Point_position_array = new Vector3[Points_array.Length];
            Save_settings_gizmos.Mode_Bezier_bool_array = new bool[Points_array.Length];
            Save_settings_gizmos.Point_Bezier_position_array = new Vector3[Points_array.Length];

            for (int x = 0; x < Points_array.Length; x++)
            {
                Save_settings_gizmos.Point_position_array[x] = Points_array[x].Pos_point;

                Save_settings_gizmos.Point_Bezier_position_array[x] = Points_array[x].Pos_Bezier_point;
                Save_settings_gizmos.Mode_Bezier_bool_array[x] = Points_array[x].Bezier_mode;
            }
        }

        #endregion

        //==============================================================================================

        #region Исполняюще методы
        /// <summary>
        /// Передвигает сегментированного персонажа
        /// </summary>
        void Move_segments_object()
        {
            float mod_speed = 1;

            bool stop_bool = false;

            for (int x = 0; x < Target_object_segments_array.Length; x++)
            {
                int position_id_active = Point_setting_list[x].Id_position_active;

                Vector3 next_position = Vector3.zero;

                while (true)
                {
                    if (Point_setting_list[x].Position_list.Count > position_id_active)
                    {
                        //print("Позиция 1 = " + Target_object_segments_array[x].transform.position + "  Позиция 2" + next_position);
                        next_position = Point_setting_list[x].Position_list[position_id_active] + transform.position;

                        if (Vector3.Distance(Target_object_segments_array[x].transform.position, next_position) < (Speed) * Time.deltaTime)
                        {
                            Point_setting_list[x].Id_position_active += 1;
                            position_id_active = Point_setting_list[x].Id_position_active;

                        }
                        else
                            break;
                    }
                    else
                    {
                        //print(1);
                        //if (Type_movement == Movement_enum.Loop)
                        //{
                        //    Point_setting_list[x].Id_position_active = 0;
                        //}
                        //else
                        //{
                            stop_bool = true;
                            End_way();
                            break;
                        //}
                    }

                }

                if (!stop_bool)
                {

                    //Передвигаем объект
                    //------------------------------------------------

                    //if(x != 0)
                    //mod_speed = mod_speed + 0.1f * Test_check_segments(x - 1, x);

                    Vector3 new_position = Vector3.MoveTowards(Target_object_segments_array[x].transform.position, next_position, Speed * mod_speed * Time.deltaTime);
                    //Vector3 dir_move = Point_setting_list[x].Position_list[position_id_active] - Target_object_segments_array[x].position;


                    //Vector3 new_position = Target_object_segments_array[x].position + dir_move * Speed * Time.deltaTime;

                    Target_object_segments_array[x].transform.position = new_position;



                    //Поворачиваем объект
                    //------------------------------------------------
                    if (Rotation_direct_way_bool)
                    {

                        Vector3 direction_look;


                        Vector3 target = Target_object_segments_array[x].transform.position;

                        switch (Type_Rotation_direct_way)
                        {
                            case Rotation_direct_way_enum.XY:
                                target.z = Point_setting_list[0].Position_list[position_id_active].z;
                                break;

                            case Rotation_direct_way_enum.XZ:
                                target.y = Point_setting_list[0].Position_list[position_id_active].y;
                                break;
                        }

                        if (x == 0)
                            direction_look = next_position - target;
                        else
                            direction_look = Target_object_segments_array[x - 1].transform.position - target;

                        if (direction_look != Vector3.zero)
                        {
                            Quaternion fin_rotation = Quaternion.LookRotation(direction_look);

                            if (x == 0)
                                Target_object_segments_array[x].transform.rotation = Quaternion.RotateTowards(Target_object_segments_array[x].transform.rotation, fin_rotation, Speed_rotation * 10f * Time.deltaTime);
                            else
                                Target_object_segments_array[x].transform.rotation = fin_rotation;
                        }

                    }
                    //Проверяем дошли ли до конца
                    //------------------------------------------------


                    //if (Vector3.Distance(new_position, Point_setting_list[x].Position_list[position_id_active]) <= (Speed * 1.5f ) * Time.deltaTime)
                    //{

                    //Point_setting_list[x].Id_position_active += 1;
                    if (Point_setting_list[x].Id_position_active >= Point_setting_list[x].Position_list.Count)
                    {
                        End_way();
                    }
                    //}
                }
            }
        }


        //Получить массив точек которые составляет путь
        Vector3[] Get_Points_way()
        {
            Vector3[] result = new Vector3[0];

            List<Vector3> buffer_result = new List<Vector3>();

            

            if (Points_array.Length > 0)
            {
                int number_point = Points_array.Length + 1;

                for (int point_id = 0; point_id < Points_array.Length; point_id++)
                {
                    if (!Points_array[point_id].Bezier_mode)
                    {
                        if (buffer_result.Count == 0 || buffer_result[buffer_result.Count - 1] != Points_array[point_id].Pos_point)
                            buffer_result.Add(Points_array[point_id].Pos_point);

                        if (point_id == Points_array.Length - 1 && Type_movement == Movement_enum.Loop)
                            buffer_result.Add(Points_array[0].Pos_point);

                    }
                    else
                    {
                        Vector3 point_start = Points_array[point_id].Pos_point;

                        Vector3 point_end = Vector3.zero;

                        if (point_id + 1 < Points_array.Length)
                            point_end = Points_array[point_id + 1].Pos_point;
                        else if (Type_movement == Movement_enum.Loop)
                            point_end = Points_array[0].Pos_point;
                        else
                            continue;


                        Vector3[] points_array;

                        int step = 2;

                        while (true)
                        {
                            points_array = Game_Bezier_curve.Get_Points_from_Bezier(point_start, Points_array[point_id].Pos_Bezier_point, point_end, step);

                            if (Vector3.Distance(points_array[0], points_array[1]) <= Smooth_bending)
                                break;

                            else if (step > 1000)
                            {
                                print("Слишком много попыток");
                                break;
                            }
                            step++;
                        }

                        //Vector3[] points_array = Game_Bezier_curve.Get_Points_from_Bezier(point_start, Point_way.Points_array[point_id].Point_bezier.position, Point_way.Points_array[point_id].Point_way.position, Point_way.Points_array[point_id].Step);


                        Vector3 old_pos = Vector3.zero;

                        for (int x = 0; x < points_array.Length; x++)
                        {
                            buffer_result.Add(points_array[x]);
                            old_pos = points_array[x];

                        }

                    }

                }
            }

            Preparation_Save_settings_gizmos();

            result = Game_calculator.Convert_from_List_to_Array(buffer_result);

            return result;
        }

        /// <summary>
        /// Дошли до конца
        /// </summary>
        void End_way()
        {
            Active_bool = false;
            End_point_event.Invoke();

            for (int x = 0; x < Point_setting_list.Count; x++)
            {

                if(Points_array.Length < Point_setting_list[x].Id_position_active)
                    Point_setting_list[x].Id_position_active = 0;
            }

            switch (Type_movement)
            {
                case Movement_enum.Last_end:
                    Active_bool = false;
                    break;

                case Movement_enum.Loop:
                    Active_bool = true;
                    break;

                case Movement_enum.Ping_pong:
                    Active_bool = true;

                    for (int x = 0; x < Point_setting_list.Count; x++)
                    {
                        Point_setting_list[x].Position_list.Reverse();
                    }

                    break;
            }
        }

        #endregion

        //==============================================================================================

        #region Проверяющие методы
        private bool Check_old_way
        {
            get
            {
                bool result = true;

                if (Save_settings_gizmos.Old_Smooth_bending != Smooth_bending)
                    result = false;

                if(Save_settings_gizmos.Type_movement != Type_movement)
                    result = false;

                if (Save_settings_gizmos.Point_position_array.Length != Points_array.Length ||
                    Save_settings_gizmos.Point_Bezier_position_array.Length != Points_array.Length)
                    result = false;

                if (result == true)
                    for (int x = 0; x < Points_array.Length; x++)
                    {

                        if (Save_settings_gizmos.Point_position_array[x] != Points_array[x].Pos_point ||
                            Save_settings_gizmos.Point_Bezier_position_array[x] != Points_array[x].Pos_Bezier_point)
                        {
                            result = false;
                            break;
                        }


                        if (Save_settings_gizmos.Point_Bezier_position_array[x] != Points_array[x].Pos_Bezier_point)
                        {
                            result = false;
                            break;
                        }

                        if (Points_array[x].Bezier_mode != Save_settings_gizmos.Mode_Bezier_bool_array[x])
                        {
                            result = false;
                            break;
                        }

                    }

                return result;
            }
        }
        #endregion

        //==============================================================================================

        #region Остальные методы
        /*
        [ContextMenu("Создать новую точку")]
        private void Button_new_point()
        {

            StartCoroutine(Coroutine_New_point());

        }
        */

        [ContextMenu("Запустить ")]
        private void Button_start()
        {
                Active_bool = true;

                if (Points_array.Length < 2)
                {
                    Debug.LogError("Недостаточно точек для передвижения!");
                }
        }


        [ContextMenu("Остановить ")]
        private void Button_stop()
        {
                Active_bool = false;
        }


        IEnumerator Coroutine_New_point()
        {

            int count = Points_array.Length;

            Point_class[] pos_point_array_old = Points_array;

            Points_array = new Point_class[count + 1];

            for (int x = 0; x < count; x++)
            {
                Points_array[x] = pos_point_array_old[x];
            }

            Vector3 direction = Vector3.zero;

            Vector3 new_position = Vector3.zero;

            if (pos_point_array_old.Length == 0)
            {
                direction = transform.forward;
                new_position = transform.InverseTransformPoint(transform.position) + direction * 1f;
            }
            else if (pos_point_array_old.Length == 1)
            {
                direction = pos_point_array_old[0].Pos_point - transform.InverseTransformPoint(transform.position);
                direction.Normalize();

                new_position = pos_point_array_old[0].Pos_point + direction * 1f;
            }
            else
            {
                direction = pos_point_array_old[pos_point_array_old.Length - 1].Pos_point - pos_point_array_old[pos_point_array_old.Length - 2].Pos_point;
                direction.Normalize();

                new_position = pos_point_array_old[pos_point_array_old.Length - 1].Pos_point + direction * 1f;
            }

            yield return new WaitForSeconds(0.1f);

            Update_preparation();

            Points_array[Points_array.Length - 1].Pos_point = new_position;
        }

        #endregion

        //==============================================================================================

        #endregion


        //**********************************************************************************************************


        #region Публичные методы

        //==============================================================================================

        #region Подготовительные методы

        #endregion

        //==============================================================================================

        #region Исполняюще методы
        public void Set_way(Vector3[] _points, float _speed)
        {
            print("Доделать функцию установки нового пути.");
        }

        /// <summary>
        /// Вкл/Выкл
        /// </summary>
        /// <param name="_activity">Активность</param>
        public void Activity(bool _activity)
        {
            Active_bool = _activity;
        }


        /// <summary>
        /// Изменить параметр точки
        /// </summary>
        /// <param name="_pos"></param>
        /// <param name="_id_pos"></param>
        public void Set_point_way_position(Vector3 _pos, int _id_pos)
        {
            Points_array[_id_pos].Pos_point = transform.InverseTransformPoint(_pos);
        }

        /// <summary>
        /// Изменить параметр точки Безье
        /// </summary>
        /// <param name="_pos"></param>
        /// <param name="_id_pos"></param>
        public void Set_point_bezier_position(Vector3 _pos, int _id_pos)
        {
            Points_array[_id_pos].Pos_Bezier_point = transform.InverseTransformPoint(_pos);
        }


        //Сбрасывает позицию на изначальную
        public void Reset()
        {

            transform.position = Points_array[0].Pos_point;

            Point_setting_list.Clear();
            Preparation();

            Active_bool = true;
        }

        #endregion

        //==============================================================================================

        #region Проверяющие методы

        #endregion

        //==============================================================================================

        #region Остальные методы

        #endregion

        //==============================================================================================

        #endregion

        //**********************************************************************************************************

        #region Для отображения в редакторе

        private void OnValidate()
        {
            Update_preparation();
        }

        private void OnDrawGizmosSelected()
        {
            //Отображение направляющих точек для кривых Безье
            //------------------------------------------------------

            Gizmos.color = Main_color;
            Gizmos.DrawSphere(transform.position, 0.3f * Point_scalle);

            for (int i = 0; i < Points_array.Length; i++)
            {
                Vector3 point_position = transform.position + Points_array[i].Pos_point;


                if (Type_point == Point_enum.PositionHandle)
                {
                    //Gizmos.DrawIcon(point_position, "Light Gizmo.tiff", true, UnityEngine.Color.blue);
                    Gizmos.color = UnityEngine.Color.yellow;
                    Gizmos.DrawSphere(point_position, 0.2f);
                }


                //Основные точки
                Gizmos.color = Points_array[i].Color_point;
                Gizmos.DrawSphere(point_position, 0.3f * Point_scalle);

                //Точки отвечающие на кривую Безье
                if (Points_array[i].Bezier_mode)
                {
                    Vector3 pos_point_bezier = transform.position + Points_array[i].Pos_Bezier_point;

                    Gizmos.color = Point_bezier_color;
                    Gizmos.DrawSphere(pos_point_bezier, 0.35f * Point_scalle);

                    //Показывает линию к какой кривой прикреплены точки
                    if(Points_array.Length - 1 > i) 
                    {
                        Vector3 line_start_pos = Game_Bezier_curve.Get_point_Bezier(transform.position + Points_array[i].Pos_point,
                         pos_point_bezier, 
                         transform.position + Points_array[i + 1].Pos_point, 0.5f);

                        Gizmos.DrawLine(line_start_pos, pos_point_bezier);
                    }

                    else if (Type_movement == Movement_enum.Loop)
                    {
                        Vector3 line_start_pos = Game_Bezier_curve.Get_point_Bezier(transform.position + Points_array[i].Pos_point,
                        pos_point_bezier,
                        transform.position + Points_array[0].Pos_point, 0.5f);

                        Gizmos.DrawLine(line_start_pos, pos_point_bezier);
                    }
                        
                }
            }
        }

        private void OnDrawGizmos()
        {
            //print(Active_pos_points_array.Length);

            if (Gizmos_mode_bool)
            {
                for (int point_id = 0; Points_array.Length > point_id; point_id++)
                {
                    
                    Vector3 point_start = Vector3.zero;

                    Vector3 point_end = Vector3.zero;

                    if (point_id == 0)
                        point_start = Points_array[0].Pos_point;
                    else
                        point_start = Points_array[point_id - 1].Pos_point;

                    point_end = Points_array[point_id].Pos_point;

                    //Отображение как будет выглядеть объект на ключевых точках
                    //------------------------------------------------------
                    if (Object_mesh != null)
                    {
                        Gizmos.color = Color_mesh;
                        Gizmos.DrawWireMesh(Object_mesh,transform.position + Points_array[0].Pos_point, Quaternion.identity);
                    }

                    //Отображение пути
                    //------------------------------------------------------

                    Gizmos.color = Point_way_color;

                    for (int x = 1; x < Active_pos_points_array.Length; x++)
                    {
                        Gizmos.DrawLine(transform.position + Active_pos_points_array[x - 1], 
                            transform.position + Active_pos_points_array[x]);
                        //Gizmos.DrawSphere(points_array[x], Point_scalle);
                    }

                }

            }
        }
        #endregion

        //**********************************************************************************************************

        #region Дополнительные классы

        [System.Serializable]
        public class Point_class
        {
            public string Name = "Name";

            public UnityEngine.Color Color_point = UnityEngine.Color.red;

            public Vector3 Pos_point = Vector3.zero;

            public Vector3 Pos_Bezier_point = Vector3.zero;

            [Tooltip("Сгладить путь сделав кривые Безье")]
            public bool Bezier_mode = false;

            /*
void OnGUI()
{
    if (!btnTexture)
    {
        Debug.LogError("Please assign a texture on the inspector");
        return;
    }

    if (GUI.Button(new Rect(10, 10, 50, 50), btnTexture))
        Debug.Log("Clicked the button with an image");

    if (GUI.Button(new Rect(10, 70, 50, 30), "Click"))
        Debug.Log("Clicked the button with text");
}
*/

        }

        #endregion

        //**********************************************************************************************************

#if UNITY_EDITOR
        [CustomEditor(typeof(Movement_object_to_points))]
        class Draw_point : Editor
        {
            void OnSceneGUI()
            {
                //Handles.color = UnityEngine.Color.red;

                Movement_object_to_points myObj = (Movement_object_to_points)target;

                /*
                //Handles.DrawWireArc(myObj.transform.position, myObj.transform.up, -myObj.transform.right, 180, myObj.shieldArea);

                //myObj.shieldArea = (float)Handles.ScaleValueHandle(myObj.shieldArea, myObj.transform.position + myObj.transform.forward * myObj.shieldArea, myObj.transform.rotation, 1, Handles.ConeHandleCap, 1);

                //int Id = GUIUtility.GetControlID(FocusType.Passive);


                Handles.SphereHandleCap(
        Id,
        myObj.transform.position + new Vector3(0f, 0f, 1f),
        myObj.transform.rotation * Quaternion.LookRotation(Vector3.forward),
        1f,
        EventType.Repaint
        );
                */

                //float size = HandleUtility.GetHandleSize(myObj.pos_point) * 0.5f;

                Vector3 snap = Vector3.one * 5;

                int count_points = myObj.Points_array.Length;

                if (count_points > 0)
                    for (int x = 0; x < count_points; x++)
                    {
                        Vector3 new_position_point = Vector3.zero;

                        Vector3 new_position_bezier = Vector3.zero;

                        //Handles.color = myObj.Point_array[x].Color_point;
                        switch (myObj.Type_point)
                        {
                            case Point_enum.FreeMoveHandle:
                                new_position_point = Handles.FreeMoveHandle(myObj.transform.position + myObj.Points_array[x].Pos_point, 1f, snap, Handles.RectangleHandleCap);

                                if (myObj.Points_array[x].Bezier_mode)
                                {
                                    new_position_bezier = Handles.FreeMoveHandle(myObj.transform.position + myObj.Points_array[x].Pos_Bezier_point, 1f, snap, Handles.RectangleHandleCap);
                                }
                                break;

                            case Point_enum.PositionHandle:
                                Vector3 position_point = myObj.transform.position + myObj.Points_array[x].Pos_point;

                                new_position_point = Handles.PositionHandle(position_point, Quaternion.identity);

                                if (myObj.Points_array[x].Bezier_mode)
                                {
                                    new_position_bezier = Handles.PositionHandle(myObj.transform.position + myObj.Points_array[x].Pos_Bezier_point, Quaternion.identity);
                                }
                                break;
                        }
                        
                            


                        if (EditorGUI.EndChangeCheck())
                        {
                            myObj.Update_preparation();
                            //Undo.RecordObject(myObj, "Change Look At Target Position");
                            myObj.Set_point_way_position(new_position_point, x);

                            if (myObj.Points_array[x].Bezier_mode)
                                myObj.Set_point_bezier_position(new_position_bezier, x);
                        }
                    }



                //myObj.pos_point = Handles.PositionHandle(myObj.pos_point, Quaternion.identity);
            }

        }
#endif

    }


    //%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%


    #region Внешние классы

    [System.Serializable]
    class Save_settings_gizmos_class
    {
        public float Old_Smooth_bending = 0;

        public Vector3[] Point_position_array = new Vector3[0];

        public bool[] Mode_Bezier_bool_array = new bool[0];

        public Vector3[] Point_Bezier_position_array = new Vector3[0];

        public Movement_enum Type_movement = Movement_enum.Last_end;
    }

    [System.Serializable]
    class Point_setting_class
    {
        public List<Vector3> Position_list = new List<Vector3>();

        public List<Quaternion> Rotation_list = new List<Quaternion>();

        public int Id_position_active = 0;
    }

    #endregion

}