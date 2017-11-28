//--------------------------------------------------------------------------------------
// Purpose: Credit button.
//
// Description: The CreditsButton script is gonna be used for changing from and too the 
// credits screen. This script is to be attached to a button, after attaching to a 
// button drag the object again onto the onClick event (You'll have to create a new 
// onClick) for that button. Once the onClick event is created and script is assigned 
// select the ButtonClick function.
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
    // OBJECTS //
    //--------------------------------------------------------------------------------------
    // Title for this section of public values.
    [Header("Objects:")]

    // public gameobject for the mainmenu group.
    [LabelOverride("Main Menu Object")] [Tooltip("The main menu object that is to be disabled on credit entering.")]
    public GameObject m_gMainMenu;

    // public gameobject for the credits group.
    [LabelOverride("Credits Menu Object")] [Tooltip("The credits object that is to be disabled on credit exiting.")]
    public GameObject m_gCredits;

    // Leave a space in the inspector
    [Space]
    //--------------------------------------------------------------------------------------

    // BUTTON //
    //--------------------------------------------------------------------------------------
    // Title for this section of public values.
    [Header("Button:")]

    // public bool for if the buttion is a credit exit button or not.
    [LabelOverride("Is Exit Button")] [Tooltip("Is this credit button the enter for credits or exit?")]
    public bool m_bIsExit;

    // Leave a space in the inspector
    [Space]
    //--------------------------------------------------------------------------------------

    // ANIMATION //
    //--------------------------------------------------------------------------------------
    // Title for this section of public values.
    [Header("Animation:")]

    // public Animator for the teddy bear object.
    [LabelOverride("Teddy Animator")] [Tooltip("The teddy bear object with the animator.")]
    public Animator m_aTeddy;

    // Leave a space in the inspector
    [Space]
    //--------------------------------------------------------------------------------------

    // PUBLIC HIDDEN //
    //--------------------------------------------------------------------------------------
    // public bool for teddy animation.
    [HideInInspector]
    public bool m_bTeddyAni;
    //--------------------------------------------------------------------------------------

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

            // set the teddy animation back to idle.
            m_aTeddy.SetBool("CreditsAni", false);
        }

        // if it is not a exit button.
        else if (!m_bIsExit)
        {
            // Turn mainmenu UI off and credit UI on.
            m_gMainMenu.SetActive(false);
            m_gCredits.SetActive(true);

            // Set the teddy animation to credits.
            m_aTeddy.SetBool("CreditsAni", true);
        }
    }
}