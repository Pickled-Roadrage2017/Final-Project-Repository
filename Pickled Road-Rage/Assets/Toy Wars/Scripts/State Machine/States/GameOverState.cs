//--------------------------------------------------------------------------------------
// Purpose: The state for when the game has eneded.
//
// Description: The DelayState script is gonna be used for when the player turn is in a
// delay state. This delay state happens during the delay timer. // REWIRTE COMMENT!
//
// Author: Thomas Wiltshire.
//--------------------------------------------------------------------------------------

// Using, etc
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//--------------------------------------------------------------------------------------
// DelayState object. Inheriting from State. Delays the turn switching.
//--------------------------------------------------------------------------------------
public class GameOverState : State
{
    // State machine instance
    StateMachine m_sStateMachine;

    // Turn manager instance
    TurnManager m_tTurnManager;

    //--------------------------------------------------------------------------------------
    // Initialization: Constructor for the State.
    //
    // Param:
    //      sMachine: A reference to the StateMachine.
    //--------------------------------------------------------------------------------------
    public GameOverState(StateMachine sMachine)
    {
        // Set the instance of the statemachine.
        m_sStateMachine = sMachine;

        // Set the instance of the turn manager.
        m_tTurnManager = m_sStateMachine.m_tTurnManger;
    }

    //--------------------------------------------------------------------------------------
    // Update: Function that calls each frame to update game objects.
    //--------------------------------------------------------------------------------------
    public override void OnUpdate()
    {
        
    }

    //--------------------------------------------------------------------------------------
    // onEnter: A virtual function that runs first when a state is loaded.
    //--------------------------------------------------------------------------------------
    public override void OnEnter()
    {
        m_tTurnManager.canvas.SetActive(true);
    }

    //--------------------------------------------------------------------------------------
    // onExit: A virtual function that runs on state exit.
    //--------------------------------------------------------------------------------------
    public override void OnExit()
    {

    }
}
