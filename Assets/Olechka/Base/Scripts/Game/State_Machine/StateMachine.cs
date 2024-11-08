namespace Olechka
{
    public class StateMachine
    {
        public State_abstract Current_State { get; private set; }



        /// <summary>
        /// Сменить состояние на другоие
        /// </summary>
        /// <param name="new_State">новое состояние</param>
        public void Change_State(State_abstract new_State)
        {
            if(Current_State != null)
            Current_State.Exit_state();

            Current_State = new_State;

            if (new_State != null)
            {
                new_State.Enter_state();
            }

        }

        /// <summary>
        /// Узнать какое текущее состояние
        /// </summary>
        public State_abstract Find_out_Current_State
        {
            get
            {
                return Current_State;
            }
        }

    }
}