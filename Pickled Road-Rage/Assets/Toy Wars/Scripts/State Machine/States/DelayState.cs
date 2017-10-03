// Using, etc
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//--------------------------------------------------------------------------------------
// State object. Inheriting from State. // FINISH THIS COMMENT
//--------------------------------------------------------------------------------------
public class DelayState : State
{
    // float for turn delay.
    public float m_fDelayTimer;

    //--------------------------------------------------------------------------------------
    // initialization.
    //--------------------------------------------------------------------------------------
    public DelayState()
    {

    }

    //--------------------------------------------------------------------------------------
    // Update: Function that calls each frame to update game objects.
    //--------------------------------------------------------------------------------------
    public override void OnUpdate(StateMachine sMachine)
    {
        // Update the timer by deltatime.
        m_fDelayTimer -= Time.deltaTime;

        // Once the timer ends.
        if (m_fDelayTimer < 0)
        {
            // Push to the action state
            sMachine.ChangeState(ETurnManagerStates.ETURN_ACTION);
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
        // Reset the timer.
        m_fDelayTimer = TurnManager.m_sfStaticDelayLength;
    }

    //--------------------------------------------------------------------------------------
    // onExit: A virtual function that runs on state exit.
    //
    // Param:
    //		sMachine: a reference to the state machine.
    //--------------------------------------------------------------------------------------
    public override void OnExit(StateMachine sMachine)
    {
        // Set the delay back to 0
        m_fDelayTimer = 0;
    }
}
