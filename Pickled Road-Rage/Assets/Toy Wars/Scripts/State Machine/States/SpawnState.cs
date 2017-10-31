//--------------------------------------------------------------------------------------
// Purpose: The state when soldier respawning happens.
//
// Description: The SpawnState script is gonna be used for checking if the soliders can 
// respawn and if so respawning these soliders. The state will end on a timer, giving 
// time for animation, reseting, etc.
//
// Author: Thomas Wiltshire.
//--------------------------------------------------------------------------------------

// Using, etc
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//--------------------------------------------------------------------------------------
// SpawnState object. Inheriting from State. State for turn spawning new soldiers.
//--------------------------------------------------------------------------------------
public class SpawnState : State
{
    //--------------------------------------------------------------------------------------
    // Initialization: Constructor for the State.
    //
    // Param:
    //      sMachine: A reference to the StateMachine.
    //--------------------------------------------------------------------------------------
    public SpawnState(StateMachine sMachine) : base(sMachine)
    {

    }

    //--------------------------------------------------------------------------------------
    // Update: Function that calls each frame to update game objects.
    //--------------------------------------------------------------------------------------
    public override void OnUpdate()
    {
        // Check if a soldier can respawn.
        bool bIsRespawn = GetCurrentPlayerScript().CheckRespawn();

        // if a soilder can not respawn.
        if (!bIsRespawn)
        {
            // set timer to zero
            TurnManager.m_fTimer = 0;
        }

        // Update the timer by deltatime.
        TurnManager.m_fTimer -= Time.deltaTime;

        // If the timer runs out or end turn is true.
        if (TurnManager.m_fTimer < 0)
        {
            // Push to the endturn state.
            m_sStateMachine.ChangeState(ETurnManagerStates.ETURN_START);
        }
    }

    //--------------------------------------------------------------------------------------
    // onEnter: A virtual function that runs first when a state is loaded.
    //--------------------------------------------------------------------------------------
    public override void OnEnter()
    {
        // Reset the timer.
        TurnManager.m_fTimer = TurnManager.m_sfStaticSpawnLength;
    }

    //--------------------------------------------------------------------------------------
    // onExit: A virtual function that runs on state exit.
    //--------------------------------------------------------------------------------------
    public override void OnExit()
    {
        // Set timer back to 0.
        TurnManager.m_fTimer = 0;
    }
}
