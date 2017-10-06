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
    [Tooltip("Which scene would you like the button to switch to?")]
    public string m_sDestinationScene;

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
        // load scene by destination string.
        SceneManager.LoadScene(m_sDestinationScene);
    }
}
