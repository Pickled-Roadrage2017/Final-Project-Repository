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

    // public gameobject for the soldier prefab.
    [Tooltip("The prefab for the Soldier object.")]
    public GameObject m_gSoldierBlueprint;

    // public array of gameobjects for player soldiers.
    private GameObject[] m_agSoldierList;

    // pool size. how many soldiers allowed on screen at once.
    [Range(1, 6)][Tooltip("Specify the max number of solider on each time at once.")]
    public int m_nPoolSize;

    // public gameobject for the Teddy base of this player.
    [Tooltip("The Teddy base for this player.")]
    public GameObject m_gTeddyBase;

    // Current Soliders with a turn.
    private GameObject m_gCurrentSoldier;

    //--------------------------------------------------------------------------------------
    // initialization.
    //--------------------------------------------------------------------------------------
    void Awake()
    {
        // initialize bullet list with size.
        m_agSoldierList = new GameObject[m_nPoolSize];

        // Go through each bullet.
        for (int i = 0; i < m_nPoolSize; ++i)
        {
            // Instantiate and set active state.
            m_agSoldierList[i] = Instantiate(m_gSoldierBlueprint);
            m_agSoldierList[i].SetActive(false);
        }

        // REDO // REDO // REDO
        // Allocate soliders to the pool.
        GameObject gSoldier1 = AllocateSoldier(); // Allocating 2 at the start for now until..
        GameObject gSoldier2 = AllocateSoldier(); // ..we have a better idea of solider spawning.

        // Spawn at the teddy base.
        gSoldier1.transform.position = m_gTeddyBase.transform.position;
        gSoldier2.transform.position = m_gTeddyBase.transform.position;
        // REDO // REDO // REDO
    }

    //--------------------------------------------------------------------------------------
    // Update: Function that calls each frame to update game objects.
    //--------------------------------------------------------------------------------------
    void Update()
    {
        // Check if it is this players turn.
        if (m_nPlayerNumber == TurnManager.m_snCurrentTurn)
        {
            // Update the soldier.
            m_nSoldierTurn = SoldierTurnManager(); // ASK RICHARD.
        }
    }

    //--------------------------------------------------------------------------------------
    // AllocateSoldier: Allocate soldiers to the pool.
    //
    // Return:
    //      GameObject: Current gameobject in the pool.
    //--------------------------------------------------------------------------------------
    GameObject AllocateSoldier()
    {
        // For each in the pool.
        for (int i = 0; i < m_nPoolSize; ++i)
        {
            // Check if active.
            if (!m_agSoldierList[i].activeInHierarchy)
            {
                // Set active state.
                m_agSoldierList[i].SetActive(true);

                // return the bullet.
                return m_agSoldierList[i];
            }
        }

        // if all fail return null.
        return null;
    }

    //--------------------------------------------------------------------------------------
    // SoldierTurnManager: Function that will manager which soldier the player is able to 
    //                 use per turn.
    //--------------------------------------------------------------------------------------
    int SoldierTurnManager()
    {
        // new int turn.
        int nTurn = 0;

        // Go up one soldiers turn.
        nTurn += 1;

        // Go back to the start of the list
        if (nTurn > m_agSoldierList.Length)
            nTurn = 1;

        // return turn number.
        return nTurn;
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
