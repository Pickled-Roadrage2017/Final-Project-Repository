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
    [Range(1,2)][Tooltip("Specify which Player it is between 1 and 2.")]
    public int m_nPlayerNumber;

    // public int for current soldiers turn.
    [Tooltip("Current soldiers turn, out of all the players soldiers.")]
    public int m_nSoldierTurn; // ASK RICHARD ABOUT SEEING IN INSPECTOR.

    // public array of gameobjects for player soldiers.
    [Tooltip("List of all the soldiers a player has.")]
    public GameObject[] m_agSoldiers;

    // public gameobject for the Teddy base of this player.
    [Tooltip("The Teddy base for this player.")]
    public GameObject m_gTeddyBase;

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
        // TODO.
    }

    //--------------------------------------------------------------------------------------
    // SoldierMovement: Function for the current soldiers movement.
    //--------------------------------------------------------------------------------------
    void SoldierMovement()
    {
        // TODO.
    }

    //--------------------------------------------------------------------------------------
    // SoldierFire: Function for the current soldiers firing.
    //--------------------------------------------------------------------------------------
    void SoldierFire()
    {
        // TODO.
    }
}
