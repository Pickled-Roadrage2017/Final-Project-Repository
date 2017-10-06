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
