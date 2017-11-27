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

    // Text value to store text component.
    [LabelOverride("Text Object")] [Tooltip("The text object to be lerped across screen.")]
    public Text m_tText;

    // Leave a space in the inspector
    [Space]
    //--------------------------------------------------------------------------------------

    // BACKGROUND //
    //--------------------------------------------------------------------------------------
    // Title for this section of public values.
    [Header("Background Object:")]

    // public gamobject for the background object.
    [LabelOverride("Background Object")] [Tooltip("The background object to be colored for each player when it is their turn.")]
    public GameObject m_gBackground;

    // public color for the player one color.
    [LabelOverride("Player 1 UI Color")] [Tooltip("What color do you want the swap turn UI to be when the turn switches?")]
    public Color m_cPlayer1Color;

    // public color for the player two color.
    [LabelOverride("Player 2 UI Color")] [Tooltip("What color do you want the swap turn UI to be when the turn switches?")]
    public Color m_cPlayer2Color;
    
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

    // Leave a space in the inspector
    [Space]

    // public vector 3 for the starting postion of the lerp.
    [LabelOverride("Start Position")][Tooltip("The starting position of the Lerp.")]
    public Vector3 m_v3StartPos;

    // bool to center the Y of the start postion of the lerp.
    [LabelOverride("Center Start Y")] [Tooltip("Center the Y starting postion to the screen.")]
    public bool m_bCenterStartY;

    // bool to center the X of the start postion of the lerp. 
    [LabelOverride("Center Start X")] [Tooltip("Center the X starting postion to the screen.")]
    public bool m_bCenterStartX;

    // Leave a space in the inspector
    [Space]

    // public vector 3 for the ending postion of the lerp.
    [LabelOverride("End Position")][Tooltip("The ending position of the Lerp.")]
    public Vector3 m_v3EndPos;

    // bool to center the Y of the end postion of the lerp.
    [LabelOverride("Center End Y")] [Tooltip("Center the Y ending postion to the screen.")]
    public bool m_bCenterEndY;

    // bool to center the X of the end postion of the lerp.
    [LabelOverride("Center End X")] [Tooltip("Center the Y ending postion to the screen.")]
    public bool m_bCenterEndX;
    
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
        // Check center y for start bool and center the y if ticked.
        if (m_bCenterStartY)
            m_v3StartPos = new Vector3(m_v3StartPos.x, Screen.height / 2, 0);

        // Check center x for start bool and center the x if ticked.
        if (m_bCenterStartX)
            m_v3StartPos = new Vector3(Screen.width / 2, m_v3StartPos.y, 0);

        // Check center y for end bool and center the y if ticked.
        if (m_bCenterEndY)
            m_v3EndPos = new Vector3(m_v3EndPos.x, Screen.height / 2, 0);

        // Check center x for end bool and center the x if ticked.
        if (m_bCenterEndX)
            m_v3EndPos = new Vector3(Screen.width / 2, m_v3EndPos.y, 0);
    }

    //--------------------------------------------------------------------------------------
    // Update: Function that calls each frame to update game objects.
    //--------------------------------------------------------------------------------------
    void Update()
    {
        // If in the action state display the ticking timer.
        if (StateMachine.GetState() == ETurnManagerStates.ETURN_ACTION)
        {
            // Set the gameobject children to disabled.
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }

            // Reset the lerp.
            m_fCurrentLerpTime = 0f;
        }

        // If it is currently the start state.
        if (StateMachine.GetState() == ETurnManagerStates.ETURN_START)
        {
            // Set the gameobject children to enabled.
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(true);
            }

            // Check if it is player1s turn.
            if (TurnManager.m_snCurrentTurn == 1)
            {
                // Display which players it currently is.
                m_tText.text = "Blue's Turn!";

                // Get the image.
                Image healthImage = m_gBackground.GetComponent<Image>();

                // New color for the player1 color.
                Color newColor = m_cPlayer1Color;

                // Set alpha to 1 and color to the newColor.
                newColor.a = 1;
                healthImage.color = newColor;
            }

            // Check if it is player2s turn. 
            else if (TurnManager.m_snCurrentTurn == 2)
            {
                // Display which players it currently is.
                m_tText.text = "Red's Turn!";

                // Get the image.
                Image healthImage = m_gBackground.GetComponent<Image>();

                // New color for the player2 color.
                Color newColor = m_cPlayer2Color;

                // Set alpha to 1 and color to the newColor.
                newColor.a = 1;
                healthImage.color = newColor;
            }

            // Slide the object into the screen
            Lerp();
        }
    }

    //--------------------------------------------------------------------------------------
    // LerpText: Function to lerp a passed in text object.
    //
    // Param:
    //		tTextObject: The text object you want to lerp.
    //--------------------------------------------------------------------------------------
    void Lerp()
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

        // Lerp the text object and background
        transform.position = Vector3.Lerp(m_v3StartPos, m_v3EndPos, fProgress);
    }
}
