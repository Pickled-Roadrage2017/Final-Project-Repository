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
    [Range(1, 2)] [Tooltip("Specify which Player it is between 1 and 2. eg. Player1 or Player2.")]
    public int m_nPlayerNumber;

    // public gameobject for the soldier prefab.
    [Tooltip("The prefab for the Soldier object.")]
    public GameObject m_gSoldierBlueprint;

    // pool size. how many soldiers allowed on screen at once.
    [Range(1, 6)]
    [Tooltip("Specify the max number of soliders allowed in game at once.")]
    public int m_nPoolSize;

    // public gameobject for the Teddy base of this player.
    [Tooltip("The Teddy base for this player.")]
    public GameObject m_gTeddyBase;

    // private int for current soldiers turn.
    public int m_nSoldierTurn; // ONLY PUBLIC FOR DEBUGGING. // ONLY PUBLIC FOR DEBUGGING. // ONLY PUBLIC FOR DEBUGGING.

    // public array of gameobjects for player soldiers.
    private GameObject[] m_agSoldierList;

    //
    [Tooltip("")]
    public GameObject m_gSoldier1Spawn;

    //
    [Tooltip("")]
    public GameObject m_gSoldier2Spawn;

    // An int for how many active soldier there is.
    private int m_nActiveSoldiers;

    // Active Soldiers getter.
    public int GetActiveSoldiers()
    {
        return m_nActiveSoldiers;
    }

    //--------------------------------------------------------------------------------------
    // initialization.
    //--------------------------------------------------------------------------------------
    void Awake()
    {
        // initialize soldier list with size.
        m_agSoldierList = new GameObject[m_nPoolSize];

        // Start the solider turn at 1.
        m_nSoldierTurn = 0;

        // Go through each soldier.
        for (int i = 0; i < m_nPoolSize; ++i)
        {
            // Instantiate and set active state.
            m_agSoldierList[i] = Instantiate(m_gSoldierBlueprint);
            m_agSoldierList[i].SetActive(false);
        }

        // REDO // REDO // REDO // REDO // REDO // REDO // REDO // REDO // REDO // REDO // REDO // REDO
        // Allocate soliders to the pool.
        GameObject p1 = AllocateSoldier(); // Allocating 2 at the start for now until..
        GameObject p2 = AllocateSoldier(); // ..we have a better idea of solider spawning.

        // Spawn at the teddy base.
        p1.transform.position = m_gSoldier1Spawn.transform.position;
        p2.transform.position = m_gSoldier2Spawn.transform.position;
        // REDO // REDO // REDO // REDO // REDO // REDO // REDO // REDO // REDO // REDO // REDO // REDO
    }

    //--------------------------------------------------------------------------------------
    // Update: Function that calls each frame to update game objects.
    //--------------------------------------------------------------------------------------
    void Update()
    {
        // Check if it is this players turn.
        if (m_nPlayerNumber == TurnManager.m_snCurrentTurn) // TODO: There are errors here when soldiers are null.
        {
            // Get the soldier object and script.
            GameObject gCurrentSoldier = GetSoldier(m_nSoldierTurn);
            SoldierActor sCurrentSoldier = gCurrentSoldier.GetComponentInChildren<SoldierActor>(); // TODO: Fix this, GetComponentInChildren is slow.

            // Get the solider update functions
            sCurrentSoldier.Move();
            sCurrentSoldier.FaceMouse();
            sCurrentSoldier.Fire(sCurrentSoldier.m_fCharge);
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

                // increment the active soldier number by 1.
                m_nActiveSoldiers += 1;

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
    public void SoldierTurnManager()
    {
        // Go up one soldiers turn.
        m_nSoldierTurn += 1;

        // Loop through the soldier list.
        while (m_nSoldierTurn < m_agSoldierList.Length && !m_agSoldierList[m_nSoldierTurn].activeInHierarchy)
        {
            m_nSoldierTurn += 1;
        }

        // Go back to the start of the list
        if (m_nSoldierTurn >= m_agSoldierList.Length)
            m_nSoldierTurn = 0;
    }

    //--------------------------------------------------------------------------------------
    // GetCurrentSoldier: Function that returns a requested soldier.
    //
    // Param:
    //		nSoldierNumber: An index for which soldier is wanted.
    // Return:
    //      GameObject: the soldier that is being returned.
    //--------------------------------------------------------------------------------------
    GameObject GetSoldier(int nSoldierNumber)
    {
        // Loop through the soldier list.
        if (nSoldierNumber < m_agSoldierList.Length)
        {
             // return the soldier.
                return m_agSoldierList[nSoldierNumber];
        }

        // else return null.
        return null;
    }

    //--------------------------------------------------------------------------------------
    // SoldierMovement: Function for the current soldiers movement.
    //--------------------------------------------------------------------------------------
    void SoldierMovement()
    {
        
    }

    //--------------------------------------------------------------------------------------
    // SoldierFire: Function for the current soldiers firing.
    //--------------------------------------------------------------------------------------
    void SoldierFire()
    {
        
    }
}
