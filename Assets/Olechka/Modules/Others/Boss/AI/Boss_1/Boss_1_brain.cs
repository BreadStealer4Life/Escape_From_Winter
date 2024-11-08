using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Olechka
{
    public class Boss_1_brain : Brain_SM_AI_abstract
    {
        #region Variables

        [Tooltip("Время через которое применит атаку")]
        [SerializeField]
        Vector2 Time_change_state = new Vector2(1f,5f);

        [Tooltip("Фазы Босса")]
        [SerializeField]
        Phase_Boss[] Phase_array = new Phase_Boss[0];

        int Active_phase = 0;

        Coroutine Random_state_coroutine = null;

  #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////

        #region MonoBehaviour Methods

  protected override void Start()
  {
      base.Start();
  }

  protected override void Update()
  {
      base.Update();
  }

  protected override void FixedUpdate()
  {
      base.FixedUpdate();
  }

  #endregion

  ///////////////////////////////////////////////////////////////////////////////////////////////////

  #region Core Methods

        /*
  protected override void Initialized_State_machine()
  {
      base.Initialized_State_machine();
  }
*/
  protected override void Slow_update()
  {
      base.Slow_update();
  }

  protected override void Slow_update_2()
  {
      base.Slow_update_2();
  }



  #endregion

  ///////////////////////////////////////////////////////////////////////////////////////////////////

  #region Methods
        IEnumerator Coroutine_random_state()
        {
            float time = UnityEngine.Random.Range(Time_change_state.x, Time_change_state.y);

            int id_state = UnityEngine.Random.Range(0, Active_phase + 1);

            yield return new WaitForSecondsRealtime(time);

            Change_state(id_state);

            Random_state_coroutine = null;
        }

        public void Preparation_phase()
        {
            int next_phase = Active_phase + 1;

            if (next_phase > State_array.Length - 1)
                next_phase = State_array.Length - 1;

            if(next_phase > Phase_array.Length)
                next_phase = Phase_array.Length;

            
            if (next_phase > Active_phase && math.round((float)Main_character.Health_script.Health_max * (float)Phase_array[Active_phase].Percent_health_phase / 100) >= Main_character.Health_script.Health_active)
            {
                Time_change_state = Phase_array[Active_phase].Time_change_state;
                Phase_array[Active_phase].Start_phase();
                Active_phase = next_phase;
            }


        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////

        #region Public Methods
        public override void Activation()
        {
            base.Activation();

            StartCoroutine(Coroutine_random_state());
        }

        public override void Off_state()
        {
            base.Off_state();


            if(Random_state_coroutine == null)
            Random_state_coroutine = StartCoroutine(Coroutine_random_state());
        }

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////

        #region Additionally

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////

        #region Gizmos

        #endregion

        ///////////////////////////////////////////////////////////////////////////////////////////////////


        [System.Serializable]
        class Phase_Boss
        {
            [field: Tooltip("До какого состояния нужно довести, что бы началась эта фаза")]
            [field: SerializeField]
            public int Percent_health_phase { get; private set; } = 50;

            [Tooltip("Ивент начала фазы")]
            [SerializeField]
            UnityEvent Start_phase_event = new UnityEvent();

            [Tooltip("Время через которое применит атаку")]
            [SerializeField]
            public Vector2 Time_change_state = new Vector2(1f, 5f);

            public void Start_phase()
            {
                Start_phase_event.Invoke();
            }
        }
    }
}