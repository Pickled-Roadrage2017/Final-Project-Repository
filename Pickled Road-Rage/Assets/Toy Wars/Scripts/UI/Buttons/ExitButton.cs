//--------------------------------------------------------------------------------------
// Purpose: Exit the game.
//
// Description: The ExitButton script is gonna be used for closing the application on 
// a button press. This script is to be attached to a button, after attaching to a button
// drag this script again onto the onClick event (You'll have to create a new onClick) 
// for that button. Once the onClick event is created and script is assigned select the
// QuitGame function.
//
// Author: Thomas Wiltshire.
//--------------------------------------------------------------------------------------

// Using, etc
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//--------------------------------------------------------------------------------------
// ExitButton object. Inheriting from MonoBehaviour. Script for the Exit Button UI.
//--------------------------------------------------------------------------------------
public class ExitButton : MonoBehaviour
{
    // public bool for if this button is a menu button or pause button.
    [LabelOverride("Main Menu Button")] [Tooltip("Is this button used for the main menu?")]
    public bool m_bIsMenuButton = false;

    // public Animator for the teddy bear object.
    [LabelOverride("Teddy Animator")] [Tooltip("The teddy bear object with the animator.")]
    public Animator m_aTeddy;

    // private float timer for delaying the exit.
    private float m_fTimer = 0.0f;

    // bool for if the game is quiting.
    private bool m_bIsQuiting = false;

    //--------------------------------------------------------------------------------------
    // initialization.
    //--------------------------------------------------------------------------------------
    void Awake()
    {
		
	}

    //--------------------------------------------------------------------------------------
    // Update: Function that calls each frame to update game objects.
    //--------------------------------------------------------------------------------------
    void Update()
    {
        // if quiting
        if (m_bIsQuiting)
        {
            // update timer by delta time.
            m_fTimer += Time.deltaTime;

            // if timer is greater than animation time
            if (m_fTimer > 1.7f)
            {
                // Close application.
                Application.Quit();

                // Check that quit is actually being fired.
                Debug.Log("Game is exiting");
            }
        }
    }

    //--------------------------------------------------------------------------------------
    // QuitGame: Close the game window / quit the game.
    //--------------------------------------------------------------------------------------
    public void QuitGame()
    {
        // make sure that it is unpaused.
        PauseManager.m_sbPaused = false;

        // the project is quiting.
        m_bIsQuiting = true;
        
        // if the button is a menu button.
        if (m_bIsMenuButton)
        {
            // Play teddy death
            m_aTeddy.SetBool("HurtAni", true);
        }

        // if the button is not a menu button.
        else if (!m_bIsMenuButton)
        {
            // Close application.
            Application.Quit();

            // Check that quit is actually being fired.
            Debug.Log("Game is exiting");
        }
    }
}
