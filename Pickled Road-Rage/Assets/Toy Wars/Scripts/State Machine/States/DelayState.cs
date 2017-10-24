//--------------------------------------------------------------------------------------
// Purpose: The state of delay between player turns.
//
// Description: The DelayState script is gonna be used for when the player turn is in a
// delay state. This delay state happens during the delay timer.
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
public class DelayState : State
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
    public DelayState(StateMachine sMachine)
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
        // Update the timer by deltatime.
        TurnManager.m_fTimer -= Time.deltaTime;

        // Once the timer ends.
        if (TurnManager.m_fTimer < 0)
        {
            // Push to the action state
            m_sStateMachine.ChangeState(ETurnManagerStates.ETURN_ACTION);
        }
    }

    //--------------------------------------------------------------------------------------
    // onEnter: A virtual function that runs first when a state is loaded.
    //--------------------------------------------------------------------------------------
    public override void OnEnter()
    {
        // Reset the timer.
        TurnManager.m_fTimer = TurnManager.m_sfStaticDelayLength;

        // if it is no ones turn then dont run.
        if (TurnManager.m_snCurrentTurn != 0)
        {
            // Get current players turn.
            GameObject gCurrentPlayer = m_tTurnManager.GetPlayer(TurnManager.m_snCurrentTurn);
            Player pCurrentPlayer = gCurrentPlayer.GetComponent<Player>();

            // Run the soldier manager script.
            pCurrentPlayer.SoldierTurnManager();

            // Get the current soldier object and script.
            GameObject gCurrentSoldier = pCurrentPlayer.GetSoldier(pCurrentPlayer.m_nSoldierTurn);
            SoldierActor sCurrentSoldier = gCurrentSoldier.GetComponent<SoldierActor>();

            // Activate the soldier canvas when it is the players turn.
            sCurrentSoldier.CanvasActive(true); // MAYBE GOING TO CHANGE!
        }
    }

    //--------------------------------------------------------------------------------------
    // onExit: A virtual function that runs on state exit.
    //--------------------------------------------------------------------------------------
    public override void OnExit()
    {
        // Set the delay back to 0
        TurnManager.m_fTimer = 0;
    }
}
