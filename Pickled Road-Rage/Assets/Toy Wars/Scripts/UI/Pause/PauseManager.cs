//--------------------------------------------------------------------------------------
// Purpose: Manages the pausing of the game.
//
// Description: This script will manage the state of game pause throughout the game.
// Attach to an gameobject.
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
    public static bool m_sbPaused;

    // public gameobject for pause canvas.
    [HideInInspector]
    public GameObject m_gCanvas;

    // public gameobject for gameover canvas.
    [HideInInspector]
    public GameObject m_gGameOver;

    // private array of audio sources.
    AudioSource[] m_aaAudioSources;

    //--------------------------------------------------------------------------------------
    // initialization.
    //--------------------------------------------------------------------------------------
    void Awake()
    {
        // Get the Pause and Gameover canvas.
        m_gCanvas = GameObject.FindGameObjectWithTag("PauseMenu");
        m_gGameOver = GameObject.FindGameObjectWithTag("EndMenu");

        // Check if there is a valid pause canvas.
        if (m_gCanvas != null)
            m_gCanvas.SetActive(false);

        // set the default for pause to false.
        m_sbPaused = false;
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
            m_sbPaused = !m_sbPaused;
        }

        // if paused.
        if (m_sbPaused)
        {
            // Check if there is a valid pause canvas
            if (m_gCanvas != null)
            {
                // Set the pause canvas to true.
                m_gCanvas.SetActive(true);

            }

            // stop game clock.
            Time.timeScale = 0;

            // Get all audio sources.
            m_aaAudioSources = FindObjectsOfType<AudioSource>() as AudioSource[];

            // loop through each audio source.
            foreach (AudioSource audio in m_aaAudioSources)
            {
                // If the audio is not UI tagged than pause audio.
                if (audio.tag != "UI")
                    audio.Pause();
            }
        }

        // if not paused.
        else if (!m_sbPaused)
        {
            // Check if there is a valid pause canvas.
            if (m_gCanvas != null)
            {
                // Set the pause canvas to false.
                m_gCanvas.SetActive(false);
            }

            // start game clock.
            Time.timeScale = 1;

            // Get all audio sources.
            m_aaAudioSources = FindObjectsOfType<AudioSource>() as AudioSource[];

            // loop through each audio source.
            foreach (AudioSource audio in m_aaAudioSources)
            {
                // If the audio is not UI tagged than pause audio.
                if (audio.tag != "UI")
                    audio.UnPause();
            }
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
                m_sbPaused = true;
        }
    }
}
