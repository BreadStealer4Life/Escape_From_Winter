//Скрипт который содержит звуки.
//Вешать на всё, что издаёт звуки.
// Для настройки уровня звука во время игры требуется на сцене Setting_menu
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using NaughtyAttributes;
using UnityEngine.Video;

namespace Olechka
{
    enum Type_sound
    {
        [InspectorName(displayName: ("Это музыка"))]
        Music,
        [InspectorName(displayName: ("Это звук эффектов"))]
        Effect
    }
    /*
    enum Enum_audio
    {
        [InspectorName(displayName: ("Это AudioSource"))]
        AudioSource,
        [InspectorName(displayName: ("Это VideoPlayer"))]
        VideoPlayer
    }
    */
    [AddComponentMenu("Olechka scripts / General / Sound control")]
    public class Sound_control : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler
    {
        [Tooltip("В зависимости от этого будет применятся настройка громкости музыки или звуков")]
        [Header("Тип издаваемых звуков")]
        [SerializeField]
        Type_sound Sound_type = Type_sound.Music;

        //[Header("Тип динамиков")]
        //[SerializeField]
        //Enum_audio Type_audio = Enum_audio.AudioSource;

        //[ShowIf(nameof(Type_audio), Enum_audio.AudioSource)]
        [Header("Компонент динамик")]
        public AudioSource Audioo = null;

        //[ShowIf(nameof(Type_audio), Enum_audio.VideoPlayer)]
        //[Header("Компонент воспроизведение видео")]
        //public VideoPlayer Video_player = null;

        [Foldout("Другие типы проигрывания звуков")]
        [Header("Активность объекта")]
        [Tooltip("Звук который будет проигрыватся при включение объекта")]
        [SerializeField]
        AudioClip Sound_enable = null;

        [Foldout("Другие типы проигрывания звуков")]
        [Tooltip("Будет ли звук зацикленным")]
        [SerializeField]
        bool Loop_enable_bool = false;

        [Foldout("Другие типы проигрывания звуков")]
        [Tooltip("Звук который будет проигрыватся при выключение/уничтожение объекта")]
        [SerializeField]
        AudioClip Sound_disable = null;

        [Foldout("Другие типы проигрывания звуков")]
        [Tooltip("Будет ли звук зацикленным")]
        [SerializeField]
        bool Loop_disable_bool = false;




        [Foldout("Проигрывание звуков для UI")]
        [Header("Если это UI")]
        [Tooltip("Звук который будет проигрыватся при наведение курсора на объект")]
        [SerializeField]
        AudioClip Sound_OnPointerEnter = null;

        [Foldout("Проигрывание звуков для UI")]
        [Tooltip("Будет ли звук зацикленным")]
        [SerializeField]
        bool Loop_OnPointerEnter_bool = false;

        [Foldout("Проигрывание звуков для UI")]
        [Tooltip("Звук который будет проигрыватся при нажимание")]
        [SerializeField]
        AudioClip Sound_OnPointerDown = null;

        [Foldout("Проигрывание звуков для UI")]
        [Tooltip("Будет ли звук зацикленным")]
        [SerializeField]
        bool Loop_OnPointerDown_bool = false;

        [Foldout("Проигрывание звуков для UI")]
        [Tooltip("Звук который будет проигрыватся при отпускание")]
        [SerializeField]
        AudioClip Sound_OnPointerUp = null;

        [Foldout("Проигрывание звуков для UI")]
        [Tooltip("Будет ли звук зацикленным")]
        [SerializeField]
        bool Loop_OnPointerUp_bool = false;

        [Foldout("Проигрывание звуков для UI")]
        [Header("Если это игровой объект")]
        [Tooltip("Звук который будет проигрыватся при клике по этому объекту")]
        [SerializeField]
        AudioClip Sound_click = null;

        [Foldout("Проигрывание звуков для UI")]
        [Tooltip("Будет ли звук зацикленным")]
        [SerializeField]
        bool Loop_click_bool = false;




        [Header("Список для прочих звуков")]
        [SerializeField]
        Sound_class[] Sound_array = new Sound_class[0];

        private float Default_value;

        float Local_volume = 1f;

        float Global_volume = 1f;

        #region Системные методы

        void Awake()
        {
            Default_value = Audioo.volume;
            /*
            switch (Type_audio)
            {
                case Enum_audio.AudioSource:
                    
                    break;

                case Enum_audio.VideoPlayer:
                    Default_value = Video_player.GetDirectAudioVolume;
                    break;
            }
            */
            
        }

        void OnEnable()
        {
            Preparation_sound();

            if (Setting_menu.Singleton_Instance)
            {
                Setting_menu.Singleton_Instance.Global_volume_d += Change_global_value;

                if (Sound_type == Type_sound.Music)
                {
                    Setting_menu.Singleton_Instance.Music_d += Change_value;
                }
                else
                {
                    Setting_menu.Singleton_Instance.Effect_sound_d += Change_value;
                }
            }

            if (Sound_enable != null)
                Play_sound(Sound_enable, Loop_enable_bool);
        }

        private void OnDisable()
        {
            if (Sound_disable != null && Audioo != null)
                Play_sound(Sound_disable, Loop_disable_bool);

            if (Setting_menu.Singleton_Instance)//(FindObjectOfType<Setting_menu>())
            {
                Setting_menu.Singleton_Instance.Global_volume_d -= Change_global_value;

                if (Sound_type == Type_sound.Music)
                    Setting_menu.Singleton_Instance.Music_d -= Change_value;
                else
                    Setting_menu.Singleton_Instance.Effect_sound_d -= Change_value;
            }
        }

