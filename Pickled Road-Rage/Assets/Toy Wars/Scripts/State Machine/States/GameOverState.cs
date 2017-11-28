//--------------------------------------------------------------------------------------
// Purpose: The state for when the game has eneded.
//
// Description: The GameOverState script is gonna be used for when the game hits its end
// end state. Will enable the gameover UI etc.
//
// Author: Thomas Wiltshire.
//--------------------------------------------------------------------------------------

// Using, etc
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//--------------------------------------------------------------------------------------
// GameOverState object. Inheriting from State. The end of game state.
//--------------------------------------------------------------------------------------
public class GameOverState : State
{
    //--------------------------------------------------------------------------------------
    // Initialization: Constructor for the State.
    //
    // Param:
    //      sMachine: A reference to the StateMachine.
    //--------------------------------------------------------------------------------------
    public GameOverState(StateMachine sMachine) : base(sMachine)
    {
    }

    //--------------------------------------------------------------------------------------
    // Update: Function that calls each frame to update game objects.
    //--------------------------------------------------------------------------------------
    public override void OnUpdate()
    {
        // If player 1 has a gameover.
        if (GetPlayerScript(1).CheckGameOver()) // Possibly fix if the death animation takes to long to play
        {
            // Play the teddy winning animation for the winning teddy.
            GetPlayerScript(2).m_gTeddyBase.GetComponent<Teddy>().m_bWinAni = true;

            // Go through each soldier for player 1
            for (int i = 0; i < GetPlayerScript(1).m_agSoldierList.Length; ++i)
            {
                // Set the soldier winning animation.
                GetPlayerScript(2).m_agSoldierList[i].GetComponent<SoldierActor>().m_bWinAni = true;
            }
        }

        // If player 2 has a gameover.
        if (GetPlayerScript(2).CheckGameOver()) // Possibly fix if the death animation takes to long to play
        {
            // Play the teddy winning animation for the winning teddy.
            GetPlayerScript(1).m_gTeddyBase.GetComponent<Teddy>().m_bWinAni = true;

            // Go through each soldier for player 2 
            for (int i = 0; i < GetPlayerScript(1).m_agSoldierList.Length; ++i)
            {
                // Set the soldier winning animation.
                GetPlayerScript(1).m_agSoldierList[i].GetComponent<SoldierActor>().m_bWinAni = true;
            }
        }
    }

    //--------------------------------------------------------------------------------------
    // onEnter: A virtual function that runs first when a state is loaded.
    //--------------------------------------------------------------------------------------
    public override void OnEnter()
    {
        // Check if there is a valid GameOverCanvas
        if (m_tTurnManager.m_gGameOverCanvas != null)
        {
            // Set the gameover canvas to true.
            m_tTurnManager.m_gGameOverCanvas.SetActive(true);
        }

        // Check if there is a valid fireworks
        if (m_tTurnManager.m_gFireworks != null)
        {
            // Set the firworks to true.
            m_tTurnManager.m_gFireworks.SetActive(true);
        }
    }

    //--------------------------------------------------------------------------------------
    // onExit: A virtual function that runs on state exit.
    //--------------------------------------------------------------------------------------
    public override void OnExit()
    {
    }
}
