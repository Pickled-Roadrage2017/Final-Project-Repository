//--------------------------------------------------------------------------------------
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
    [LabelOverride("Destination Scene")][Tooltip("The Scene to be changed to when pushing this button.")]
    public string m_sDestinationScene;



    public bool transtionAnimation;

    public bool m_bFancyAni = true;

    float fTimer = 0.0f;


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
    // LoadLevel: Load the scene that has been set for m_sDestinationScene.
    //--------------------------------------------------------------------------------------
    public void LoadLevel()
    {
        fTimer += Time.deltaTime;

        if (transtionAnimation)
        {
            GetComponent<Animator>().SetBool("FancyAni", m_bFancyAni);

            if (fTimer > 2)
                SceneManager.LoadScene(m_sDestinationScene);
        }


        // load scene by destination string.
        //SceneManager.LoadScene(m_sDestinationScene);

    }
}