        private void OnMouseDown()
        {
            if (Sound_click != null)
                Play_sound(Sound_click, Loop_click_bool);
        }

        public void OnPointerEnter(PointerEventData eventData)//Звук который будет проигрыватся при наведение курсора на объект
        {
            if (Sound_OnPointerEnter != null)
                Play_sound(Sound_OnPointerEnter, Loop_OnPointerEnter_bool);
        }

        public void OnPointerDown(PointerEventData eventData)//Звук который будет проигрыватся при нажимание на кнопку
        {
            if (Sound_OnPointerDown != null)
                Play_sound(Sound_OnPointerDown, Loop_OnPointerDown_bool);
        }

        public void OnPointerUp(PointerEventData eventData)//Звук который будет проигрыватся при отпускание кнопки
        {
            if (Sound_OnPointerUp != null)
                Play_sound(Sound_OnPointerUp, Loop_OnPointerUp_bool);
        }

        #endregion



        #region Методы

        /// <summary>
        /// Настройка звука в начале
        /// </summary>
        void Preparation_sound()
        {
            Audioo.volume = Check_sound_volume(Default_value);

        }


        /// <summary>
        /// Узнать уровень громкости
        /// </summary>
        /// <param name="_add_individual_volume">Дополнительная корректирующая громкость звука</param>
        /// <returns></returns>
        float Check_sound_volume(float _add_individual_volume)
        {
            float result = 0;
                
            float global_volume = Save_PlayerPrefs.Know_parameter(Type_parameter_value_float.Global_volume_sound);

            if (Sound_type == Type_sound.Music)
            result = _add_individual_volume * global_volume * Save_PlayerPrefs.Know_parameter(Type_parameter_value_float.Sound_music);
            else if (Sound_type == Type_sound.Effect)
            result = _add_individual_volume * global_volume * Save_PlayerPrefs.Know_parameter(Type_parameter_value_float.Sound_effect);

            return result;
        }

        /// <summary>
        /// Изменение громкости
        /// </summary>
        /// <param name="_value">Значение громкости</param>
        void Change_value(float _value)
        {
            if (Audioo)
            {
                Local_volume = Default_value * _value;
                Audioo.volume = Global_volume * Local_volume;
            }
        }

        /// <summary>
        /// Изменение громкости
        /// </summary>
        /// <param name="_value">Значение громкости</param>
        void Change_global_value(float _value)
        {
            if (Audioo)
            {
                Global_volume = _value;
                Audioo.volume = Global_volume * Local_volume;
            }
        }

        /// <summary>
        /// Запустить звук
        /// </summary>
        /// <param name="_clip">Клип звука</param>
        /// <param name="_loop">Зациклено?</param>
        void Play_sound(AudioClip _clip, bool _loop)
        {
            if (Audioo)
            {

                Audioo.loop = _loop;
                if (_loop)
                {
                    Audioo.clip = _clip;
                    Audioo.Play();
                }
                else
                {
                    Audioo.PlayOneShot(_clip);
                }
            }
        }

        #endregion


        #region Публичные методы

        /// <summary>
        /// Запустить активный звук
        /// </summary>
        public void Sound_PlayOneShot()
        {
            if (Audioo)
            {
                Audioo.Play();
            }
        }


        /// <summary>
        /// Запустить звук 1 раз по id
        /// </summary>
        /// <param name="_id_sound">Id звука</param>
        public void Sound_PlayOneShot(int _id_sound)
        {
            if (Audioo)
            {
                Audioo.volume = Check_sound_volume(Sound_array[_id_sound].Sound_volume);
                Audioo.PlayOneShot(Sound_array[_id_sound].Sound);
            }
        }

        /// <summary>
        /// Запустить звук 1 раз по имени
        /// </summary>
        /// <param name="_name_sound">Имя звука</param>
        public void Sound_PlayOneShot(string _name_sound)
        {
            if (Audioo)
            {
                for(int x = 0; x < Sound_array.Length; x++)
                {
                    if (Sound_array[x].Name == _name_sound)
                    {
                        Audioo.volume = Check_sound_volume(Sound_array[x].Sound_volume);
                        Audioo.PlayOneShot(Sound_array[x].Sound);
                        break;
                    }
                }

                Debug.LogError("Звука по имени " + _name_sound + " не существует!");
                
            }
        }


        /// <summary>
        /// Включить воспроизведение звука из списка
        /// </summary>
        /// <param name="_id_sound">ID звука из списка</param>
        /// <param name="_loop">Зациклена ?</param>
        /// /// <param name="_oneShot">Воспроизвести сразу?</param>
        public void Sound_Play(int _id_sound, bool _loop, bool _oneShot)
        {
            if (Audioo)
            {
                Audioo.volume = Check_sound_volume(Sound_array[_id_sound].Sound_volume);

                Audioo.loop = _loop;

                if(_oneShot)
                    Audioo.PlayOneShot(Sound_array[_id_sound].Sound);
                else
                {
                    Audioo.clip = Sound_array[_id_sound].Sound;
                    Audioo.Play();
                }
                    
            }
        }

        #endregion


        [System.Serializable]
        protected class Sound_class//Класс содержащий описание и ссылку на сам звук
        {
            [Tooltip("Описание для звука, что бы не путаться")]
            [SerializeField]
            public string Name = "Название звука";

            [Tooltip("Ссылка на звук")]
            [SerializeField]
            public AudioClip Sound = null;

            [Range(0f, 2f)]
            [Tooltip("Индвидуальная громкость звука(если изначальный звук слишком громкий или тихий")]
            [SerializeField]
            public float Sound_volume = 1;
        }

    }
}