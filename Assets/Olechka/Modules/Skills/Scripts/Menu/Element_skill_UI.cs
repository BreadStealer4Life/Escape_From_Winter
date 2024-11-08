using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;
using UnityEngine.Events;

namespace Olechka
{
    public class Element_skill_UI : MonoBehaviour
    {
        [Tooltip("Показатель заряда")]
        [SerializeField]
        Image Value = null;

        [Tooltip("Можно ли применить способность")]
        [SerializeField]
        Image Active_image = null;

        [Tooltip("Показатель накопленных зарядов")]
        [SerializeField]
        TextMeshProUGUI Save_value_text = null;

        [Tooltip("Показать на какую кнопку активируется")]
        [SerializeField]
        TextMeshProUGUI Input_text = null;

        [Foldout("Ивенты")]
        [Tooltip("Ивент сколько накопилось зарядки (от 0 до 1)")]
        [SerializeField]
        UnityEvent<float> Value_event = new UnityEvent<float>();

        [Foldout("Ивенты")]
        [Tooltip("Ивент показывающий, что скил можно использовать или нет")]
        [SerializeField]
        UnityEvent<bool> Active_event = new UnityEvent<bool>();

        [Foldout("Ивенты")]
        [Tooltip("Ивент сколько накопилось полных зарядов")]
        [SerializeField]
        UnityEvent<string> Save_value_event = new UnityEvent<string>();

        [Foldout("Ивенты")]
        [Tooltip("Ивент показывающий на какую кнопку нажимать")]
        [SerializeField]
        UnityEvent<string> Input_name_event = new UnityEvent<string>();

        public void Change_charge(float _value)
        {
            if(Value)
                Value.fillAmount = _value;

            Value_event.Invoke(_value);
        }

        public void Active_change(bool _active)
        {
            if (Active_image) 
            {
                if (_active)
                {
                    Active_image.color = Color.green;
                }
                else
                {
                    Active_image.color = Color.red;
                }
            }

            Active_event.Invoke(_active);
        }

        public void Change_save_value(int _value)
        {
            if(Save_value_text)
                Save_value_text.text = _value.ToString();

            Save_value_event.Invoke(_value.ToString());
        }

        public void Change_text_input(string _text)
        {
            if(Input_text)
                Input_text.text = _text;

            Input_name_event.Invoke(_text);
        }

    }
}