//--------------------------------------------------------------------------------------
// Purpose: Display current player and soldier text.
//
// Description: The SwapTurnUI script is gonna be used for displaying the current 
// players turn and their current soldier. The Text lerps across the screen, settings 
// for this are in the inspector. This script is to be attached to a textobject.
// This only runs during the Delay timer so make sure it as long as you need it to be.
//
// Author: Thomas Wiltshire.
//--------------------------------------------------------------------------------------

// Using, etc
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//--------------------------------------------------------------------------------------
// SwapTurnUI object. Inheriting from MonoBehaviour. Script for displaying the swap turn
// text and lerp across the screen.
//--------------------------------------------------------------------------------------
public class SwapTurnUI : MonoBehaviour
{
    // TEXT //
    //--------------------------------------------------------------------------------------
    // Title for this section of public values.
    [Header("Text Object:")]

    // public text object for displaying the current player turn.
    [LabelOverride("Player Turn Text")][Tooltip("The text object in the canvas that this script is attached to.")]
    public Text m_tPlayerTurnText;

    // Leave a space in the inspector
    [Space]
    //--------------------------------------------------------------------------------------

    // LERP //
    //--------------------------------------------------------------------------------------
    // Title for this section of public values.
    [Header("Lerp Settings:")]

    // public float for how long the lerp is to go for
    [LabelOverride("Lerp Time")][Tooltip("The length of time in seconds that the lerp should run for.")]
    public float m_fLerpTime;

    // public vector 3 for the starting postion of the lerp.
    [LabelOverride("Start Position")][Tooltip("The starting position of the Lerp.")]
    public Vector3 m_v3StartPos;

    // public vector 3 for the ending postion of the lerp.
    [LabelOverride("End Position")][Tooltip("The ending position of the Lerp.")]
    public Vector3 m_v3EndPos;

    // Leave a space in the inspector
    [Space]
    //--------------------------------------------------------------------------------------

    // PRIVATE VALUES //
    //--------------------------------------------------------------------------------------
    // private float for the current lerp timer.
    private float m_fCurrentLerpTime;
    //--------------------------------------------------------------------------------------

    //--------------------------------------------------------------------------------------
    // initialization.
    //--------------------------------------------------------------------------------------
    void Awake()
    {
        // Start the PlayerTurn UI text as disabled.
        m_tPlayerTurnText.enabled = false;

        // Set text object to support richtext.
        m_tPlayerTurnText.supportRichText = true;
    }

    //--------------------------------------------------------------------------------------
    // Update: Function that calls each frame to update game objects.
    //--------------------------------------------------------------------------------------
    void Update()
    {
        // If in the action state display the ticking timer.
        if (StateMachine.GetState() == ETurnManagerStates.ETURN_ACTION)
        {
            // Set the PlayerTurn text element to disabled.
            m_tPlayerTurnText.enabled = false;

            // Reset the lerp.
            m_fCurrentLerpTime = 0f;
        }

        // If it is currently the start state.
        if (StateMachine.GetState() == ETurnManagerStates.ETURN_START)
        {
            // Set the PlayerTurn text element to enabled.
            m_tPlayerTurnText.enabled = true;

            // if player ones turn.
            if (TurnManager.m_snCurrentTurn == 1)
            {
                // Display which players it currently is.
                m_tPlayerTurnText.text = "Blue's Turn!";
            }

            // if player twos turn.
            if (TurnManager.m_snCurrentTurn == 2)
            {
                // Display which players it currently is.
                m_tPlayerTurnText.text = "Red's Turn!";
            }

            // Slide the text into the screen
            TextLerp(m_tPlayerTurnText);
        }
    }

    //--------------------------------------------------------------------------------------
    // LerpText: Function to lerp a passed in text object.
    //
    // Param:
    //		tTextObject: The text object you want to lerp.
    //--------------------------------------------------------------------------------------
    void TextLerp(Text tTextObject)
    {
        // update lerp timer by delta time.
        m_fCurrentLerpTime += Time.deltaTime;

        // Check if the current time is greater than the lerp time.
        if (m_fCurrentLerpTime > m_fLerpTime)
        {
            // if it is greater then current time equels lerp time.
            m_fCurrentLerpTime = m_fLerpTime;
        }

        // New float for the progress through the lerp.
        float fProgress = m_fCurrentLerpTime / m_fLerpTime;

        // Lerp the text object.
        m_tPlayerTurnText.transform.position = Vector3.Lerp(m_v3StartPos, m_v3EndPos, fProgress);
    }
}
