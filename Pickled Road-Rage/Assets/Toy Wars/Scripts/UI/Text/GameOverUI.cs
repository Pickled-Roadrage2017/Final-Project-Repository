//--------------------------------------------------------------------------------------
// Purpose: Change the title text of the gameover canvas.
//
// Description: This script is used to change the title text for the gameover screen
// depending on which player wins the game. Attach to the gameover text object.
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
public class GameOverUI : MonoBehaviour
{
    // public text object for text that is gonna change.
    [LabelOverride("Title Text")] [Tooltip("The text object this is attached to.")]
    public Text m_tTitleText;

    // public player object.
    [LabelOverride("Player1 Object")] [Tooltip("The Player1 object.")]
    public Player m_pPlayer1;

    // public player object.
    [LabelOverride("Player2 Object")] [Tooltip("The Player2 object.")]
    public Player m_pPlayer2;

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
        // Check if player1 has gameover.
        if (m_pPlayer1.CheckGameOver())
        {
            // Change gameover canvas text to display this player won.
            m_tTitleText.text = "Red Wins!";
        }

        // Check if player2 has gameover.
        if (m_pPlayer2.CheckGameOver())
        {
            // Change gameover canvas text to display this player won.
            m_tTitleText.text = "Blue Wins!";
        }

        // Check if both players have gameover
        if (m_pPlayer1.CheckGameOver() && m_pPlayer2.CheckGameOver())
        {
            // Change gameover canvas text to display that it is a draw.
            m_tTitleText.text = "Draw!";
        }
    }
}
