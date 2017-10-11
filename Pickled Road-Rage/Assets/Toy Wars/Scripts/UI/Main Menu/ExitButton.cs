//--------------------------------------------------------------------------------------
// Purpose: Exit the game.
//
// Description: The UnitNumberUI script is gonna be used for closing the application on 
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

    }

    //--------------------------------------------------------------------------------------
    // QuitGame: Close the game window / quit the game.
    //--------------------------------------------------------------------------------------
    public void QuitGame()
    {
        // Close application.
        Application.Quit();

        // Check that quit is actually being fired.
        Debug.Log("Game is exiting");
    }
}
