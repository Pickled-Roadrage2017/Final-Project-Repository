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



    float fTimer = 0;
    bool bCanRespawn = false;
    bool bIsRespawn = false;



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






        // if the timer is above 0
        if (TurnManager.m_sfTimer > 0)
        {
            // Check if a soldier can respawn.
            bCanRespawn = GetCurrentPlayerScript().CheckRespawn();
        }

        // if a soilder can not respawn.
        if (!bCanRespawn)
        {
            // set timer to zero
            TurnManager.m_sfTimer = 0;
        }

        // if a soilder can respawn.
        if (bCanRespawn)
        { 
            // Set respawn to true
            bIsRespawn = true;
        }

        // Update the timer by deltatime.
        TurnManager.m_sfTimer -= Time.deltaTime;

        // start the timer for when to spawn the solider.
        fTimer -= Time.deltaTime;

        // If the timer runs out or end turn is true.
        if (TurnManager.m_sfTimer < 0)
        {






            

            if (bIsRespawn)
            {
                // Play the teddy spawn animation.
                GetCurrentPlayerScript().m_gTeddyBase.GetComponent<Teddy>().m_bPlaceSoldier = true;

                //
                if (fTimer < 0)
                {
                    GetCurrentPlayerScript().RespawnSoldier();
                    bIsRespawn = false;
                }
            }
            
            if (!bIsRespawn)
            {
                // if the current player still has soldiers.
                if (GetCurrentPlayerScript().GetActiveSoldiers() != 0)
                {
                    // Push to the start turn state.
                    m_sStateMachine.ChangeState(ETurnManagerStates.ETURN_START);
                }
                else
                {
                    // Push to the endturn state.
                    m_sStateMachine.ChangeState(ETurnManagerStates.ETURN_END); // Will have a massive dealy!
                }
            }







        }
    }

    //--------------------------------------------------------------------------------------
    // onEnter: A virtual function that runs first when a state is loaded.
    //--------------------------------------------------------------------------------------
    public override void OnEnter()
    {
        // Reset the timer.
        TurnManager.m_sfTimer = TurnManager.m_sfStaticSpawnLength;





        fTimer = TurnManager.m_sfStaticSpawnLength;
        bCanRespawn = false;
        bIsRespawn = false;
    }

    //--------------------------------------------------------------------------------------
    // onExit: A virtual function that runs on state exit.
    //--------------------------------------------------------------------------------------
    public override void OnExit()
    {
        // Set timer back to 0.
        TurnManager.m_sfTimer = 0;
    }
}
