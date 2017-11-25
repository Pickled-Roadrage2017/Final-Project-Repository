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
    // float for the animation timer.
    float m_fAnimationTimer = 0;

    // bool for if a respawn can happen.
    bool m_bCanRespawn = false;

    // bool of if a respawn will happen.
    bool m_bIsRespawn = false;

    // bool for playing the spawning audio.
    bool m_bCanPlayAudio = true;

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
            m_bCanRespawn = GetCurrentPlayerScript().CheckRespawn();
        }

        // if a soilder can not respawn.
        if (!m_bCanRespawn)
        {
            // set timer to zero
            TurnManager.m_sfTimer = 0;
        }

        // if a soilder can respawn.
        if (m_bCanRespawn)
        {
            // Set respawn to true
            m_bIsRespawn = true;
        }

        // Update the spawn timer by deltatime.
        TurnManager.m_sfTimer -= Time.deltaTime;

        // start the animation timer for when to spawn the solider.
        m_fAnimationTimer -= Time.deltaTime;

        // If the spawm timer runs out.
        if (TurnManager.m_sfTimer < 0)
        {
            // If respawn
            if (m_bIsRespawn)
            {
                // Play the teddy spawn animation.
                GetCurrentPlayerScript().m_gTeddyBase.GetComponent<Teddy>().m_bPlaceSoldierAni = true;

                // if the animation timer is less than 1 and audio can be played.
                if (m_fAnimationTimer < 1 && m_bCanPlayAudio)
                {
                    // make sure only the audio only plays once.
                    if (!GetCurrentPlayerScript().m_gTeddyBase.GetComponent<Teddy>().m_asAudioSource.isPlaying)
                    {
                        // Play palce sound.
                        GetCurrentPlayerScript().m_gTeddyBase.GetComponent<Teddy>().m_asAudioSource.PlayOneShot(GetCurrentPlayerScript().m_gTeddyBase.GetComponent<Teddy>().m_acPlaceSound);
                        m_bCanPlayAudio = false;
                    }
                }

                // spawn the soldier once the animation finishes.
                if (m_fAnimationTimer < 0)
                {
                    // Respawn soldier.
                    GetCurrentPlayerScript().RespawnSoldier();
                    
                    // respawn is now fasle.
                    m_bIsRespawn = false;
                }
            }
            
            // If no respawn
            if (!m_bIsRespawn)
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
        
        // Reset animation timer.
        m_fAnimationTimer = TurnManager.m_sfStaticSpawnLength;

        // Reset spawn bools.
        m_bCanRespawn = false;
        m_bIsRespawn = false;
    }

    //--------------------------------------------------------------------------------------
    // onExit: A virtual function that runs on state exit.
    //--------------------------------------------------------------------------------------
    public override void OnExit()
    {
        // Set timer back to 0.
        TurnManager.m_sfTimer = 0;

        // make sure the spawn sound has stop.
        m_tTurnManager.m_asAudioSource.Stop();

        // Set can play back to true.
        m_bCanPlayAudio = true;
    }
}
