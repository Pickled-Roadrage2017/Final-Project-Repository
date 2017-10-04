// Using, etc
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//--------------------------------------------------------------------------------------
// ActionState object. Inheriting from State. // FINISH THIS COMMENT
//--------------------------------------------------------------------------------------
public class ActionState : State
{
    //--------------------------------------------------------------------------------------
    // initialization.
    //--------------------------------------------------------------------------------------
    public ActionState()
    {

    }

    //--------------------------------------------------------------------------------------
    // Update: Function that calls each frame to update game objects.
    //--------------------------------------------------------------------------------------
    public override void OnUpdate(StateMachine sMachine)
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
            sMachine.ChangeState(ETurnManagerStates.ETURN_DELAY);
        }
    }

    //--------------------------------------------------------------------------------------
    // onEnter: A virtual function that runs first when a state is loaded.
    //
    // Param:
    //		sMachine: a reference to the state machine.
    //--------------------------------------------------------------------------------------
    public override void OnEnter(StateMachine sMachine)
    {
        TurnManager.m_fTimer = TurnManager.m_sfStaticTimerLength;
    }

    //--------------------------------------------------------------------------------------
    // onExit: A virtual function that runs on state exit.
    //
    // Param:
    //		sMachine: a reference to the state machine.
    //--------------------------------------------------------------------------------------
    public override void OnExit(StateMachine sMachine)
    {
        TurnManager.m_fTimer = 0;
    }
}
