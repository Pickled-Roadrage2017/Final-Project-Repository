﻿//--------------------------------------------------------------------------------------
// Purpose: Change the current scene.
//
// Description: The UnitNumberUI script is gonna be used for changing the current scene 
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
    // public string for the scene to chnage to.
    [LabelOverride("Destination Scene")] [Tooltip("The Scene to be changed to when pushing this button.")]
    public string m_sDestinationScene;



    public bool m_bIsAnimated;

    [HideInInspector]
    public bool m_bFancyAni;

    float m_fTimer;

    public float m_fTranstionTime;

    public Animator m_aAnimator;



    //--------------------------------------------------------------------------------------
    // initialization.
    //--------------------------------------------------------------------------------------
    void Awake()
    {
        m_bFancyAni = false;
        m_fTimer = 0.0f;
    }

    //--------------------------------------------------------------------------------------
    // Update: Function that calls each frame to update game objects.
    //--------------------------------------------------------------------------------------
    void Update()
    {
        if (m_bFancyAni)
        {
            m_fTimer += Time.deltaTime;

            if (m_fTimer > m_fTranstionTime) // make 2 a public float.
            {
                SceneManager.LoadScene(m_sDestinationScene);
                //m_bFancyAni = false;
                //m_aAnimator.SetBool("FancyAni", m_bFancyAni);
            }

        }
    }

    //--------------------------------------------------------------------------------------
    // LoadLevel: Load the scene that has been set for m_sDestinationScene.
    //--------------------------------------------------------------------------------------
    public void LoadLevel()
    {
        if (m_bIsAnimated)
        {
            m_bFancyAni = true;
            m_aAnimator.SetBool("FancyAni", m_bFancyAni);
        }

        else if (!m_bIsAnimated)
        {
            SceneManager.LoadScene(m_sDestinationScene);
        }
    }
}
