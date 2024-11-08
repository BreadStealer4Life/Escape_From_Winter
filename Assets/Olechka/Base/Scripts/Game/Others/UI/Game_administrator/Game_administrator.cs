using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Olechka
{
    public class Game_administrator : Singleton<Game_administrator>
    {
        [Tooltip("Ивент который говорит,что игра начата")]
        [SerializeField]
        public UnityEvent Start_game_event = new UnityEvent();

        [Tooltip("Ивент который говорит,что игра закончена")]
        [SerializeField]
        public UnityEvent End_game_event = new UnityEvent();

        private void Start()
        {
            Invoke(nameof(Start_game), 0.1f);
        }

        public void Start_game()
        {
            Start_game_event.Invoke();
        }

        public void End_game()
        {
            End_game_event.Invoke();

            if(Save_PlayerPrefs.Know_parameter(Type_parameter_bool.New_game))
            Save_PlayerPrefs.Save_parameter(Type_parameter_bool.New_game, false);
        }
    }
}