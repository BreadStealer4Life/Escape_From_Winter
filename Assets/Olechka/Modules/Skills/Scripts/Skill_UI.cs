using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

namespace Olechka
{
    public class Skill_UI : Singleton<Skill_UI>
    {
        //[Tooltip("Префаб показателя скилла")]
        //[SerializeField]
        //Element_skill_UI Element_ui_prefab = null;

        [Tooltip("Показатели зарядки скиллов")]
        [SerializeField]
        UI_element[] Element_array = new UI_element[0];

        public void Preparation(int _elements_number, Input_player _input)
        {
            //Element_array = new UI_element[_elements_number];

            for (int x = 0; x < _elements_number; x++)
            {

                //Element_array[x] = new UI_element();

                //Element_skill_UI element = Instantiate(Element_ui_prefab, transform);

                //Element_array[x].Element = element;

                if(x < _input.Skill_input_array.Length)
                    Element_array[x].Change_text_input(_input.Skill_input_array[x].ToString());
                else
                    Element_array[x].Change_text_input("");
            }

        }

        public void Change_save_value (int _id, int _value)
        {
            Element_array[_id].Change_save_value(_value);
        }

        public void Change_charge(int _id, float _value)
        {
            Element_array[_id].Change_charge(_value);
        }

        public void Active_change(int _id, bool _active)
        {
            Element_array[_id].Active_change(_active);
        }

        [System.Serializable]
        class UI_element
        {

            [Tooltip("Элемент скилла")]
            [SerializeField]
            public Element_skill_UI Element = null;

            public void Change_text_input(string _text)
            {
                Element.Change_text_input(_text);
            }

            public void Active_change (bool _active)
            {
                Element.Active_change(_active);
            }

            public void Change_save_value(int _value)
            {
                Element.Change_save_value(_value);
            }

            public void Change_charge(float _value)
            {
                Element.Change_charge(_value);
            }
        }

    }
}