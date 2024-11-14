using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace Olechka
{
    [AddComponentMenu("Olechka scripts / Game / Logic / Change material color")]
    [DisallowMultipleComponent]
    public class Change_material_color : MonoBehaviour
    {

        [Tooltip("Материал объекта")]
        [SerializeField]
        Renderer[] Material_obj = new Renderer[0];

        [Tooltip("Номер материала")]
        [SerializeField]
        int Id_material = 0;

        [Tooltip("Массив цветов")]
        [SerializeField]
        //[ColorUsage(true, false)]
        Color[] Color_array = new Color[0];

        [Tooltip("Сразу запускает цвет материала 0")]
        [SerializeField]
        bool Start_zero_color_bool = false;

        [Tooltip("Менять силу свечения?")]
        [SerializeField]
        bool Emission_bool = false;

        [field: ShowIf(nameof(Emission_bool))]
        [field: Tooltip("Сила свечения")]
        [field: SerializeField]
        float Emission_identity = 10f;

        Color[] Default_color = new Color[0];

        int Active_id_color = 0;

        private void Start()
        {
            if(Material_obj != null)
            {
                Default_color = new Color[Material_obj.Length];

                for (int i = 0; i < Material_obj.Length; i++)
                {
                    Default_color[i] = Material_obj[i].materials[Id_material].color;
                }
            }
                
            

            if (Start_zero_color_bool)
                Change_color(0);
        }

        public void Reset_color()
        {
            if (Material_obj != null)
            {

                for (int i = 0; i < Material_obj.Length; i++)
                {
                    Material_obj[i].materials[Id_material].color = Default_color[i];

                    if (Emission_bool)
                        Material_obj[i].materials[Id_material].SetColor("_EmissionColor", Default_color[i] * Emission_identity);
                }
            }
                
        }

        public void Next_color()
        {
            Active_id_color++;

            if (Active_id_color > Color_array.Length - 1)
            {
                Active_id_color = 0;
            }

            Change_color(Active_id_color);
        }

        public void Change_color(int _id_color)
        {
            if (Material_obj != null && _id_color >= 0 && _id_color < Color_array.Length)
            {
                Active_id_color = _id_color;

                for (int i = 0; i < Material_obj.Length; i++)
                {
                    Material_obj[i].materials[Id_material].color = Color_array[_id_color];

                    if (Emission_bool)
                        Material_obj[i].materials[Id_material].SetColor("_EmissionColor", Color_array[_id_color] * Emission_identity);
                }
            }
        }

    }
}