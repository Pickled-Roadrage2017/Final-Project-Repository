// Using, etc
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//--------------------------------------------------------------------------------------
// Player object. Inheriting from MonoBehaviour. Used for controling turns and soldiers.
//--------------------------------------------------------------------------------------
public class Player : MonoBehaviour
{
    // public int for which player this is.
    public int m_nPlayerNumber; // MAKE SURE CANT GO OVER 2. // TOOLTIPS!

    //--------------------------------------------------------------------------------------
    // initialization
    //--------------------------------------------------------------------------------------
    void Awake()
    {

    }

    //--------------------------------------------------------------------------------------
    // Update: Function that calls each frame to update game objects.
    //--------------------------------------------------------------------------------------
    void Update()
    {
        // Check if it is this players turn.
        if (m_nPlayerNumber == TurnManager.m_snCurrentTurn)
        {
            // Soldier Manager.
            SoldierManager();
        }
    }

    //--------------------------------------------------------------------------------------
    // SoldierManager: Function that will manager which soldier the player is able to 
    //                 use per turn.
    //--------------------------------------------------------------------------------------
    void SoldierManager()
    {


    }
}
