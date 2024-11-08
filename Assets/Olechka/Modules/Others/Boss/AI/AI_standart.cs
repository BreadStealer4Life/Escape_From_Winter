using Olechka;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Olechka
{
    public class AI_standart : MonoBehaviour
    {

        [Tooltip("Время реакции на игрока")]
        [SerializeField]
        float Reaction_time = 0.8f;

        [Tooltip("Оружие")]
        [SerializeField]
        Gun Gun_enemy = null;

        bool Player_detect_bool = false;

        bool Reaction_bool = false;

        Coroutine Reaction_coroutine = null;

        private void OnEnable()
        {
            Reaction_coroutine = null;
            Reaction_bool = false;
        }

        private void Update()
        {
            if (Player_detect_bool)
                Gun_enemy.Activation();
        }

        IEnumerator Coroutine_reation()
        {
            Reaction_bool = true;

            yield return new WaitForSecondsRealtime(Reaction_time);

            Player_detect_bool = true;

            Reaction_coroutine = null;
        }

        public void Player_detect(bool _change)
        {
            if (_change == true && Reaction_bool == false)
            {
                if (Reaction_coroutine != null)
                    StopCoroutine(Reaction_coroutine);

                StartCoroutine(Coroutine_reation());
            }
            else
            {
                Player_detect_bool = false;
                Reaction_bool = false;
            }
        }
    }
}