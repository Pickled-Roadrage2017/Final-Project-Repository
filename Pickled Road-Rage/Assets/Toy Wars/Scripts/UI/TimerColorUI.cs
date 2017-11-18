//--------------------------------------------------------------------------------------
// Purpose: Change the color of the UI.
//
// Description: This script is used for changing the color of the timer UI depending on
// the player with the current turn. Attach to the timer UI image.
//
// Author: Thomas Wiltshire.
//--------------------------------------------------------------------------------------

// Using, etc
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//--------------------------------------------------------------------------------------
// TimerColorUI object. Inheriting from MonoBehaviour. 
//--------------------------------------------------------------------------------------
public class TimerColorUI : MonoBehaviour
{
    // public color for the player one color.
    [LabelOverride("Player 1 UI Color")] [Tooltip("What color do you want the timer UI to be while this player is taking a turn?")]
    public Color m_cPlayer1Color;

    // public color for the player two color.
    [LabelOverride("Player 2 UI Color")] [Tooltip("What color do you want the timer UI to be while this player is taking a turn?")]
    public Color m_cPlayer2Color;

    //--------------------------------------------------------------------------------------
    // Initialization.
    //--------------------------------------------------------------------------------------
    void Awake()
    {

    }

    //--------------------------------------------------------------------------------------
    // Update: Function that calls each frame to update game objects.
    //--------------------------------------------------------------------------------------
    void Update()
    {
        // Check if it is player1s turn.
        if (TurnManager.m_snCurrentTurn == 1)
        {
            // Get the image.
            Image healthImage = GetComponent<Image>();

            // New color for the player1 color.
            Color newColor = m_cPlayer1Color;

            // Set alpha to 1 and color to the newColor.
            newColor.a = 1;
            healthImage.color = newColor;
        }

        // Check if it is player2s turn. 
        else if (TurnManager.m_snCurrentTurn == 2)
        {
            // Get the image.
            Image healthImage = GetComponent<Image>();

            // New color for the player2 color.
            Color newColor = m_cPlayer2Color;

            // Set alpha to 1 and color to the newColor.
            newColor.a = 1;
            healthImage.color = newColor;
        }
    }
}
