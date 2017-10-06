// Using, etc
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//--------------------------------------------------------------------------------------
// TurnManager object. Inheriting from MonoBehaviour. Used for switch Player turns.
//--------------------------------------------------------------------------------------
public class TurnManager : MonoBehaviour
{
    // public float starting time for the Turn timer.
    [Tooltip("How long should each turn go for. Time in seconds.")]
    public float m_fTimerLength;

    // public float starting time for the delay timer.
    [Tooltip("How long should the delay be before a turn starts. Time in seconds.")]
    public float m_fDelayLength;

    // public gameobject for the player1 object.
    [Tooltip("The Empty object used for Player1 prefab")]
    public GameObject m_gPlayer1;

    // public gamobject for the player2 object.
    [Tooltip("The Empty object used for Player2 prefab.")]
    public GameObject m_gPlayer2;

    // static int for the current players turn.
    public static int m_snCurrentTurn;

    // static bool for ending player turns.
    public static bool m_sbEndTurn;

    // New instance of the state machine.
    private StateMachine m_sStateMachine;

    // Static values for timers.
    public static float m_sfStaticTimerLength;
    public static float m_sfStaticDelayLength;

    // float for timer.
    public static float m_fTimer;
    
    //--------------------------------------------------------------------------------------
    // initialization.
    //--------------------------------------------------------------------------------------
    void Awake()
    {
        // Set default values.
        m_sbEndTurn = false;
        m_snCurrentTurn = 0;

        // Set static values
        m_sfStaticTimerLength = m_fTimerLength;
        m_sfStaticDelayLength = m_fDelayLength;

        // Create a new Statmachine.
        m_sStateMachine = new StateMachine(this);

        // Add states to the machine.
        m_sStateMachine.AddState(ETurnManagerStates.ETURN_DELAY, new DelayState(m_sStateMachine));
        m_sStateMachine.AddState(ETurnManagerStates.ETURN_ACTION, new ActionState(m_sStateMachine));

        // Set the first state to the delay state.
        m_sStateMachine.ChangeState(ETurnManagerStates.ETURN_DELAY);

        // Decide who goes first
        DecideTurn();
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
    // GetPlayer: Return the Player object from the number passed in.
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