//--------------------------------------------------------------------------------------
// Purpose: The state of turn ending for players.
//
// Description: The EndTurnState script is gonna be used for when the player turn is
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
public class EndTurnState : State
{
    // State machine instance
    StateMachine m_sStateMachine;

    // Turn manager instance
    TurnManager m_tTurnManager;

    // Get player1
    Player m_pPlayer1;

    // Get player2
    Player m_pPlayer2;

    //--------------------------------------------------------------------------------------
    // Initialization: Constructor for the State.
    //
    // Param:
    //      sMachine: A reference to the StateMachine.
    //--------------------------------------------------------------------------------------
    public EndTurnState(StateMachine sMachine)
    {
        // Set the instance of the statemachine.
        m_sStateMachine = sMachine;

        // Set the instance of the turn manager.
        m_tTurnManager = m_sStateMachine.m_tTurnManger;

        // Get player1 and assign to pPlayer1
        GameObject gPlayer1 = m_tTurnManager.GetPlayer(1);
        m_pPlayer1 = gPlayer1.GetComponent<Player>();

        // Get player2 and assign to pPlayer2
        GameObject gPlayer2 = m_tTurnManager.GetPlayer(2);
        m_pPlayer2 = gPlayer2.GetComponent<Player>();
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
            bool IsGameOver = GameOver();

            // if game over function is true.
            if (!IsGameOver)
            {
                // if a player has no soldiers dont swap to that player.
                if (m_pPlayer1.GetActiveSoldiers() != 0 && m_pPlayer2.GetActiveSoldiers() != 0)
                {
                    // set the turn to ended.
                    TurnManager.m_sbEndTurn = true;

                    // Switch players turn.
                    TurnManager.SwitchTurn();
                }
                else
                {
                    // set the end turn to true.
                    TurnManager.m_sbEndTurn = false;
                }

                // Push to the delay state
                m_sStateMachine.ChangeState(ETurnManagerStates.ETURN_DELAY);
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

        // Get current players turn.
        GameObject gCurrentPlayer = m_tTurnManager.GetPlayer(TurnManager.m_snCurrentTurn);
        Player pCurrentPlayer = gCurrentPlayer.GetComponent<Player>();

        // Get the current soldier object and script.
        GameObject gCurrentSoldier = pCurrentPlayer.GetSoldier(pCurrentPlayer.m_nSoldierTurn);
        SoldierActor sCurrentSoldier = gCurrentSoldier.GetComponent<SoldierActor>();

        // Activate the soldier canvas when it is the players turn.
        sCurrentSoldier.CanvasActive(false); // MAYBE GOING TO CHANGE!
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
        // Get player1 and Teddy.
        float fTeddyHealth1 = m_pPlayer1.GetComponent<Player>().m_gTeddyBase.GetComponent<Teddy>().m_fCurrentHealth;

        // Get player2 and Teddy.
        float fTeddyHealth2 = m_pPlayer2.GetComponent<Player>().m_gTeddyBase.GetComponent<Teddy>().m_fCurrentHealth;

        // Check Teddy current health for each player.
        if (fTeddyHealth1 <= 0 || fTeddyHealth2 <= 0 )
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
