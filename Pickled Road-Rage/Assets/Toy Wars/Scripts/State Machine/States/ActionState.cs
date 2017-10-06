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
    // initialization.
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
    //
    // Param:
    //		sMachine: a reference to the state machine.
    //--------------------------------------------------------------------------------------
    public override void OnEnter()
    {
        TurnManager.m_fTimer = TurnManager.m_sfStaticTimerLength;
    }

    //--------------------------------------------------------------------------------------
    // onExit: A virtual function that runs on state exit.
    //
    // Param:
    //		sMachine: a reference to the state machine.
    //--------------------------------------------------------------------------------------
    public override void OnExit()
    {
        TurnManager.m_fTimer = 0;
    }
}
