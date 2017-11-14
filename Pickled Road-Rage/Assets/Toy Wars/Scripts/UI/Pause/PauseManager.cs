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

//--------------------------------------------------------------------------------------
// PauseManager object. Inheriting from MonoBehaviour. Manager for the pausing state.
//--------------------------------------------------------------------------------------
public class PauseManager : MonoBehaviour
{
    // public static bool for if the game is paused.
    [HideInInspector]
    public static bool m_bPaused;

    // public gameobject for pause canvas.
    [HideInInspector]
    public GameObject m_gCanvas;

    // public gameobject for gameover canvas.
    [HideInInspector]
    public GameObject m_gGameOver;

    //--------------------------------------------------------------------------------------
    // initialization.
    //--------------------------------------------------------------------------------------
    void Awake()
    {
        // Get the canvas and set it to inactive.
        m_gCanvas = GameObject.FindGameObjectWithTag("PauseMenu");

        m_gGameOver = GameObject.FindGameObjectWithTag("EndMenu");

        // Check if there is a valid pause canvas.
        if (m_gCanvas != null)
        {
            m_gCanvas.SetActive(false);
        }

        // set the default for pause to false.
        m_bPaused = false;
	}

    //--------------------------------------------------------------------------------------
    // Update: Function that calls each frame to update game objects.
    //--------------------------------------------------------------------------------------
    void Update()
    {
        // Check if the pause button was pressed.
        if (Input.GetButtonDown("Pause") && !m_gGameOver.activeSelf)
        {
            // toggle pause bool.
            m_bPaused = !m_bPaused;
        }

        // if paused.
        if (m_bPaused)
        {
            // Check if there is a valid pause canvas
            if (m_gCanvas != null)
            {
                // Set the pause canvas to true.
                m_gCanvas.SetActive(true);

            }

            // stop game clock.
            Time.timeScale = 0;
        }

        // if not paused.
        else if (!m_bPaused)
        {
            // Check if there is a valid pause canvas.
            if (m_gCanvas != null)
            {
                // Set the pause canvas to false.
                m_gCanvas.SetActive(false);
            }

            // start game clock.
            Time.timeScale = 1;
        }
	}

    //--------------------------------------------------------------------------------------
    // OnApplicationFocus: Returns true or false if the window has focus.
    //--------------------------------------------------------------------------------------
    void OnApplicationFocus(bool hasFocus)
    {
        // make sure that gameover is not on screen.
        if (!m_gGameOver.activeSelf)
        {
            // toggle pause bool.
            if (!hasFocus)
                m_bPaused = true;

        }
    }
}
