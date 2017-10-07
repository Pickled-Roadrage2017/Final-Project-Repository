// Using, etc
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//--------------------------------------------------------------------------------------
// SwapTurnUI object. Inheriting from MonoBehaviour. Script Swap Turn text object.
//--------------------------------------------------------------------------------------
public class SwapTurnUI : MonoBehaviour
{
    // public text object for displaying the current player turn.
    [Tooltip("The Player turn text object in the Canvas.")]
    public Text m_tPlayerTurnText;

    // public float for how long the lerp is to go for
    [Tooltip("How long do you want the Lerp to play for.")]
    public float m_fLerpTime;

    // public vector 3 for the starting postion of the lerp.
    [Tooltip("The starting postion of the Lerp.")]
    public Vector3 m_v3StartPos;

    // public vector 3 for the ending postion of the lerp.
    [Tooltip("The ending postion of the Lerp.")]
    public Vector3 m_v3EndPos;

    // private float for the current lerp timer.
    private float m_fCurrentLerpTime;

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

        // If it is currently the delay state.
        if (StateMachine.GetState() == ETurnManagerStates.ETURN_DELAY)
        {
            // Set the PlayerTurn text element to enabled.
            m_tPlayerTurnText.enabled = true;

            // Display which players it currently is.
            m_tPlayerTurnText.text = "Player " + TurnManager.m_snCurrentTurn + "'s turn!";

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
