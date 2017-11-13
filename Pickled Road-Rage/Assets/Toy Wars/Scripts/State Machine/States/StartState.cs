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
// StartState object. Inheriting from State. Start of the Player turn after switching.
//--------------------------------------------------------------------------------------
public class StartState : State
{
    //--------------------------------------------------------------------------------------
    // Initialization: Constructor for the State.
    //
    // Param:
    //      sMachine: A reference to the StateMachine.
    //--------------------------------------------------------------------------------------
    public StartState(StateMachine sMachine) : base(sMachine)
    {
    }

    //--------------------------------------------------------------------------------------
    // Update: Function that calls each frame to update game objects.
    //--------------------------------------------------------------------------------------
    public override void OnUpdate()
    {
        // Update the timer by deltatime.
        TurnManager.m_fTimer -= Time.deltaTime;

        // Once the timer ends or any key is pressed
        if (TurnManager.m_fTimer < 0 || Input.anyKeyDown)
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
            // Play start turn sound.
            m_tTurnManager.m_asAudioSource.PlayOneShot(m_tTurnManager.m_acTurnStartAudio);

            // Run the soldier manager script.
            GetCurrentPlayerScript().SoldierTurnManager();

            // Set the soldier turn to true.
            GetCurrentSoldierScript().CurrentTurn(true); // MAYBE GOING TO CHANGE!

            // Get active soldiers for each player.
            int nActiveSoldiersP1 = GetPlayerScript(1).GetActiveSoldiers();
            int nActiveSoldiersP2 = GetPlayerScript(2).GetActiveSoldiers();

            // Go through each soldier for player2 and set the kinematic to true.
            for (int i = 0; i < nActiveSoldiersP1; i++)
            {
                GetSoldierScript(1, i).m_rbRigidBody.isKinematic = true;
            }

            // Go through each soldier for player2 and set the kinematic to true.
            for (int i = 0; i < nActiveSoldiersP2; i++)
            {
                GetSoldierScript(2, i).m_rbRigidBody.isKinematic = true;
            }

            // Set the current soldier isKinematic to false.
            GetCurrentSoldierScript().m_rbRigidBody.isKinematic = false;
        }
    }

    //--------------------------------------------------------------------------------------
    // onExit: A virtual function that runs on state exit.
    //--------------------------------------------------------------------------------------
    public override void OnExit()
    {
        // Set the delay back to 0
        TurnManager.m_fTimer = 0;

        // make sure the start turn song has stop.
        m_tTurnManager.m_asAudioSource.Stop();
    }
}
