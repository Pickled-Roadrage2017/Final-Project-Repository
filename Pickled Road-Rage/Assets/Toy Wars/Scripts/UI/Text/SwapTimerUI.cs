//--------------------------------------------------------------------------------------
// Purpose: Show the time left until the turn start.
//
// Description: The SwapTimerUI script is gonna be used for displaying the ammount of 
// time until the current players turn starts. This script is to be attached to a 
// textobject.
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
public class SwapTimerUI : MonoBehaviour
{
    // LERP COLOR //
    //--------------------------------------------------------------------------------------
    // Title for this section of public values.
    [Header("Lerp Color:")]

    // public colors array for color to lerp from and to.
    [LabelOverride("Colors")] [Tooltip("What colors (first being default color, second being the color to lerp) do you want to lerp.")]
    public Color[] m_acColors;
    
    // Leave a space in the inspector
    [Space]
    //--------------------------------------------------------------------------------------

    // PRIVATE VALUES //
    //--------------------------------------------------------------------------------------
    // text object for displaying the current player timer.
    private Text m_tTurnTimerText;
    //--------------------------------------------------------------------------------------

    //--------------------------------------------------------------------------------------
    // initialization.
    //--------------------------------------------------------------------------------------
    void Awake()
    {
        // get the text component.
        m_tTurnTimerText = GetComponent<Text>();
    }

    //--------------------------------------------------------------------------------------
    // Update: Function that calls each frame to update game objects.
    //--------------------------------------------------------------------------------------
    void Update()
    {
        // Format the timer as a timer with seconds.
        int nSeconds = Mathf.FloorToInt(TurnManager.m_sfTimer);
        string nTimer = string.Format("{0}", nSeconds + 1);

        // If in the start state display the ticking timer.
        if (StateMachine.GetState() == ETurnManagerStates.ETURN_START)
        {
            // display the ui timer.
            m_tTurnTimerText.text = nTimer;

            // Switch the color to white when the timer is higher than 3.
            if (TurnManager.m_sfTimer > 3)
            {
                m_tTurnTimerText.color = Color.LerpUnclamped(m_acColors[1], m_acColors[0], 1);
            }

            // Switch the color to red when the timer is lower than 3.
            if (TurnManager.m_sfTimer < 3)
            {
                m_tTurnTimerText.color = Color.LerpUnclamped(m_acColors[1], m_acColors[0], 0);
            }
        }
    }
}
