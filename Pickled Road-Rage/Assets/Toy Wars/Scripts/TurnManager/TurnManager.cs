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
    public float m_fTurnTimerLength;

    // public float starting time for the delay timer.
    [Tooltip("How long should each turn go for. Time in seconds.")]
    public float m_fDelayTimerLength;

    // static int for the current players turn.
    //[Tooltip("Which Player's turn is it currently.")]
    private static int m_snCurrentTurn; // ASK RICHARD ABOUT SEEING IN INSPECTOR.

    // float for the turn timer.
    //[Tooltip("What the current player ticking turn timer is.")] // ASK RICHARD ABOUT SEEING IN INSPECTOR.
    private static float m_sfTurnTimer; // ASK RICHARD IF CAN BE PUBLIC BUT NOT CHANGED.

    // float for turn delay.
    [Tooltip("What the current player ticking delay timer is.")]
    public float m_fDelayTimer; // ASK RICHARD IF CAN BE PUBLIC BUT NOT CHANGED.

    // static bool for ending player turns.
    private static bool m_sbEndTurn;

    // Getter for m_snCurrentTurn.
    public static int GetCurrentTurn()
    {
        return m_snCurrentTurn;
    }

    // Getter for m_sfTurnTimer.
    public static float GetTurnTimer()
    {
        return m_sfTurnTimer;
    }

    // Getter for m_sbEndTurn.
    public static bool GetEndTurn()
    {
        return m_sbEndTurn;
    }

    //--------------------------------------------------------------------------------------
    // initialization.
    //--------------------------------------------------------------------------------------
    void Awake()
    {
        // Set default values.
        m_sfTurnTimer = m_fTurnTimerLength;
        m_fDelayTimer = m_fDelayTimerLength;
        m_sbEndTurn = false;
        m_snCurrentTurn = 0;
    }

    //--------------------------------------------------------------------------------------
    // Update: Function that calls each frame to update game objects.
    //--------------------------------------------------------------------------------------
    void Update()
    {
        // If there is no current turn
        if (m_snCurrentTurn == 0)
        {
            // then select a random player.
            m_snCurrentTurn = Random.Range(1, 2);
        }

        // Update the timer by deltatime.
        m_sfTurnTimer -= Time.deltaTime;

        // If the timer runs out.
        if (m_sfTurnTimer < 0)
        {
            // set the turn to ended.
            m_sbEndTurn = true;

            // set the delay timer.
            m_fDelayTimer -= Time.deltaTime;
        }

        // Switch players turn.
        SwitchTurn();
    }

    //--------------------------------------------------------------------------------------
    // DelayTurn: Delay the player turn from changing.
    //--------------------------------------------------------------------------------------
    void DelayTurn()
    {
        // Once the timer ends.
        if (m_fDelayTimer < 0)
        {
            // change end turn back to false.
            m_sbEndTurn = false;

            // Set the timers back to Start time.
            m_sfTurnTimer = m_fTurnTimerLength;
            m_fDelayTimer = m_fDelayTimerLength;
        }
    }

    //--------------------------------------------------------------------------------------
    // SwitchTurn: Function switches to the next players turn.
    //--------------------------------------------------------------------------------------
    void SwitchTurn()
    {
        // If turn has ended.
        if (m_sbEndTurn == true)
        {
            // If player 1
            if (m_snCurrentTurn == 1)
            {
                // Switch to player 2.
                m_snCurrentTurn = 2;

                // Delay turn start.
                DelayTurn();

                return;
            }

            // If Player 2
            if (m_snCurrentTurn == 2)
            {
                // Switch to player 1.
                m_snCurrentTurn = 1;

                // Delay turn start.
                DelayTurn();

                return;
            }

            // if for some reason the current turn isnt 1 or 2.
            else
            {
                // Reset values.
                m_snCurrentTurn = 0;
                m_sbEndTurn = false;
                m_sfTurnTimer = m_fTurnTimerLength;
                m_fDelayTimer = m_fDelayTimerLength;
            }
        }
    }
}