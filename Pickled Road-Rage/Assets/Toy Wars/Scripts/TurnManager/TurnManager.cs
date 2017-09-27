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
    public float m_fStartTime; // TOOLTIPS!

    // static int for the current players turn.
    public static int m_snCurrentTurn; // ASK RICHARD ABOUT SEEING IN INSPECTOR. // TOOLTIPS!

    // float for the turn timer.
    public float m_fTurnTimer; // PUBLIC FOR TESTING. // ASK RICHARD IF CAN BE PUBLIC BUT NOT CHANGED. // TOOLTIPS!

    // static bool for ending player turns.
    static bool m_sbEndTurn;  // TOOLTIPS!

    //--------------------------------------------------------------------------------------
    // initialization
    //--------------------------------------------------------------------------------------
    void Awake()
    {
        // Set default values.
        m_fTurnTimer = m_fStartTime;
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
        m_fTurnTimer -= Time.deltaTime;

        // If the timer runs out.
        if (m_fTurnTimer < 0)
        {
            // set the turn to ended.
            m_sbEndTurn = true;
        }

        // Switch players turn.
        SwitchTurn();
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

                // change end turn back to false.
                m_sbEndTurn = false;

                // Set the timer back to Start time.
                m_fTurnTimer = m_fStartTime;

                return;
            }

            // If Player 2
            if (m_snCurrentTurn == 2)
            {
                // Switch to player 1.
                m_snCurrentTurn = 1;

                // change end turn back to false.
                m_sbEndTurn = false;

                // Set the timer back to Start time.
                m_fTurnTimer = m_fStartTime;

                return;
            }

            // if for some reason the current turn isnt 1 or 2 reset the turn manager.
            else
            {
                m_snCurrentTurn = 0;
                m_sbEndTurn = false;
                m_fTurnTimer = m_fStartTime;
            }
        }
    }
}