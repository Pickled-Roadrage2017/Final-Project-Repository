//--------------------------------------------------------------------------------------
// Purpose: Change the current scene.
//
// Description: The ChangeScene script is gonna be used for changing the current scene 
// on a button press. This script is to be attached to a button, after attaching to a 
// button drag this script again onto the onClick event (You'll have to create a new 
// onClick) for that button. Once the onClick event is created and script is assigned 
// select the LoadLevel function.
//
// Author: Thomas Wiltshire.
//--------------------------------------------------------------------------------------

// Using, etc
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//--------------------------------------------------------------------------------------
// ChangeScene object. Inheriting from MonoBehaviour. Script for changing the scene.
//--------------------------------------------------------------------------------------
public class ChangeScene : MonoBehaviour
{
    // SCENE //
    //--------------------------------------------------------------------------------------
    // Title for this section of public values.
    [Header("Scene:")]

    // public string for the scene to chnage to.
    [LabelOverride("Destination Scene")] [Tooltip("The Scene to be changed to when pushing this button.")]
    public string m_sDestinationScene;

    // Leave a space in the inspector
    [Space]
    //--------------------------------------------------------------------------------------

    // ANIMATION //
    //--------------------------------------------------------------------------------------
    // Title for this section of public values.
    [Header("Animation:")]

    // public bool for if the button triggers an animation.
    [LabelOverride("Animated Transition")] [Tooltip("Does this button trigger a transition animation?")]
    public bool m_bIsAnimated;

    // public float for the time it takes to transition.
    [LabelOverride("Transition Time")] [Tooltip("Time it takes to change scene once the button is clicked.")]
    public float m_fTransitionTime;

    // public Animator for the object that is animating.
    [LabelOverride("Animator Object")] [Tooltip("The object in the scene that has the animation to be triggered.")]
    public Animator m_aAnimator;

    // Leave a space in the inspector
    [Space]
    //--------------------------------------------------------------------------------------

    // PUBLIC HIDDEN //
    //--------------------------------------------------------------------------------------
    // public bool for activating the animation.
    [HideInInspector]
    public bool m_bFancyAni;
    //--------------------------------------------------------------------------------------

    // PRIVATE VALUES //
    //--------------------------------------------------------------------------------------
    // private float for animation timer.
    private float m_fTimer;
    //--------------------------------------------------------------------------------------

    //--------------------------------------------------------------------------------------
    // initialization.
    //--------------------------------------------------------------------------------------
    void Awake()
    {
        // reset animation values.
        m_bFancyAni = false;
        m_fTimer = 0.0f;
    }
    
    //--------------------------------------------------------------------------------------
    // Update: Function that calls each frame to update game objects.
    //--------------------------------------------------------------------------------------
    void Update()
    {
        // if animation has played.
        if (m_bFancyAni)
        {
            // Start timer.
            m_fTimer += Time.deltaTime;

            // if timer is over the transition time.
            if (m_fTimer > m_fTransitionTime)
            {
                // stop animation.
                m_bFancyAni = false;
                m_aAnimator.SetBool("FancyAni", m_bFancyAni);

                // Change scene.
                SceneManager.LoadScene(m_sDestinationScene);
            }

        }
    }

    //--------------------------------------------------------------------------------------
    // LoadLevel: Load the scene that has been set for m_sDestinationScene.
    //--------------------------------------------------------------------------------------
    public void LoadLevel()
    {
        // Make sure the game isnt paused.
        PauseManager.m_sbPaused = false;

        // Check if this button triggers animation.
        if (m_bIsAnimated)
        {
            // set the timer back to 0.
            m_fTimer = 0.0f;

            // play animation.
            m_bFancyAni = true;
            m_aAnimator.SetBool("FancyAni", m_bFancyAni);
        }

        // if this button doesnt trigger animation.
        else if (!m_bIsAnimated)
        {
            // Change scene.
            SceneManager.LoadScene(m_sDestinationScene);
        }
    }
}
