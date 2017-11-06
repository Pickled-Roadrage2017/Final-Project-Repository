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
    public static bool isPaused;

    //--------------------------------------------------------------------------------------
    // initialization.
    //--------------------------------------------------------------------------------------
    void Awake()
    {
        // set the default for pause to false.
        isPaused = false;
	}

    //--------------------------------------------------------------------------------------
    // Update: Function that calls each frame to update game objects.
    //--------------------------------------------------------------------------------------
    void Update()
    {
        // Check if the pause button was pressed.
        if (Input.GetButtonDown("Pause"))
        {
            // toggle pause bool.
            isPaused = !isPaused;
        }

        // DO PAUSE THINGS HERE!
	}

    //--------------------------------------------------------------------------------------
    // OnApplicationFocus: Returns true or false if the window has focus.
    //--------------------------------------------------------------------------------------
    void OnApplicationFocus(bool hasFocus)
    {
        // toggle pause bool.
        isPaused = !hasFocus;
    }
}
