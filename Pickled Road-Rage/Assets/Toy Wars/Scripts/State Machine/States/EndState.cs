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
        // If a bear for either team has taken damage.
        if (GetPlayerScript(1).m_gTeddyBase.GetComponent<Teddy>().m_bDamageAni || GetPlayerScript(2).m_gTeddyBase.GetComponent<Teddy>().m_bDamageAni)
        {
            // add seconds to the timer.
            TurnManager.m_sfTimer += 2.5f;

            // stop the animation from playing again.
            GetPlayerScript(1).m_gTeddyBase.GetComponent<Teddy>().m_bDamageAni = false;
            GetPlayerScript(2).m_gTeddyBase.GetComponent<Teddy>().m_bDamageAni = false;
        }
        
        // Update the timer by deltatime.
        TurnManager.m_sfTimer -= Time.deltaTime;
        
        // Once the timer ends.
        if (TurnManager.m_sfTimer < 0)
        {
            // Check if gameover.
            bool bIsGameOver = GameOver();

            // if the game over function is not true.
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
        TurnManager.m_sfTimer = TurnManager.m_sfStaticEndLength;

        // If the solider has shot the grenade.
        if (GetCurrentPlayerScript().m_bGrenadeShot == true)
        {
            // add seconds to the timer.
            TurnManager.m_sfTimer += 2.5f;

            // set the shot bool back to false
            GetCurrentPlayerScript().m_bGrenadeShot = false;
        }

        // loop through each material on the current solider.
        for (int o = 0; o < GetCurrentSoldierScript().GetComponent<SkinnedMeshRenderer>().materials.Length; ++o)
        {
            // Apply the outline glow around the current soldier.
            GetCurrentSoldierScript().GetComponent<SkinnedMeshRenderer>().materials[o].SetFloat("_Outline_Width", 0.0f);

            // Set the color of the outline.
            GetCurrentSoldierScript().GetComponent<SkinnedMeshRenderer>().materials[o].SetColor("_Outline_Color", GetCurrentPlayerScript().m_cPlayerColor);
        }
        
        // Set the soldier turn to false
        GetCurrentSoldierScript().CurrentTurn(false);

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
        TurnManager.m_sfTimer = 0;
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
