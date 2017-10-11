//--------------------------------------------------------------------------------------
// Purpose: Display the turn timer.
//
// Description: The TimerUI script is gonna be used for displaying the current game 
// turn timer. This script is to be attached to a textobject.
//
// Author: Thomas Wiltshire.
//--------------------------------------------------------------------------------------

// Using, etc
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//--------------------------------------------------------------------------------------
// TimerUI object. Inheriting from MonoBehaviour. Script for displaying the turn timer
// text object.
//--------------------------------------------------------------------------------------
public class TimerUI : MonoBehaviour
{
    // public text object for displaying the current player turn timer.
    [LabelOverride("Turn Timer Text")][Tooltip("The text object in the canvas that this script is attached to.")]
    public Text m_tTurnTimerText;

    //--------------------------------------------------------------------------------------
    // initialization.
    //--------------------------------------------------------------------------------------
    void Awake()
    {
        // Set text object to support richtext.
        m_tTurnTimerText.supportRichText = true;
    }

    //--------------------------------------------------------------------------------------
    // Update: Function that calls each frame to update game objects.
    //--------------------------------------------------------------------------------------
    void Update()
    {
        // Format the timer as a timer with seconds and minutes.
        int nMinutes = Mathf.FloorToInt(TurnManager.m_fTimer / 60F);
        int nSeconds = Mathf.FloorToInt(TurnManager.m_fTimer - nMinutes * 60);
        string nTimer = string.Format("{0:0}:{1:00}", nMinutes, nSeconds);

        // If in the action state display the ticking timer.
        if (StateMachine.GetState() == ETurnManagerStates.ETURN_ACTION)
        {
            // display the ui timer.
            m_tTurnTimerText.text = nTimer;
        }

        // If it is currently the delay state.
        if (StateMachine.GetState() == ETurnManagerStates.ETURN_DELAY)
        {
            // display the ui timer at 0.
            m_tTurnTimerText.text = "0:00";
        }
    }
}
