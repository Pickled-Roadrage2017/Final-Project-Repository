//--------------------------------------------------------------------------------------
// Purpose: Resume the game from pause.
//
// Description: The PauseResume script is gonna be used for closing the pausing the game 
// on a button press. This script is to be attached to a button, after attaching to a
// button drag this script again onto the onClick event (You'll have to create a new 
// onClick) for that button. Once the onClick event is created and script is assigned
// select the Resume function.
//
// Author: Thomas Wiltshire.
//--------------------------------------------------------------------------------------

// Using, etc
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//--------------------------------------------------------------------------------------
// PauseResume object. Inheriting from MonoBehaviour. Script for resuming from pause menu.
//--------------------------------------------------------------------------------------
public class PauseResume : MonoBehaviour
{
    // private bool for if the mouse is pressed.
    private bool m_bMouseUp;

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
        // if the button is pressed.
        if (m_bMouseUp)
        {
            // toggle pause bool.
            PauseManager.m_sbPaused = false;

            // reset mouse up.
            m_bMouseUp = false;
        }
    }

    //--------------------------------------------------------------------------------------
    // Resume: Function to resume the game from the pause menu.
    //--------------------------------------------------------------------------------------
    public void Resume()
    {
        // toggle mouse up bool.
        m_bMouseUp = true;
    }
}
