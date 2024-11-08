using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace Olechka
{
    public class UI_administrator : Singleton<UI_administrator>
    {
        [Tooltip("Меню UI")]
        [SerializeField]
        GameObject Menu = null;

        [Tooltip("Меню победа")]
        [SerializeField]
        GameObject End_win_obj = null;

        [Tooltip("Меню проигрыша")]
        [SerializeField]
        GameObject End_lose_obj = null;

        [Tooltip("Картинка полоски здоровья")]
        [SerializeField]
        Image[] Healht_image_array = null;

        [Tooltip("Ивент конца")]
        [SerializeField]
        UnityEvent End_event = new UnityEvent();





        [Space(20)]
        [Header("Босс")]

        [Tooltip("Интерфейс при битве с Боссом")]
        [SerializeField]
        GameObject Boss_obj = null;

        [Tooltip("Здоровье Босса")]
        [SerializeField]
        Image Image_health_Boss = null;

        bool Menu_bool = false;

        private void Update()
        {
            

            if(Input.GetKeyDown(KeyCode.Escape))
            {
                Menu_bool = !Menu_bool;
                Menu.SetActive (Menu_bool);
            }
        }

        public void End_win()
        {
            End_win_obj.SetActive(true);

            End_event.Invoke();
        }

        public void End_lose()
        {
            End_lose_obj.SetActive(true);

            End_event.Invoke();
        }

        public void Activity_Boss(bool _active)
        {
            Boss_obj.SetActive(_active);
        }

        public void Change_health_Boss(float _value)
        {
            Image_health_Boss.fillAmount = _value;
        }

        public void Change_health(int _change)
        {
            for (int x = 0; x < Healht_image_array.Length; x++)
            {

                if (x <= _change - 1)
                {
                    Healht_image_array[x].gameObject.SetActive(true);
                }
                else
                {
                    Healht_image_array[x].gameObject.SetActive(false);
                }
            }
        }
    }
}