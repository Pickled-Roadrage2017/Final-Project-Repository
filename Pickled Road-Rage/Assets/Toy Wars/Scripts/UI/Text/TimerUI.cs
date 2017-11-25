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
using System.Timers;
using UnityEngine;
using UnityEngine.UI;

//--------------------------------------------------------------------------------------
// TimerUI object. Inheriting from MonoBehaviour. Script for displaying the turn timer
// text object.
//--------------------------------------------------------------------------------------
public class TimerUI : MonoBehaviour
{
    // TEXT //
    //--------------------------------------------------------------------------------------
    // Title for this section of public values.
    [Header("Text:")]
    
    // public text object for displaying the current player turn timer.
    [LabelOverride("Turn Timer Text")][Tooltip("The text object in the canvas that this script is attached to.")]
    public Text m_tTurnTimerText;
    
    // Leave a space in the inspector
    [Space]
    //--------------------------------------------------------------------------------------

    // LERP COLOR //
    //--------------------------------------------------------------------------------------
    // Title for this section of public values.
    [Header("Lerp Color:")]

    // public colors array for color to lerp from and to.
    [LabelOverride("Colors")] [Tooltip("What colors (first being default color, second being the color to lerp) do you want to lerp.")]
    public Color[] m_acColors;

    // public float for the flash starting time.
    [LabelOverride("Starting Time")] [Tooltip("When in the remaining time do you want to start the text flashing?")]
    public float m_fFlashTimeStart = 6.0f;

    // public float to increment by.
    [LabelOverride("Increment By")] [Tooltip("How much to increment the lerp by (this is not in seconds)")]
    public float m_fIncrement = 0.40f;

    // public float for when to switch lerp.
    [LabelOverride("Switch Time")] [Tooltip("When to switch the lerp (this is not in seconds. 0.5 is about half a second)")]
    public float m_fSwitchTime = 5.0f;

    // Leave a space in the inspector
    [Space]
    //--------------------------------------------------------------------------------------

    // PRIVATE VALUES //
    //--------------------------------------------------------------------------------------
    // private bool for when to change color.
    private bool m_bChangeColor = false;

    // private float for lerping.
    private float m_fLerpTimer = 0;
    //--------------------------------------------------------------------------------------

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
        int nMinutes = Mathf.FloorToInt(TurnManager.m_sfTimer / 60F);
        int nSeconds = Mathf.FloorToInt(TurnManager.m_sfTimer - nMinutes * 60);
        string nTimer = string.Format("{0:0}:{1:00}", nMinutes, nSeconds);

        // If in the action state display the ticking timer.
        if (StateMachine.GetState() == ETurnManagerStates.ETURN_ACTION)
        {
            // display the ui timer.
            m_tTurnTimerText.text = nTimer;

            // lerp the text color.
            LerpTimerText();
        }

        // If in the end state display
        if (StateMachine.GetState() == ETurnManagerStates.ETURN_END)
        {
            // create temp string for checking.
            string sTemp = "0:00";

            // if text equals temp string than lerp the color.
            if (m_tTurnTimerText.text.Equals(sTemp))
                LerpTimerText();
        }

        // If it is currently the start state.
        if (StateMachine.GetState() == ETurnManagerStates.ETURN_START)
        {
            m_tTurnTimerText.color = m_acColors[0];

            // display the ui timer at 0.
            m_tTurnTimerText.text = string.Format("{0:0}:{1:00}", 0, Mathf.FloorToInt(TurnManager.m_sfStaticTimerLength - nMinutes * 60));
        }
    }

    //--------------------------------------------------------------------------------------
    // LerpTimerText: Lerp from 1 color to another. Used for text flashing when timer is low.
    //--------------------------------------------------------------------------------------
    void LerpTimerText()
    {
        // if timer is less than flash time start and not the turn start state.
        if (TurnManager.m_sfTimer < m_fFlashTimeStart)
        {
            // increment up and down.
            if (!m_bChangeColor)
                m_fLerpTimer += m_fIncrement;
            else
                m_fLerpTimer -= m_fIncrement;

            // if LerpTimer is over m_fSwitchTime
            if (m_fLerpTimer >= m_fSwitchTime)
            {
                // Set change color bool and change text color.
                m_bChangeColor = true;
                m_tTurnTimerText.color = Color.LerpUnclamped(m_acColors[1], m_acColors[0], 0);
            }

            // if LerpTimer is less than 0
            if (m_fLerpTimer <= 0)
            {
                // Set change color bool and change text color.
                m_bChangeColor = false;
                m_tTurnTimerText.color = Color.LerpUnclamped(m_acColors[1], m_acColors[0], 1);
            }
        }
    }
}
