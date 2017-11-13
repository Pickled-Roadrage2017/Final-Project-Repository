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

    // public color to apply to soldiers.
    [LabelOverride("Soldier Color")] [Tooltip("The material color of this players soldiers.")]
    public Color m_cSoldierColor;

    // Title for this section of public values.
    [Header("Teddy:")]

    // public gameobject for the Teddy base of this player.
    [LabelOverride("Teddy Object")] [Tooltip("The Teddy Object for this player.")]
    public GameObject m_gTeddyBase;

    // Title for this section of public values.
    [Header("Spawn Points:")]

    // public array for the spawn postion for the players soldiers.
    [LabelOverride("Soldier Spawn Point")] [Tooltip("Spawn postion for the soldier spawn, pass in an empty gameobject.")]
    public GameObject[] m_agSoldierSpawn;

    // public empty gameobject for respawn point.
    [LabelOverride("Respawn Point")] [Tooltip("Spawn postion for a soldier when it respawns, pass in an empty gameobject.")]
    public GameObject m_gRespawnPoint;

    // public empty gameobject for respawn point.
    [LabelOverride("Respawn Rate")] [Tooltip("How long does it take in turns for this players soliders to respawn.")]
    public int m_nRespawnRate;

    // public int for the max amount of times this player can respawn.
    [LabelOverride("Max Respawns")] [Tooltip("The max amount of times this player can respawn soldiers.")]
    public int m_nMaxRespawns;

    // int for keeping track of the respawns.
    [HideInInspector]
    public int m_nMaxRespawnCounter;
    
    // private int for current soldiers turn.
    [HideInInspector]
    public int m_nSoldierTurn;

    // public array of gameobjects for player soldiers.
    private GameObject[] m_agSoldierList;

    // pool size. how many soldiers allowed on screen at once.
    private int m_nPoolSize;

    // int for how many turns until respawn.
    private int m_nRespawnCounter;

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
        // Pool size equel the amount of spawn points.
        m_nPoolSize = m_agSoldierSpawn.Length;

        // initialize soldier list with size.
        m_agSoldierList = new GameObject[m_nPoolSize];

        // Start the solider turn at 1.
        m_nSoldierTurn = 0;

        // Go through each soldier in the pool.
        for (int i = 0; i < m_nPoolSize; ++i)
        {
            // Instantiate and set active state.
            m_agSoldierList[i] = Instantiate(m_gSoldierBlueprint);
            m_agSoldierList[i].SetActive(false);

            // loop through each material on the soliders.
            for (int o = 0; o < m_agSoldierList[i].GetComponent<Renderer>().materials.Length; ++o)
            {
                // Change the color of each material to the m_cSoldierColor.
                m_agSoldierList[i].GetComponent<Renderer>().materials[o].color = m_cSoldierColor;
            }
        }

        // Go through each spawn point in the soldier spawn array.
        for (int i = 0; i < m_agSoldierSpawn.Length; ++i)
        {
            // Allocate some soldiers to the pool.
            GameObject o = AllocateSoldier();

            // Set the postion of the soldiers to the postion of the spawn point.
            o.transform.position = m_agSoldierSpawn[i].transform.position;
        }
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

            // if in the turns action state.
            if (StateMachine.GetState() == ETurnManagerStates.ETURN_ACTION)
            {
                // If not paused then can move and shoot.
                if (!PauseManager.m_bPaused)
                {
                    // Switch soldier weapon on key presses.
                    SwitchWeapon(sCurrentSoldier);

                    // Get the mouse input functions.
                    MouseDown(sCurrentSoldier);
                    bool bMouseHeld = MouseHeld(sCurrentSoldier);
                    MouseUp(sCurrentSoldier);

                    // if the mouse is not held.
                    if (!bMouseHeld)
                    {
                        // Move the soldier.
                        SoldierMovement(sCurrentSoldier);
                    }

                    // if mouse held.
                    else if (bMouseHeld)
                    {
                        // stop the current soldier from moving.
                        sCurrentSoldier.Move(0, 0);
                    }
                }
            }

            // if in the end turn state.
            else if (StateMachine.GetState() == ETurnManagerStates.ETURN_END)
            {
                // stop the current soldier from moving.
                sCurrentSoldier.Move(0, 0);
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

                ++m_nActiveSoldiers;

                // return the bullet.
                return m_agSoldierList[i];
            }
        }

        // if all fail return null.
        return null;
    }

    //--------------------------------------------------------------------------------------
    // RespawnSoldier: Reswpawn a dead soldier to the respawn postion.
    //--------------------------------------------------------------------------------------
    public void RespawnSoldier()
    {
        // if there is dead soliders.
        if (m_nActiveSoldiers < m_agSoldierSpawn.Length)
        {
            // Allocate some soldiers to the pool.
            GameObject o = AllocateSoldier();

            // Set the postion of the soldiers to the postion of the spawn point.
            o.transform.position = m_gRespawnPoint.transform.position;

            // reset the spawn counter.
            m_nRespawnCounter = 0;

            // incriment the mac soldier respawn count.
            m_nMaxRespawnCounter++;
        }
    }

    //--------------------------------------------------------------------------------------
    // RespawnSoldier: Check if the player can respawn a solider.
    //
    // Return:
    //      bool: Return if the solider can spawn or not.
    //--------------------------------------------------------------------------------------
    public bool CheckRespawn()
    {
        // check if the max respawn has been hit yet.
        if (m_nMaxRespawnCounter < m_nMaxRespawns)
        {
            // if there is dead soliders.
            if (m_nActiveSoldiers < m_agSoldierSpawn.Length)
            {
                // Incriment respawn counter each turn.
                m_nRespawnCounter++;
            }

            // if respawn counter is the same as respawn rate.
            if (m_nRespawnCounter == m_nRespawnRate)
            {
                // Respawn a solider at the teddy base.
                RespawnSoldier();

                // Return true if the solider respawns.
                return true;
            }
        }

        // if no respawn return false.
        return false;
    }

    //--------------------------------------------------------------------------------------
    // CheckGameOver: Check if the player has hit a gameover conditon.
    //
    // Return:
    //      bool: Return true or false for if the player has hit a gameover or not.
    //--------------------------------------------------------------------------------------
    public bool CheckGameOver()
    {
        // has max respawns been hit? has the player got soldiers?
        if (m_nMaxRespawnCounter >= m_nMaxRespawns && GetActiveSoldiers() <= 0)
        {
            return true;
        }

        // Check Teddy current health for the player.
        else if (m_gTeddyBase.GetComponent<Teddy>().m_fCurrentHealth <= 0)
        {
            return true;
        }

        // return true if max respawns has not been hit.
        else
        {
            return false;
        }
    }

    //--------------------------------------------------------------------------------------
    // SoldierTurnManager: Function that will manager which soldier the player is able to 
    // use per turn.
    //--------------------------------------------------------------------------------------
    public void SoldierTurnManager()
    {
        // Check if there is active soldier so the while loop doesnt go forever.
        if (GetActiveSoldiers() <= 0)
            return;

        // Loop through the soldier list.
        do
        {
            // Go up one soldiers turn.
            m_nSoldierTurn += 1;

            // Go back to the start of the list
            if (m_nSoldierTurn >= m_agSoldierList.Length)
                m_nSoldierTurn = 0;
        }
        while (m_nSoldierTurn < m_agSoldierList.Length && !m_agSoldierList[m_nSoldierTurn].activeInHierarchy);
    }

    //--------------------------------------------------------------------------------------
    // GetCurrentSoldier: Function that returns a requested soldier.
    //
    // Param:
    //		nSoldierNumber: An index for which soldier is wanted.
    // Return:
    //      GameObject: the soldier that is being returned.
    //--------------------------------------------------------------------------------------
    public GameObject GetSoldier(int nSoldierNumber)
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

        // Update the mpuse face function in soldier.
        sCurrentSoldier.FaceMouse();
    }
    
    //--------------------------------------------------------------------------------------
    // SwitchWeapon: Function for switching the current soldiers weapon.
    //
    // Param:
    //		sCurrentSoldier: A SoldierActor object for which soldier wants to move.
    //--------------------------------------------------------------------------------------
    void SwitchWeapon(SoldierActor sCurrentSoldier)
    {
        // if the 1 key is pressed.
        if (Input.GetButtonDown("SwapRocket"))
        {
            // Switch the current soldiers weapon to RPG.
            sCurrentSoldier.m_eCurrentWeapon = EWeaponType.EWEP_RPG;
        }

        // if the 2 key is pressed.
        else if (Input.GetButtonDown("SwapGrenade") && sCurrentSoldier.m_nGotGrenade > 0)
        {
            // Switch the current soldiers weapon to Grenade. 
            sCurrentSoldier.m_eCurrentWeapon = EWeaponType.EWEP_GRENADE;
        }

        // if the 3 key is pressed.
        else if (Input.GetButtonDown("SwapMini") && sCurrentSoldier.m_nGotMinigun > 0)
        {
            // Switch the current soldiers weapon to MiniGun.
            sCurrentSoldier.m_eCurrentWeapon = EWeaponType.EWEP_MINIGUN;
        }
    }

    //--------------------------------------------------------------------------------------
    // MouseDown: Function for when the mouse is pressed down.
    //
    // Param:
    //		sCurrentSoldier: A SoldierActor object for which soldier is firing.
    // Return:
    //      bool: Returns a bool if the mouse is pressed or not.
    //--------------------------------------------------------------------------------------
    private bool MouseDown(SoldierActor sCurrentSoldier)
    {
        // if the left mouse button is pressed and the timer is greater than 1.
        if (Input.GetButtonDown("Fire1") && TurnManager.m_fTimer > 1)
        {
            // Run the soldier MouseDown fucntion.
            sCurrentSoldier.MouseDown();

            // if mouse is pressed down return bool
            return true;
        }

        // if mouse isnt pressed return false.
        return false;
    }

    //--------------------------------------------------------------------------------------
    // MouseHeld: Function for when the mouse is held down.
    //
    // Param:
    //		sCurrentSoldier: A SoldierActor object for which soldier is firing.
    // Return:
    //      bool: Returns a bool if the mouse is held or not.
    //--------------------------------------------------------------------------------------
    private bool MouseHeld(SoldierActor sCurrentSoldier)
    {
        // if the left mouse button is held down and timer is greater than 1.
        if (Input.GetButton("Fire1") && TurnManager.m_fTimer > 1)
        {
            // Run the soldier MouseHeld function.
            sCurrentSoldier.MouseHeld();

            // if mouse is held return true. 
            return true;
        }

        // if mouse isnt held down return false.
        return false;
    }

    //--------------------------------------------------------------------------------------
    // MouseUp: Function for the when the mouse is released from being down.
    //
    // Param:
    //		sCurrentSoldier: A SoldierActor object for which soldier is firing.
    // Return:
    //      bool: Returns a bool if the mouse is released or not.
    //--------------------------------------------------------------------------------------
    private bool MouseUp(SoldierActor sCurrentSoldier)
    {
        // if the mouse is released or the timer is less than 1.
        if (Input.GetButtonUp("Fire1") || TurnManager.m_fTimer < 1)
        {
            // Set to end turn.
            TurnManager.m_sbEndTurn = true;

            // Run the soldier MouseUp function.
            sCurrentSoldier.MouseUp();

            // if the mouse is released return true.
            return true;
        }

        // if the mouse isnt released return false.
        return false;
    }
}