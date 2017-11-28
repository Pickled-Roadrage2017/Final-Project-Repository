//--------------------------------------------------------------------------------------
// Purpose: Credit button.
//
// Description: This button is to be used
//
// Author: Thomas Wiltshire.
//--------------------------------------------------------------------------------------

// Using, etc
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//--------------------------------------------------------------------------------------
// ChangeScene object. Inheriting from MonoBehaviour. Script for changing the scene.
//--------------------------------------------------------------------------------------
public class CreditsButton : MonoBehaviour
{
    // public gameobject for the mainmenu group.
    //[LabelOverride("")] [Tooltip("")]
    public GameObject m_gMainMenu;

    // public gameobject for the credits group.
    //[LabelOverride("")] [Tooltip("")]
    public GameObject m_gCredits;

    // public bool for if the buttion is a credit exit button or not.
    //[LabelOverride("")] [Tooltip("")]
    public bool m_bIsExit;

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
    // ButtonClick: 
    //--------------------------------------------------------------------------------------
    public void ButtonClick()
    {
        // Make sure the game isnt paused.
        PauseManager.m_sbPaused = false;

        // If it is a exit button.
        if (m_bIsExit)
        {
            // Turn mainmenu UI on and credit UI off.
            m_gMainMenu.SetActive(true);
            m_gCredits.SetActive(false);
        }

        // if it is not a exit button.
        else if (!m_bIsExit)
        {
            // Turn mainmenu UI off and credit UI on.
            m_gMainMenu.SetActive(false);
            m_gCredits.SetActive(true);
        }
    }
}