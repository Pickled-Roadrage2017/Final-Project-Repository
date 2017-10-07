// Using, etc
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//--------------------------------------------------------------------------------------
// UnitNumbersUI object. Inheriting from MonoBehaviour. Script for the Players 
// unit numbers text object.
//--------------------------------------------------------------------------------------
public class UnitNumbersUI : MonoBehaviour {

    // public text object for displaying player1s unit numbers.
    [Tooltip("The Player unit number text object in the Canvas.")]
    public Text m_tUnitNumber1Text;

    //--------------------------------------------------------------------------------------
    // initialization.
    //--------------------------------------------------------------------------------------
    void Awake()
    {
        // Set text object to support richtext.
        m_tUnitNumber1Text.supportRichText = true;
    }

    //--------------------------------------------------------------------------------------
    // Update: Function that calls each frame to update game objects.
    //--------------------------------------------------------------------------------------
    void Update()
    {
        // TODO
        // Solider Numbers for each team.
        // TODO
    }
}
