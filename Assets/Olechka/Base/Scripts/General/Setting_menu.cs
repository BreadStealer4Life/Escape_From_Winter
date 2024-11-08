//Скрипт настроек и задаёт положение настроек согласно сохранению
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Olechka
{
    public class Setting_menu : Singleton<Setting_menu>
    {
        [Header("Общая настройка звука")]
        [Tooltip("Ползунок общей громкости")]
        [SerializeField]
        private Slider Global_volume_slider = null;

        public delegate void Global_volume_delegate(float _value);//Делегат изменения громкости эффектов
        public Global_volume_delegate Global_volume_d;//Экземпляр делегата изменения громкости эффектов

        [Header("Настройки эффектов")]
        [Tooltip("Ползунок громкости эффектов")]
        [SerializeField]
        private Slider Effect_sound_slider = null;

        public delegate void Effect_sound_delegate(float _value);//Делегат изменения громкости эффектов
        public Effect_sound_delegate Effect_sound_d;//Экземпляр делегата изменения громкости эффектов

        [Header("Настройки музыки")]
        [Tooltip("Ползунок громкости музыки")]
        [SerializeField]
        private Slider Music_slider = null;

        public delegate void Music_delegate(float _value);//Делегат изменения громкости музыки
        public Music_delegate Music_d;//Экземпляр делегата изменения громкости музыки

        [Header("Настройки чувствительности мыши")]
        [Tooltip("Ползунок чувствительности мыши")]
        [SerializeField]
        private Slider Mouse_sensitivity_slider = null;

        public delegate void Mouse_sensitivity_delegate(float _value);//Делегат изменения чувствительности мыши
        public Mouse_sensitivity_delegate Mouse_sensitivity_d;//Экземпляр делегата изменения чувствительности мыши

        [Header("Настройки оповещения")]
        [Tooltip("Галочка оповещения")]
        [SerializeField]
        private Toggle Alert_toggle = null;

        [Header("Настройки оповещения")]
        [Tooltip("Галочка вибрации")]
        [SerializeField]
        private Toggle Vibration_toggle = null;

        [Tooltip("Счётчик FPS")]
        [SerializeField]
        private Toggle FPS_counter_toggle = null;

        [Tooltip("Значение Показателя FPS")]
        [SerializeField]
        private TextMeshProUGUI UI_FPS_counter_value = null;

        [Tooltip("Показатель FPS")]
        [SerializeField]
        private GameObject UI_FPS_counter = null;

        [Header("Выбранный язык")]
        [Tooltip("Список выбора языка")]
        [SerializeField]
        private Dropdown Language_option = null;

        [Header("Выбранное ограничение FPS")]
        [Tooltip("Ограничение FPS")]
        [SerializeField]
        private Dropdown FPS_option = null;


        public delegate void Language_delegate(int _index);//Делегат изменения языка
        public Language_delegate Language_d;//Экземпляр делегата изменения языка

        public delegate void Input_key_delegate();//Делегат изменения клавиш управления
        public Input_key_delegate Input_key_d;//Экземпляр делегата изменения клавиш управления

        public delegate void Reset_delegate();//Делегат сбрасывающий насйтроки
        public Reset_delegate Reset_d;//Экземпляр делегата сбрасывающий насйтроки

        private void Start()
        {
            Preparation();

            InvokeRepeating(nameof(FPS_counter), 1f, 1f);
        }

        private void OnEnable()
        {
            //Game_Player.Cursor_player(true);
        }

        void FPS_counter()
        {
            float fps = (int)(1f / Time.unscaledDeltaTime);
            UI_FPS_counter_value.text = fps.ToString();
        }

        /// <summary>
        /// Подготовка при включение
        /// </summary>
        void Preparation()
        {
            Global_volume_slider.onValueChanged.AddListener(Global_volume_control);

            Global_volume_slider.value = Save_PlayerPrefs.Know_parameter(Type_parameter_value_float.Global_volume_sound);

            Effect_sound_slider.onValueChanged.AddListener(Sound_effect_control);

            Effect_sound_slider.value = Save_PlayerPrefs.Know_parameter(Type_parameter_value_float.Sound_effect);

            Music_slider.onValueChanged.AddListener(Sound_music_control);

            Music_slider.value = Save_PlayerPrefs.Know_parameter(Type_parameter_value_float.Sound_music);

            Mouse_sensitivity_slider.onValueChanged.AddListener(Mouse_sensitivity_control);

            Mouse_sensitivity_slider.value = Save_PlayerPrefs.Know_parameter(Type_parameter_value_float.Mouse_sensitivity);

            if (Language_option)
            {
                Language_option.onValueChanged.AddListener(Language_control);

                Language_option.value = Save_PlayerPrefs.Know_parameter(Type_parameter_value_int.Language);
            }

            if (FPS_option)
            {
                FPS_option.onValueChanged.AddListener(FPS_control);

                int index = Save_PlayerPrefs.Know_parameter(Type_parameter_value_int.FPS_limit);

                FPS_option.value = index;

                FPS_control(index);
            }


            if (Alert_toggle)
            {
                Alert_toggle.onValueChanged.AddListener(Alert_control);

                Alert_toggle.isOn = Save_PlayerPrefs.Know_parameter(Type_parameter_bool.Alert_bool);
            }
                

            if (Vibration_toggle)
            {
                Vibration_toggle.onValueChanged.AddListener(Vibration_control);

                Vibration_toggle.isOn = Save_PlayerPrefs.Know_parameter(Type_parameter_bool.Vibration_bool);
            }

            if (FPS_counter_toggle)
            {
                FPS_counter_toggle.onValueChanged.AddListener(FPS_counter_control);

                FPS_counter_toggle.isOn = Save_PlayerPrefs.Know_parameter(Type_parameter_bool.FPS_counter_bool);

                FPS_counter_control(FPS_counter_toggle.isOn);
            }



        }



        /// <summary>
        /// Вернуть настройки в изначальное состояние
        /// </summary>
        public void Reset_setting()
        {
            Mouse_sensitivity_control(1);
            Sound_music_control(1);
            Sound_effect_control(1);
            Alert_control(true);
            Vibration_control(true);
            FPS_counter_control(false);
            Language_control(0);
            FPS_control(0);

            Preparation();

            Reset_d?.Invoke();
        }


        /// <summary>
        /// Изменение клавиш управления
        /// </summary>
        public void Input_key_control()
        {
            Input_key_d?.Invoke();
        }



        /// <summary>
        /// Изменение языка
        /// </summary>
        /// <param name="_index">Индекс</param>
        public void Language_control(int _index)
        {
            Save_PlayerPrefs.Save_parameter(Type_parameter_value_int.Language, _index);

            Language_d?.Invoke(_index);
        }

        /// <summary>
        /// Изменение ограничения FPS
        /// </summary>
        /// <param name="_index">Индекс</param>
        public void FPS_control(int _index)
        {
            Save_PlayerPrefs.Save_parameter(Type_parameter_value_int.FPS_limit, _index);

            Language_d?.Invoke(_index);

            int result = 0;

            switch (_index)
            {
                case 0:
                    result = 0;
                break;

                case 1:
                    result = 200;
                    break;

                case 2:
                    result = 120;
                    break;

                case 3:
                    result = 60;
                    break;

                case 4:
                    result = 30;
                    break;

                case 5:
                    result = 24;
                    break;

                case 6:
                    result = 12;
                    break;
            }

            Application.targetFrameRate = result;
        }


        /// <summary>
        /// Изменение общегор звука
        /// </summary>
        /// <param name="_value">Сила звука</param>
        public void Global_volume_control(float _value)
        {
            Save_PlayerPrefs.Save_parameter(Type_parameter_value_float.Global_volume_sound, _value);

            Global_volume_d?.Invoke(_value);
        }


        /// <summary>
        /// Изменение звука эффектов
        /// </summary>
        /// <param name="_value">Сила звука</param>
        public void Sound_effect_control(float _value)
        {
            Save_PlayerPrefs.Save_parameter(Type_parameter_value_float.Sound_effect, _value);

            Effect_sound_d?.Invoke(_value);
        }

        /// <summary>
        /// Изменение чувствительности мыши
        /// </summary>
        /// <param name="_value">Чувствительность</param>
        public void Mouse_sensitivity_control(float _value)
        {
            Save_PlayerPrefs.Save_parameter(Type_parameter_value_float.Mouse_sensitivity, _value);

            Mouse_sensitivity_d?.Invoke(_value);
        }

        /// <summary>
        /// Изменение звука музыки
        /// </summary>
        /// <param name="_value">Сила звука</param>
        public void Sound_music_control(float _value)
        {
            Save_PlayerPrefs.Save_parameter(Type_parameter_value_float.Sound_music, _value);

            if (Music_d != null)
                Music_d?.Invoke(_value);
        }


        /// <summary>
        /// Изменение оповещения
        /// </summary>
        /// <param name="_bool">Включить или отключить</param>
        public void Alert_control(bool _bool)
        {
            Save_PlayerPrefs.Save_parameter(Type_parameter_bool.Alert_bool, _bool);
        }


        /// <summary>
        /// Изменение вибрации
        /// </summary>
        /// <param name="_bool">Включить или отключить</param>
        public void Vibration_control(bool _bool)
        {
            Save_PlayerPrefs.Save_parameter(Type_parameter_bool.Vibration_bool, _bool);
        }


        /// <summary>
        /// Изменение отображение счётчика FPS
        /// </summary>
        /// <param name="_bool">Включить или отключить</param>
        public void FPS_counter_control(bool _bool)
        {
            Save_PlayerPrefs.Save_parameter(Type_parameter_bool.FPS_counter_bool, _bool);

            UI_FPS_counter.SetActive(_bool);
        }

    }
}