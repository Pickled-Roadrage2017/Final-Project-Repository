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
            PauseManager.m_bPaused = false;

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
