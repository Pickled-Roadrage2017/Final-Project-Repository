//--------------------------------------------------------------------------------------
// Purpose: 
//
// Description: 
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

    // private bool for when to change color.
    private bool m_bChangeColor = false;

    // private float for lerping.
    private float m_fLerpTimer = 0;

    // float for the flash starting time.
    private float m_fFlashTimeStart = 5;
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
        string nTimer = string.Format("{0}", nSeconds);

        // If in the start state display the ticking timer.
        if (StateMachine.GetState() == ETurnManagerStates.ETURN_START)
        {
            // display the ui timer.
            m_tTurnTimerText.text = nTimer;

            if (TurnManager.m_sfTimer < 3)
            {
                m_tTurnTimerText.color = Color.LerpUnclamped(m_acColors[1], m_acColors[0], 0);
            }
        }
        
        // If it is currently the start state.
        if (StateMachine.GetState() == ETurnManagerStates.ETURN_ACTION)
        {
            m_tTurnTimerText.color = Color.LerpUnclamped(m_acColors[1], m_acColors[0], 1);
        }
    }
}
