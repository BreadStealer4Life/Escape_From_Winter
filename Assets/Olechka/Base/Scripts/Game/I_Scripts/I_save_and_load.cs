using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Olechka
{
    public interface I_save_and_load
    {
       
        public void Preparation_save_and_load()
        {
            if (Game_administrator.Singleton_Instance)
            {
                Game_administrator.Singleton_Instance.Start_game_event.AddListener(Load);

                Game_administrator.Singleton_Instance.End_game_event.AddListener(Save);
            }
            else
            {
                Debug.LogError("На сцене нету Game_administrator, нужно исправить и добавить на сцену!");
            }
        }

        public abstract void Save();

        public abstract void Load();
    }
}