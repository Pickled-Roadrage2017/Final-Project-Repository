﻿//--------------------------------------------------------------------------------------
// Purpose: Manager Player turns.
//
// Description: The TurnManager script is gonna be used for switching the players turns.
// This script is to be attached to an empty gameobject.
//
// Author: Thomas Wiltshire.
//--------------------------------------------------------------------------------------

// Using, etc
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//--------------------------------------------------------------------------------------
// TurnManager object. Inheriting from MonoBehaviour. Used for switch Player turns.
//--------------------------------------------------------------------------------------
public class TurnManager : MonoBehaviour
{
    // Title for this section of public values.
    [Header("Timer:")]

    // public float starting time for the Turn timer.
    [LabelOverride("Timer Length")][Tooltip("The time in seconds for how long each turn should go for.")]
    public float m_fTimerLength;

    // public float starting time for the delay timer.
    [LabelOverride("Delay Timer Length")][Tooltip("The time in seconds for how long the delay between turns should be.")]
    public float m_fDelayLength;

    // public float starting time for the end timer.
    [LabelOverride("End Turn Timer Length")][Tooltip("The time in seconds for how long the end of turn state should be. This end turn state is just a delay for the end of turns.")]
    public float m_fEndLength;

    // Title for this section of public values.
    [Header("Player Objects:")]

    // public gameobject for the player1 object.
    [LabelOverride("Player1 Object")][Tooltip("The Player1 object in the scene.")]
    public GameObject m_gPlayer1;

    // public gamobject for the player2 object.
    [LabelOverride("Player2 Object")][Tooltip("The Player2 object in the scene.")]
    public GameObject m_gPlayer2;

    // static int for the current players turn.
    public static int m_snCurrentTurn;

    // static bool for ending player turns.
    public static bool m_sbEndTurn;

    // Static values for timers.
    public static float m_sfStaticTimerLength;
    public static float m_sfStaticDelayLength;
    public static float m_sfStaticEndLength;

    // float for timer.
    public static float m_fTimer;

    // New instance of the state machine.
    private StateMachine m_sStateMachine;
    
    //--------------------------------------------------------------------------------------
    // initialization. Awake.
    //--------------------------------------------------------------------------------------
    void Awake()
    {
        // Set default values.
        m_sbEndTurn = false;
        m_snCurrentTurn = 0;

        // Set static values
        m_sfStaticTimerLength = m_fTimerLength;
        m_sfStaticDelayLength = m_fDelayLength;
        m_sfStaticEndLength = m_fEndLength;

        // Decide who goes first
        DecideTurn();

        // Create a new Statmachine.
        m_sStateMachine = new StateMachine(this);

        // Add states to the machine.
        m_sStateMachine.AddState(ETurnManagerStates.ETURN_DELAY, new DelayState(m_sStateMachine));
        m_sStateMachine.AddState(ETurnManagerStates.ETURN_ACTION, new ActionState(m_sStateMachine));
        m_sStateMachine.AddState(ETurnManagerStates.ETURN_END, new EndTurnState(m_sStateMachine));
        m_sStateMachine.AddState(ETurnManagerStates.ETURN_GAMEOVER, new GameOverState(m_sStateMachine));

        // Pass the state machine instance to the players.
        m_gPlayer1.GetComponent<Player>().SetInstances(this, m_sStateMachine); // Might be useless?
        m_gPlayer2.GetComponent<Player>().SetInstances(this, m_sStateMachine); // Might be useless?

        









        GameObject[] gos = GameObject.FindGameObjectsWithTag("EndMenu");
        GameObject canvas = null;

        foreach (GameObject go in gos)
        {
            canvas = go;
        }

        canvas.SetActive(false);

    }

    //--------------------------------------------------------------------------------------
    // initialization. Start.
    //--------------------------------------------------------------------------------------
    private void Start()
    {
        // Set the first state to the delay state.
        m_sStateMachine.ChangeState(ETurnManagerStates.ETURN_DELAY);
    }

    //--------------------------------------------------------------------------------------
    // Update: Function that calls each frame to update game objects.
    //--------------------------------------------------------------------------------------
    void Update()
    {
        // Update the state machine.
        m_sStateMachine.OnUpdate();
    }

    //--------------------------------------------------------------------------------------
    // GetPlayer: Return the Player object from the index passed in.
    //--------------------------------------------------------------------------------------
    public GameObject GetPlayer(int nPlayerNumber)
    {
        // If nPlayerNumber is player1 return player1
        if (nPlayerNumber == 1)
        {
            return m_gPlayer1;
        }

        // If nPlayerNumber is player2 return player2
        else if (nPlayerNumber == 2)
        {
            return m_gPlayer2;
        }

        // Else if no player then return null.
        else
        {
            return null;
        }
    }

    //--------------------------------------------------------------------------------------
    // DecideTurn: Decide who goes first at the start of the game.
    //--------------------------------------------------------------------------------------
    void DecideTurn()
    {
        // If there is no current turn
        if (m_snCurrentTurn == 0)
        {
            // then select a random player.
            m_snCurrentTurn = Random.Range(1, 2);
        }
    }

    //--------------------------------------------------------------------------------------
    // SwitchTurn: Function switches to the next players turn.
    //--------------------------------------------------------------------------------------
    public static void SwitchTurn()
    {
        // If turn has ended.
        if (m_sbEndTurn == true)
        {
            // If player 1
            if (m_snCurrentTurn == 1)
            {
                // Switch to player 2.
                m_snCurrentTurn = 2;

                // Set end turn back to false.
                m_sbEndTurn = false;

                // Return from the function.
                return;
            }

            // If Player 2
            if (m_snCurrentTurn == 2)
            {
                // Switch to player 1.
                m_snCurrentTurn = 1;

                // Set end turn back to false.
                m_sbEndTurn = false;

                // Return from the function.
                return;
            }

            // if for some reason the current turn isnt 1 or 2.
            else
            {
                // Reset values.
                m_snCurrentTurn = 0;
                m_sbEndTurn = false;
            }
        }
    }
}