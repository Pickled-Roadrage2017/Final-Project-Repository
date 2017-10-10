// Using, etc
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//--------------------------------------------------------------------------------------
// ActionState object. Inheriting from State. State for turn switching timer.
//--------------------------------------------------------------------------------------
public class ActionState : State
{
    // State machine instance
    StateMachine m_sStateMachine;

    //--------------------------------------------------------------------------------------
    // Initialization: Constructor for the State.
    //
    // Param:
    //      sMachine: A reference to the StateMachine.
    //--------------------------------------------------------------------------------------
    public ActionState(StateMachine sMachine)
    {
        // Set the instance of the statemachine.
        m_sStateMachine = sMachine;
    }

    //--------------------------------------------------------------------------------------
    // Update: Function that calls each frame to update game objects.
    //--------------------------------------------------------------------------------------
    public override void OnUpdate()
    {
        // Update the timer by deltatime.
        TurnManager.m_fTimer -= Time.deltaTime;

        // If the timer runs out.
        if (TurnManager.m_fTimer < 0)
        {
            // set the turn to ended.
            TurnManager.m_sbEndTurn = true;

            // Switch players turn.
            TurnManager.SwitchTurn();

            // Push to the delay state.
            m_sStateMachine.ChangeState(ETurnManagerStates.ETURN_DELAY);
        }
    }

    //--------------------------------------------------------------------------------------
    // onEnter: A virtual function that runs first when a state is loaded.
    //--------------------------------------------------------------------------------------
    public override void OnEnter()
    {
        // Reset the timer.
        TurnManager.m_fTimer = TurnManager.m_sfStaticTimerLength;
    }

    //--------------------------------------------------------------------------------------
    // onExit: A virtual function that runs on state exit.
    //--------------------------------------------------------------------------------------
    public override void OnExit()
    {
        // Set timer back to 0.
        TurnManager.m_fTimer = 0;
    }
}
