//Статический скрипт для игровых вычислений
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Olechka
{
    public enum Find_by_distance_enum
    {
        XYZ,
        XY,
        XZ
    }

    public static class Game_calculator
    {
        /*
        /// <summary>
        /// Получить нужный конфиг объекта из System_configs
        /// </summary>
        /// <param name="_index">Индекс объекта (категория и номер)</param>
        /// <returns></returns>
        public static Config_object_SO Get_cofig_object (Vector2 _index)
        {
            Config_object_SO conf = null;

            if (_index.x > -1 && System_configs.Singleton_instance.Object_category_array.Length > _index.x && _index.y > -1 && System_configs.Singleton_instance.Object_category_array[(int)_index.x].Object_array.Length > _index.y)
            {
                conf = System_configs.Singleton_instance.Object_category_array[(int)_index.x].Object_array[(int)_index.y];
                conf.Main_name_category = System_configs.Singleton_instance.Object_category_array[(int)_index.x].Name_category;
                conf.Main_number_config = (int)_index.y;
            }
                
            else
                Debug.LogError("Конфига по ID  " + _index.x + " | " + _index.y + "  не существует!");

            return conf;
        }

        
        /// <summary>
        /// Найти ближайший объект или дальний по отношению к заданному (основному) по координатам XYZ
        /// </summary>
        /// <param name="_main_object">Основной объект от которого будет идти поиск</param>
        /// <param name="_objects_array">Список объектов</param>
        /// <param name="_nearest">Будем искать ближайший ? (или дальний)</param>
        /// <returns></returns>
        public static Transform Find_by_distance_object_XYZ(Transform _main_object, Transform[] _objects_array, bool _nearest)
        {

            return Find_by_distance_object_XYZ(_main_object.position, _objects_array, _nearest);
        }

        public static Transform Find_by_distance_object_XYZ(Transform _main_object, List<Transform> _objects_list, bool _nearest)
        {
            Transform[] array = Convert_from_List_to_Array(_objects_list);

            return Find_by_distance_object_XYZ(_main_object, array, _nearest);
        }

        /// <summary>
        /// Найти ближайший объект или дальний по отношению к точке координат по XYZ
        /// </summary>
        /// <param name="_main_point">Точка от которой будет поиск</param>
        /// <param name="_objects_array">Список объектов</param>
        /// <param name="_nearest">Будем искать ближайший ? (или дальний)</param>
        /// <returns></returns>
        public static Transform Find_by_distance_object_XYZ(Vector3 _main_point, Transform[] _objects_array, bool _nearest)
        {
            Transform result = _objects_array[0];

            float distance_active = Vector3.Distance(_main_point, result.position);

            for (int x = 0; x < _objects_array.Length; x++)
            {

                float dist = Vector3.Distance(_main_point, _objects_array[x].position);

                if (_nearest)
                {
                    if (distance_active > dist)
                    {
                        distance_active = dist;
                        result = _objects_array[x];
                    }
                }
                else
                {
                    if (distance_active < dist)
                    {
                        distance_active = dist;
                        result = _objects_array[x];
                    }
                }

            }

            return result;
        }
        */


        /// <summary>
        /// Найти ближайший объект или дальний по отношению к заданному (основному) по координатам
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="_main_point">Основной объект/позиция от которого будет идти поиск</param>
        /// <param name="_objects_array">Список объектов/позиция среди которых будут искать самый ближайший/дальний</param>
        /// <param name="_nearest">Будем искать ближайший ? (или дальний)</param>
        /// <param name="_type_find">Варианты поиска (по определённым осям)</param>
        /// <returns></returns>
        public static T2 Find_by_distance_object<T1, T2>(T1 _main_point, T2[] _objects_array, bool _nearest, Find_by_distance_enum _type_find) 
        {
            T2 result = _objects_array[0];

            Vector3 result_pos = T_thing_convert_vector3(result);

            Vector3 main_point = T_thing_convert_vector3(_main_point);

            float distance_active = Vector3.Distance(main_point, result_pos);

            for (int x = 0; x < _objects_array.Length; x++)
            {
                Vector3 transform_current = T_thing_convert_vector3(_objects_array[x]);

                Vector3 main_pos = Vector3.zero;
                Vector3 target_pos = Vector3.zero;

                switch (_type_find)
                {
                    case Find_by_distance_enum.XYZ:
                        main_pos = main_point;
                        target_pos = transform_current;
                    break;

                    case Find_by_distance_enum.XY:
                        main_pos = main_point;
                        main_pos.z = 0;
                        target_pos = transform_current;
                        target_pos.z = 0;
                        break;

                    case Find_by_distance_enum.XZ:
                        main_pos = main_point;
                        main_pos.y = 0;
                        target_pos = transform_current;
                        target_pos.y = 0;
                        break;
                }

                float dist = Vector3.Distance(main_pos, target_pos);

                if (_nearest)
                {
                    if (distance_active > dist)
                    {
                        distance_active = dist;
                        result = _objects_array[x];
                    }
                }
                else
                {
                    if (distance_active < dist)
                    {
                        distance_active = dist;
                        result = _objects_array[x];
                    }
                }

            }

            //Переконвертировать неявную переменную в конкретную
            Vector3 T_thing_convert_vector3<T>(T thing)
            {
                dynamic result = Vector3.zero;

                switch (thing)
                {
                    case Transform:
                        if((thing as Transform) == null)
                            Debug.LogError("Присланный образец пуст!");

                        result = (thing as Transform).position;
                        break;

                    case GameObject:
                        if ((thing as GameObject) == null)
                            Debug.LogError("Присланный образец пуст!");

                        result = (thing as GameObject).transform.position;
                        break;

                    case MonoBehaviour:
                        if ((thing as MonoBehaviour) == null)
                            Debug.LogError("Присланный образец пуст!");

                        result = (thing as MonoBehaviour).transform.position;
                        break;

                    case Vector3:
                        result = thing;
                        break;
                    default:
                        Debug.LogError("Неизвестный исходный формат!" + "  " + thing);
                        break;
                }

                return result;
            }


            return result;
        }




        public static T2 Find_by_distance_object<T1, T2>(T1 _main_point, List<T2> _objects_array, bool _nearest, Find_by_distance_enum _type_find)
        {
            T2[] array = Convert_from_List_to_Array(_objects_array);

            return Find_by_distance_object(_main_point, array, _nearest, _type_find);
        }




        /*
        /// <summary>
        /// Найти ближайший объект или дальний по отношению к заданному (основному) по координатам XY
        /// </summary>
        /// <param name="_main_object">Основной объект от которого будет идти поиск</param>
        /// <param name="_objects_array">Список объектов</param>
        /// <param name="_nearest">Будем искать ближайший ? (или дальний)</param>
        /// <returns></returns>
        public static Transform Find_by_distance_object_XY(Transform _main_object, Transform[] _objects_array, bool _nearest)//Найти ближайший объект или дальний по отношению к заданному (основному) по координатам XY
        {

            return Find_by_distance_object_XY(_main_object.position, _objects_array, _nearest);
        }

       

        /// <summary>
        /// Найти ближайший объект или дальний по отношению к точке координат по XY
        /// </summary>
        /// <param name="_main_point">Точка от которой будет поиск</param>
        /// <param name="_objects_array">Список объектов</param>
        /// <param name="_nearest">Будем искать ближайший ? (или дальний)</param>
        /// <returns></returns>
        public static T Find_by_distance_object_XY<T>(Vector3 _main_point, T[] _objects_array, bool _nearest)
        {
            T result = _objects_array[0];

            Vector3 result_pos = T_thing_convert_vector3(result);

            float distance_active = Vector3.Distance(new Vector3(_main_point.x, _main_point.y, 0), new Vector3(result_pos.x, result_pos.y, 0));

            for (int x = 0; x < _objects_array.Length; x++)
            {
                Vector3 transform_current = T_thing_convert_vector3(_objects_array[x]);

                float dist = Vector3.Distance(new Vector3(_main_point.x, _main_point.y, 0), new Vector3(transform_current.x, transform_current.y, 0));

                if (_nearest)
                {
                    if (distance_active > dist)
                    {
                        distance_active = dist;
                        result = _objects_array[x];
                    }
                }
                else
                {
                    if (distance_active < dist)
                    {
                        distance_active = dist;
                        result = _objects_array[x];
                    }
                }

            }

            return result;
        }

        public static T Find_by_distance_object_XY<T>(Vector3 _main_object, List<T> _objects_list, bool _nearest)//Найти ближайший объект или дальний по отношению к заданному (основному) по координатам XY
        {
            T[] array = Convert_from_List_to_Array(_objects_list);

            return Find_by_distance_object_XY(_main_object, array, _nearest);
        }


        /// <summary>
        /// Найти ближайший объект или дальний по отношению к заданному (основному) по координатам XZ
        /// </summary>
        /// <param name="_main_object">Основной объект от которого будет идти поиск</param>
        /// <param name="_objects_array">Список объектов</param>
        /// <param name="_nearest">Будем искать ближайший ? (или дальний)</param>
        /// <returns></returns>
        public static Transform Find_by_distance_object_XZ(Transform _main_object, Transform[] _objects_array, bool _nearest)
        {

            return Find_by_distance_object_XZ(_main_object.position, _objects_array, _nearest);
        }

        /// <summary>
        /// Найти ближайший объект или дальний по отношению к точке координат по XZ
        /// </summary>
        /// <param name="_main_point">Точка от которой будет поиск</param>
        /// <param name="_objects_array">Список объектов</param>
        /// <param name="_nearest">Будем искать ближайший ? (или дальний)</param>
        /// <returns></returns>
        public static T Find_by_distance_object_XZ<T>(Vector3 _main_point, T[] _objects_array, bool _nearest)//Найти ближайший объект или дальний по отношению к заданному (основному) по координатам XZ
        {
            T result = _objects_array[0];

            Vector3 result_pos = T_thing_convert_vector3(result);

            float distance_active = Vector3.Distance(new Vector3(_main_point.x, 0, _main_point.z), new Vector3(result_pos.x, 0, result_pos.z));

            for (int x = 0; x < _objects_array.Length; x++)
            {
                Vector3 transform_current = T_thing_convert_vector3(_objects_array[x]);

                float dist = Vector3.Distance(new Vector3(_main_point.x, 0, _main_point.z), new Vector3(transform_current.x, 0, transform_current.z));

                if (_nearest)
                {
                    if (distance_active > dist)
                    {
                        distance_active = dist;
                        result = _objects_array[x];
                    }
                }
                else
                {
                    if (distance_active < dist)
                    {
                        distance_active = dist;
                        result = _objects_array[x];
                    }
                }

            }

            return result;
        }

        public static T Find_by_distance_object_XZ<T>(Vector3 _main_object, List<T> _objects_list, bool _nearest)
        {
            T[] array = Convert_from_List_to_Array(_objects_list);

            return Find_by_distance_object_XZ(_main_object, array, _nearest);
        }
        */


        /// <summary>
        /// Преобразовать из Листа в Список
        /// </summary>
        /// <typeparam name="T">Получаемый список</typeparam>
        /// <param name="_list">Лист который будет преобразован</param>
        /// <returns></returns>
        public static T[] Convert_from_List_to_Array<T>(List<T> _list)
        {
            T[] result = new T[_list.Count];

            for (int x = 0; x < _list.Count; x++)
            {
                result[x] = _list[x];
            }

            return result;
        }


        /// <summary>
        /// Инвертировать массив/лист так, что бы его ячейки были изменены в обратном направление
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_list"></param>
        /// <returns></returns>
        public static T[] Invert_sequence_Array<T>(T[] _array)
        {
            T[] result = new T[_array.Length];

            for (int x = _array.Length - 1; x > -1; x--)
            {
                result[_array.Length - 1 - x] = _array[x];
            }

            return result;
        }


        public static T[] Invert_Array<T>(List<T> _list)
        {
            T[] result = new T[_list.Count];

            for (int x = 0; x < _list.Count; x++)
            {
                result[x] = _list[x];
            }

            return result;
        }

        /// <summary>
        /// Преобразовать из Списка в Лист
        /// </summary>
        /// <typeparam name="T">Получаемый лист</typeparam>
        /// <param name="_array">Список который будет преобразован</param>
        /// <returns></returns>
        public static List<T> Convert_from_Array_to_List<T>(T[] _array)
        {
            List<T> result = new List<T>();

            for (int x = 0; x < _array.Length; x++)
            {
                result.Add(_array[x]);
            }

            return result;
        }

        /// <summary>
        /// Узнать есть ли между объектами препятствия
        /// </summary>
        /// <param name="_main_object">Основной объект</param>
        /// <param name="_target">Цель</param>
        /// <returns></returns>
        public static bool Checking_obstacles_between_two_objects(Transform _main_object, Transform _target)
        {
            bool result = false;

            if (Physics.Linecast(_main_object.position, _target.position))
            {
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Узнать есть ли между объектами препятствия с учётом слоя(маски) препятсвия
        /// </summary>
        /// <param name="_main_object">Основной объект</param>
        /// <param name="_target">Цель</param>
        /// <param name="_layer">Слой(маска)</param>
        /// <returns></returns>
        public static bool Checking_obstacles_between_two_objects(Transform _main_object, Transform _target, LayerMask _layer)
        {
            bool result = false;

            if (Physics.Linecast(_main_object.position, _target.position, 1 << _layer))
            {
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Перевести целое число в процент от 0 до 100
        /// </summary>
        /// <param name="_value">Число</param>
        /// <returns></returns>
        public static float Integer_convert_to_percentage(float _value)
        {
            return _value / 100;
        }


        /// <summary>
        /// Узнать направление от точки к точке в 3D
        /// </summary>
        /// <param name="_point_start">Стартовая точка</param>
        /// <param name="_point_end">Конечная точка</param>
        /// <param name="_normalize">Нужно ли нормализовать направление</param>
        /// <returns></returns>
        public static Vector3 Find_out_Direction_3D(Vector3 _point_start, Vector3 _point_end, bool _normalize)
        {
            Vector3 result = Vector3.zero;

            result = _point_end - _point_start;

            if (_normalize)
                result.Normalize();

            return result;
        }



        /// <summary>
        /// Узнать направление от точки к точке в 3D
        /// </summary>
        /// <param name="_point_start">Стартовый объект</param>
        /// <param name="_point_end">Конечный объект</param>
        /// <param name="_normalize">Нужно ли нормализовать направление</param>
        /// <returns></returns>
        public static Vector3 Find_out_Direction_3D(Transform _point_start, Transform _point_end, bool _normalize)
        {
            Vector3 result = Find_out_Direction_3D(_point_start.position, _point_end.position, _normalize);

            return result;
        }



        /// <summary>
        /// Узнать направление от точки к точке в 2D
        /// </summary>
        /// <param name="_point_start">Стартовая точка</param>
        /// <param name="_point_end">Конечная точка</param>
        /// <param name="_normalize">Нужно ли нормализовать направление</param>
        /// <returns></returns>
        public static Vector2 Find_out_Direction_2D(Vector2 _point_start, Vector2 _point_end, bool _normalize)
        {
            Vector2 result = Vector2.zero;

            result = _point_end - _point_start;

            if (_normalize)
                result.Normalize();

            return result;
        }


        /// <summary>
        /// Узнать направление от точки к точке в 2D
        /// </summary>
        /// <param name="_point_start">Стартовый объект</param>
        /// <param name="_point_end">Конечный объект</param>
        /// <param name="_normalize">Нужно ли нормализовать направление</param>
        /// <returns></returns>
        public static Vector2 Find_out_Direction_2D(Transform _point_start, Transform _point_end, bool _normalize)
        {
            Vector2 result = Find_out_Direction_2D(_point_start.position, _point_end.position, _normalize);

            return result;
        }


    }
}