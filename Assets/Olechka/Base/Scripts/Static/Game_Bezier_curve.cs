//Создание кривой безье
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

namespace Olechka
{
    public static class Game_Bezier_curve
    {

        /// <summary>
        /// Узнать точку на кривой из 3 точек
        /// </summary>
        /// <param name="_point_start">Стартовая точка</param>
        /// <param name="_point_1">Средняя точка</param>
        /// <param name="_point_end">Последняя точка</param>
        /// <param name="_step">Шаг (промежуток кривой)</param>
        /// <returns></returns>
        public static Vector3 Get_point_Bezier(Vector3 _point_start, Vector3 _point_1, Vector3 _point_end, float _step)
        {
            Vector3 part_1_1 = Vector3.Lerp(_point_start, _point_1, _step);
            Vector3 part_1_2 = Vector3.Lerp(_point_1, _point_end, _step);

            Vector3 part_2_1 = Vector3.Lerp(part_1_1, part_1_2, _step);

            return part_2_1;
        }


        /// <summary>
        /// Получить точки  по кривой Безье
        /// </summary>
        /// <param name="_point_start">Начальная точка</param>
        /// <param name="_point_1">Точка оттягивающая кривую</param>
        /// <param name="_point_end">Конечная точка</param>
        /// <param name="_number_steps">На сколько точек будет подразделена кривая</param>
        /// <returns></returns>
        public static Vector3[] Get_Points_from_Bezier(Vector3 _point_start, Vector3 _point_1, Vector3 _point_end, int _number_steps)
        {
            Vector3[] result = new Vector3[_number_steps];

            for (int x = 0; x < _number_steps; x++)
            {
                float step_Bezier = (float)x / (_number_steps - 1);

                Vector3 pos = Get_point_Bezier(_point_start, _point_1, _point_end, step_Bezier);
                result[x] = pos;
            }

            return result;
        }


        /// <summary>
        /// Узнать точку на кривой из 4 точек
        /// </summary>
        /// <param name="_point_start">Стартовая точка</param>
        /// <param name="_point_1">1 средняя точка</param>
        /// <param name="_point_2">2 средняя точка</param>
        /// <param name="_point_end">Последняя точка</param>
        /// <param name="_step">Шаг (промежуток кривой)</param>
        /// <returns></returns>
        public static Vector3 Get_point_Bezier(Vector3 _point_start, Vector3 _point_1, Vector3 _point_2, Vector3 _point_end, float _step)
        {
            Vector3 part_1_1 = Vector3.Lerp(_point_start, _point_1, _step);
            Vector3 part_1_2 = Vector3.Lerp(_point_1, _point_2, _step);
            Vector3 part_1_3 = Vector3.Lerp(_point_2, _point_end, _step);

            Vector3 part_2_1 = Vector3.Lerp(part_1_1, part_1_2, _step);
            Vector3 part_2_2 = Vector3.Lerp(part_1_2, part_1_3, _step);

            Vector3 part_3_1 = Vector3.Lerp(part_2_1, part_2_2, _step);

            return part_3_1;
        }
        /*
        /// <summary>
        /// Узнать точку на кривой из массива точек (от 4 точек)
        /// </summary>
        /// <param name="_point_array">Список точек</param>
        /// <param name="_step">Шаг (промежуток кривой)</param>
        /// <returns></returns>
        public static Vector3 Get_point_Bezier(Vector3[] _point_array, float _step)
        {
            float[] roundingforce = new float[_point_array.Length - 1];

            for (int x = 0; x < _point_array.Length - 1; x++)
            {
                roundingforce[x] = 1;
            }

            return Get_point_Bezier(_point_array, roundingforce, _step);
        }
        */

        /// <summary>
        /// Узнать точку на кривой из массива точек (от 4 точек)
        /// </summary>
        /// <param name="_point_array">Список точек</param>
        /// <param name="_point_array">Сила закругления (если указать 0, то путь к следующей точке будет прямой)</param>
        /// <param name="_step">Шаг (промежуток кривой)</param>
        /// <returns></returns>
        public static Vector3 Get_point_Bezier(Vector3[] _point_array, float _step)
        {
            if (_point_array.Length < 3)
            {
                Debug.LogError("Количество точек должно быть больше 2-х!");
                return Vector3.zero;
            }

            /*
            if(_rounding_force.Length < _point_array.Length - 1)
            {
                Debug.LogError("Массив уточняющий скривления меньше, чем самих точек");
                return Vector3.zero;
            }
            */

            //Создаём основной размер этапов
            Vector3[][] part_pos_array = new Vector3[_point_array.Length][];

            //Создаём количество подэтапов в каждом этапе
            for (int x = 0; x < part_pos_array.Length; x++)
            {
                part_pos_array[x] = new Vector3[_point_array.Length - x];
            }

            //Заполняем первый этап с данными точками (позициями)
            for (int y = 0; y <= part_pos_array[0].Length - 1; y++)
            {
                part_pos_array[0][y] = _point_array[y];
            }

            //Debug.Log("Начало");
            //Debug.Log("Количество этапов " + _point_array.Length);

            //Вычисляем
            for (int x = 1; x  <= _point_array.Length - 1; x++)
            {
                //Debug.Log("Количество подэтапов " + part_pos_array[x].Length);
                for (int y = 0; y <= part_pos_array[x].Length - 1; y++)
                {
                    //float step = x == 1 ? math.clamp(0f, 1f, _step - (1 - _rounding_force[y])) : _step;

                    part_pos_array[x][y] = Vector3.Lerp(part_pos_array[x - 1][y], part_pos_array[x - 1][y + 1], _step);
                }
            }
            /*
            Debug.Log("Конец");
            Vector3 part_1_1 = Vector3.Lerp(_point_array[0], _point_array[1], _step);
            Vector3 part_1_2 = Vector3.Lerp(_point_array[1], _point_array[2], _step);
            Vector3 part_1_3 = Vector3.Lerp(_point_array[2], _point_array[3], _step);
            Vector3 part_1_4 = Vector3.Lerp(_point_array[3], _point_array[4], _step);

            Vector3 part_2_1 = Vector3.Lerp(part_1_1, part_1_2, _step);
            Vector3 part_2_2 = Vector3.Lerp(part_1_2, part_1_3, _step);
            Vector3 part_2_3 = Vector3.Lerp(part_1_3, part_1_4, _step);

            Vector3 part_3_1 = Vector3.Lerp(part_2_1, part_2_2, _step);
            Vector3 part_3_2 = Vector3.Lerp(part_2_2, part_2_3, _step);

            Vector3 part_4_1 = Vector3.Lerp(part_3_1, part_3_2, _step);
            */

            return part_pos_array[_point_array.Length - 1][0];
        }


    }
}