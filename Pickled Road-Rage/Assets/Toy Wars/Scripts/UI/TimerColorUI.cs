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
using UnityEngine.UI;

//--------------------------------------------------------------------------------------
// TimerColorUI object. Inheriting from MonoBehaviour. 
//--------------------------------------------------------------------------------------
public class TimerColorUI : MonoBehaviour
{
    // 
    //[LabelOverride("")] [Tooltip("")]
    //public Image m_mImage;

    public Color PlayerOneColor;

    public Color PlayerTwoColor;

    //--------------------------------------------------------------------------------------
    // Initialization.
    //--------------------------------------------------------------------------------------
    void Awake()
    {

    }

    //--------------------------------------------------------------------------------------
    // Update: Function that calls each frame to update game objects.
    //--------------------------------------------------------------------------------------
    void Update()
    {
        // 
        if (TurnManager.m_snCurrentTurn == 1)
        {
            Image healthImage = GetComponent<Image>();
            Color newColor = PlayerOneColor;
            newColor.a = 1;
            healthImage.color = newColor;
        }

        // 
        else if (TurnManager.m_snCurrentTurn == 2)
        {
            Image healthImage = GetComponent<Image>();
            Color newColor = PlayerTwoColor;
            newColor.a = 1;
            healthImage.color = newColor;
        }
    }
}
