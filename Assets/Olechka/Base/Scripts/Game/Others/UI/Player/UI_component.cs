using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Olechka
{
    public class UI_component : MonoBehaviour
    {

        public void End_win()
        {
            UI_administrator.Singleton_Instance.End_win();
        }

        public void End_lose()
        {
            UI_administrator.Singleton_Instance.End_lose();
        }

        public void Change_health(int _change)
        {
            UI_administrator.Singleton_Instance.Change_health(_change);
        }

        public void Activity_UI_Boss(bool _activity)
        {
            UI_administrator.Singleton_Instance.Activity_Boss(_activity);
        }

        public void Change_health_Boss(float _value)
        {
            UI_administrator.Singleton_Instance.Change_health_Boss(_value);
        }

    }
}