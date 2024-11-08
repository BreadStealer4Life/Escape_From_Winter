using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Olechka
{
    public class Game_component : MonoBehaviour
    {

        public void Start_game_active()
        {
            Game_administrator.Singleton_Instance.Start_game();
        }

        public void End_game_active()
        {
            Game_administrator.Singleton_Instance.End_game();
        }

    }
}
