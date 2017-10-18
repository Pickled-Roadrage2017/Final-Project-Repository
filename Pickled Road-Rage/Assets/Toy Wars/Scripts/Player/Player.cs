//--------------------------------------------------------------------------------------
// Purpose: Player functionality.
//
// Description: The Player script is gonna be used for controlling each player when it is
// their turn. This script is to be attached to an empty gameobject for each player.
//
// Author: Thomas Wiltshire.
//--------------------------------------------------------------------------------------

// Using, etc
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//--------------------------------------------------------------------------------------
// Player object. Inheriting from MonoBehaviour. Used for controling turns and soldiers.
//--------------------------------------------------------------------------------------
public class Player : MonoBehaviour
{
    // Title for this section of public values.
    [Header("Player:")]

    // public int for which player this is.
    [LabelOverride("Player Number")] [Range(1, 2)] [Tooltip("Which Player is this between 1 and 2. eg. Player1 or Player2.")]
    public int m_nPlayerNumber;

    // Title for this section of public values.
    [Header("Soldier:")]

    // public gameobject for the soldier prefab.
    [LabelOverride("Soldier Object")] [Tooltip("The prefab for the Soldier object.")]
    public GameObject m_gSoldierBlueprint;

    // pool size. how many soldiers allowed on screen at once.
    [LabelOverride("Pool Size")] [Range(1, 6)] [Tooltip("The max number of soliders allowed in game at once.")]
    public int m_nPoolSize;

    // Title for this section of public values.
    [Header("Teddy:")]

    // public gameobject for the Teddy base of this player.
    [LabelOverride("Teddy Object")] [Tooltip("The Teddy Object for this player.")]
    public GameObject m_gTeddyBase;

    // Title for this section of public values.
    [Header("Temporally Values:")]

    // Temp spawn postion for the player.
    [Tooltip("Spawn postion for the soldier spawn, pass in a gameobject.")]
    public GameObject m_gSoldier1Spawn; // REDO // REDO // REDO // REDO // REDO // REDO // REDO // REDO // REDO // REDO

    // Temp spawn postion for the player.
    [Tooltip("Spawn postion for the soldier spawn, pass in a gameobject.")]
    public GameObject m_gSoldier2Spawn; // REDO // REDO // REDO // REDO // REDO // REDO // REDO // REDO // REDO // REDO

    // private int for current soldiers turn.
    [Tooltip("ONLY PUBLIC FOR DEBUGGING, DONT CHANGE!")]
    public int m_nSoldierTurn; // ONLY PUBLIC FOR DEBUGGING. // ONLY PUBLIC FOR DEBUGGING. // ONLY PUBLIC FOR DEBUGGING.

    // public array of gameobjects for player soldiers.
    private GameObject[] m_agSoldierList;

    // state machine instance for the player.
    private StateMachine m_sStateMachine;

    // turn manager instance for the player.
    private TurnManager m_tTurnManager;

    // An int for how many active soldier there is.
    private int m_nActiveSoldiers;

    // private bool for if the mouse is pressed or not.
    private bool m_bMouseDown = false;

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
        if (m_nPlayerNumber == TurnManager.m_snCurrentTurn)
        {
            // Get the soldier object and script.
            GameObject gCurrentSoldier = GetSoldier(m_nSoldierTurn);
            SoldierActor sCurrentSoldier = gCurrentSoldier.GetComponent<SoldierActor>();

            // new bool for if the player is firing or not.
            bool bFiring = false;

            // Fire the soldier weapon. apply its state of fire to a bool.
            if (StateMachine.GetState() == ETurnManagerStates.ETURN_ACTION)
                bFiring = SoldierFire(sCurrentSoldier);

            // if not firing the soldier.
            if (!bFiring && StateMachine.GetState() == ETurnManagerStates.ETURN_ACTION)
            {
                // Move the soldier.
                SoldierMovement(sCurrentSoldier);
            }
        }

        // Set Active soldier count to 0
        m_nActiveSoldiers = 0;

        // Go through each soldier and count how many are alive.
        for (int i = 0; i < m_agSoldierList.Length; ++i)
        {
            // if the soldier is active.
            if (m_agSoldierList[i].activeInHierarchy)
            {
                // Get soldier script.
                SoldierActor soldier = m_agSoldierList[i].GetComponent<SoldierActor>();

                // soldier is alive.
                if (soldier.m_fCurrentHealth > 0)
                {
                    // increment the active soldier number by 1.
                    m_nActiveSoldiers += 1;
                }
            }
        }
    }

    //--------------------------------------------------------------------------------------
    // SetInstances: Set the instances of objects that are needed from the TurnManager.
    //
    // Param:
    //      tTurnManager: A reference to the TurnManager.
    //      sStateMachine: A reference to the StateMachine.
    //--------------------------------------------------------------------------------------
    public void SetInstances(TurnManager tTurnManager, StateMachine sStateMachine)
    {
        // Set the turn manager instance.
        m_tTurnManager = tTurnManager;

        // Set statemachine instance.
        m_sStateMachine = sStateMachine;
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
    // use per turn.
    //--------------------------------------------------------------------------------------
    public void SoldierTurnManager()
    {
        // TODO // TODO // TODO // TODO
        // reset soldier here before changing to next one.
        // Create a reset soldier function and reset anything that needs to be fresh on turn starting.
        // TODO // TODO // TODO // TODO

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
    //
    // Param:
    //		sCurrentSoldier: A SoldierActor object for which soldier wants to move.
    //--------------------------------------------------------------------------------------
    void SoldierMovement(SoldierActor sCurrentSoldier)
    {
        // Get the horizontal and vertical axis.
        float fMoveHorizontal = Input.GetAxis("Horizontal");
        float fMoveVertical = Input.GetAxis("Vertical");

        // Apply Axis to the current soldier.
        sCurrentSoldier.Move(fMoveHorizontal, fMoveVertical);
    }

    //--------------------------------------------------------------------------------------
    // SoldierFire: Function for the current soldiers firing.
    //
    // Param:
    //		sCurrentSoldier: A SoldierActor object for which soldier is firing.
    // Return:
    //      bool: Return if the soldier is firing or not.
    //--------------------------------------------------------------------------------------
    bool SoldierFire(SoldierActor sCurrentSoldier)
    {
        // new bool for if the mouse is down.
        //bool bMouseDown = false;

        // if the left mouse button is held down.
        if (Input.GetButton("Fire1"))
        {
            // Mouse down is true.
            m_bMouseDown = true;

            // Run the soldier fire script.
            sCurrentSoldier.Fire(m_bMouseDown);

            // Return the mouse down.
            return m_bMouseDown;
        }

        // If the left mouse button is let go.
        else
        {
            // Set to end turn.
            if (m_bMouseDown)
                TurnManager.m_sbEndTurn = true;

            // MouseDown is false.
            m_bMouseDown = false;

            // Run the soldier fire script.
            sCurrentSoldier.Fire(m_bMouseDown);

            // return the mouse down.
            return m_bMouseDown;
        }
    }
}
