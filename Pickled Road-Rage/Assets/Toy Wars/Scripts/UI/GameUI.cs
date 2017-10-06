// Using, etc
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//--------------------------------------------------------------------------------------
// GameUI object. Inheriting from MonoBehaviour. Script for showing the main game UI
//--------------------------------------------------------------------------------------
public class GameUI : MonoBehaviour
{
    // public text object for displaying the current player turn timer.
    [Tooltip("The Turn Timer text object in the Canvas.")]
    public Text m_tTurnTimerText;

    // public text object for displaying the current player turn.
    [Tooltip("The Player turn text object in the Canvas.")]
    public Text m_tPlayerTurnText;

    // public text object for displaying player1s unit numbers.
    [Tooltip("The Player unit number text object in the Canvas.")]
    public Text m_tUnitNumber1Text;

    // public text object for displaying player2s unit numbers.
    [Tooltip("The Player unit number text object in the Canvas.")]
    public Text m_tUnitNumber2Text;

    // public float for how long the lerp is to go for
    [Tooltip("How long do you want the Lerp to play for.")]
    public float m_fLerpTime;

    // public vector 3 for the starting postion of the lerp.
    [Tooltip("The starting postion of the Lerp.")]
    public Vector3 m_v3LerpStartPos;

    // public vector 3 for the ending postion of the lerp.
    [Tooltip("The ending postion of the Lerp.")]
    public Vector3 m_v3LerpEndPos;

    // private float for the current lerp timer.
    private float m_fCurrentLerpTime;

    //--------------------------------------------------------------------------------------
    // initialization.
    //--------------------------------------------------------------------------------------
    void Awake()
    {
        // Start the PlayerTurn UI text as disabled.
        m_tPlayerTurnText.enabled = false;

        // Set all text objects to support richtext.
        m_tPlayerTurnText.supportRichText = true;
        m_tTurnTimerText.supportRichText = true;
        m_tUnitNumber1Text.supportRichText = true;
        m_tUnitNumber2Text.supportRichText = true;
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

        // TODO
        // Solider Numbers for each team.
        // TODO

        // If in the action state display the ticking timer.
        if (StateMachine.GetState() == ETurnManagerStates.ETURN_ACTION)
        {
            // display the ui timer.
            m_tTurnTimerText.text = nTimer;

            // Set the PlayerTurn text element to disabled.
            m_tPlayerTurnText.enabled = false;

            // Reset the lerp.
            m_fCurrentLerpTime = 0f;
        }

        // If it is currently the delay state.
        if (StateMachine.GetState() == ETurnManagerStates.ETURN_DELAY)
        {
            // display the ui timer at 0.
            m_tTurnTimerText.text = "0:00";

            // Set the PlayerTurn text element to enabled.
            m_tPlayerTurnText.enabled = true;

            // Display which players it currently is.
            m_tPlayerTurnText.text = "Player " + TurnManager.m_snCurrentTurn + "'s turn!";

            // Slide the text into the screen
            LerpText(m_tPlayerTurnText);
        }
    }

    //--------------------------------------------------------------------------------------
    // LerpText: Function to lerp a passed in text object.
    //
    // Param:
    //		tTextObject: The text object you want to lerp.
    //--------------------------------------------------------------------------------------
    void LerpText(Text tTextObject)
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
        m_tPlayerTurnText.transform.position = Vector3.Lerp(m_v3LerpStartPos, m_v3LerpEndPos, fProgress);
    }
}
