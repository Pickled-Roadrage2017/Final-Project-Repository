//--------------------------------------------------------------------------------------
// Purpose: The state of action between player turns.
//
// Description: The ActionState script is gonna be used for when the player turn is in an
// action state. This action state happens during the normal turn timer.
//
// Author: Thomas Wiltshire.
//--------------------------------------------------------------------------------------

// Using, etc
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//--------------------------------------------------------------------------------------
// ActionState object. Inheriting from State. State for turn switching timer.
//--------------------------------------------------------------------------------------
public class ActionState : State
{
    //--------------------------------------------------------------------------------------
    // Initialization: Constructor for the State.
    //
    // Param:
    //      sMachine: A reference to the StateMachine.
    //--------------------------------------------------------------------------------------
    public ActionState(StateMachine sMachine) : base(sMachine)
    {
    }

    //--------------------------------------------------------------------------------------
    // Update: Function that calls each frame to update game objects.
    //--------------------------------------------------------------------------------------
    public override void OnUpdate()
    {
        // Update the timer by deltatime.
        TurnManager.m_sfTimer -= Time.deltaTime;

        // Once the timer goes below 6
        if (TurnManager.m_sfTimer < 6)
        {
            // make sure only the audio only plays once.
            if (!m_tTurnManager.m_asAudioSource.isPlaying && !PauseManager.m_sbPaused)
            {
                // Play start time wanring sound.
                m_tTurnManager.m_asAudioSource.PlayOneShot(m_tTurnManager.m_acTimeWarningAudio);
            }
        }

        // If the timer runs out or end turn is true.
        if (TurnManager.m_sfTimer < 0 || TurnManager.m_sbEndTurn == true)
        {
            // Push to the endturn state.
            m_sStateMachine.ChangeState(ETurnManagerStates.ETURN_END);
        }
    }

    //--------------------------------------------------------------------------------------
    // onEnter: A virtual function that runs first when a state is loaded.
    //--------------------------------------------------------------------------------------
    public override void OnEnter()
    {
        // Reset the timer.
        TurnManager.m_sfTimer = TurnManager.m_sfStaticTimerLength;
    }

    //--------------------------------------------------------------------------------------
    // onExit: A virtual function that runs on state exit.
    //--------------------------------------------------------------------------------------
    public override void OnExit()
    {
        // Set timer back to 0.
        TurnManager.m_sfTimer = 0;

        // make sure the time warning sound has stop.
        m_tTurnManager.m_asAudioSource.Stop();
    }
}
