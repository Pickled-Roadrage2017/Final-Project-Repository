//--------------------------------------------------------------------------------------
// Purpose: The state of turn ending for players.
//
// Description: The EndState script is gonna be used for when the player turn is
// ending. The state will end on a timer, giving time for animation, reseting, etc.
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
public class EndState : State
{
    //--------------------------------------------------------------------------------------
    // Initialization: Constructor for the State.
    //
    // Param:
    //      sMachine: A reference to the StateMachine.
    //--------------------------------------------------------------------------------------
    public EndState(StateMachine sMachine) : base(sMachine)
    {
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
            // Check if gameover.
            bool bIsGameOver = GameOver();

            // if game over function is true.
            if (!bIsGameOver)
            {
                // set the turn to ended.
                TurnManager.m_sbEndTurn = true;

                // Switch players turn.
                TurnManager.SwitchTurn();

                // Push to the spawn state
                m_sStateMachine.ChangeState(ETurnManagerStates.ETURN_SPAWN);
            }
        }
    }

    //--------------------------------------------------------------------------------------
    // onEnter: A virtual function that runs first when a state is loaded.
    //--------------------------------------------------------------------------------------
    public override void OnEnter()
    {
        // Reset the timer.
        TurnManager.m_fTimer = TurnManager.m_sfStaticEndLength;

        // Set the soldier turn to false
        GetCurrentSoldierScript().CurrentTurn(false); // MAYBE GOING TO CHANGE!

        // Get active soldiers for each player.
        int nActiveSoldiersP1 = GetPlayerScript(1).GetActiveSoldiers();
        int nActiveSoldiersP2 = GetPlayerScript(2).GetActiveSoldiers();

        // Go through each soldier for player2 and set the kinematic to true.
        for (int i = 0; i < nActiveSoldiersP1; i++)
        {
            GetSoldierScript(1, i).m_rbRigidBody.isKinematic = false;
        }

        // Go through each soldier for player2 and set the kinematic to true.
        for (int i = 0; i < nActiveSoldiersP2; i++)
        {
            GetSoldierScript(2, i).m_rbRigidBody.isKinematic = false;
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

    //--------------------------------------------------------------------------------------
    // GameOver: Function for how to hit the gameover state.
    //--------------------------------------------------------------------------------------
    private bool GameOver()
    {
        // Check if a player has had a gameover.
        if (GetPlayerScript(1).CheckGameOver() || GetPlayerScript(2).CheckGameOver())
        {
            // Push to the game over state.
            m_sStateMachine.ChangeState(ETurnManagerStates.ETURN_GAMEOVER);

            // if game over return true.
            return true;
        }

        // if game over return false.
        return false;
    }
}
