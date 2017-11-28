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
using UnityEngine.EventSystems;

//--------------------------------------------------------------------------------------
// Player object. Inheriting from MonoBehaviour. Used for controling turns and soldiers.
//--------------------------------------------------------------------------------------
public class Player : MonoBehaviour
{
    // PLAYER //
    //--------------------------------------------------------------------------------------
    // Title for this section of public values.
    [Header("Player:")]

    // public int for which player this is.
    [LabelOverride("Player Number")] [Range(1, 2)] [Tooltip("Which Player is this between 1 and 2. eg. Player1 or Player2.")]
    public int m_nPlayerNumber;


    // public color for the player color.
    [LabelOverride("Player Color")] [Tooltip("What color is this player, red or blue?")]
    public Color m_cPlayerColor;

    // Leave a space in the inspector
    [Space]
    //--------------------------------------------------------------------------------------

    // TEDDY//
    //--------------------------------------------------------------------------------------
    // Title for this section of public values.
    [Header("Teddy:")]

    // public gameobject for the Teddy base of this player.
    [LabelOverride("Teddy Object")] [Tooltip("The Teddy Object for this player.")]
    public GameObject m_gTeddyBase;

    // Leave a space in the inspector
    [Space]
    //--------------------------------------------------------------------------------------

    // SOLDIER //
    //--------------------------------------------------------------------------------------
    // Title for this section of public values.
    [Header("Soldier:")]

    // public gameobject for the soldier prefab.
    [LabelOverride("Soldier Object")] [Tooltip("The prefab for the Soldier object.")]
    public GameObject m_gSoldierBlueprint;

    // public color to apply to soldiers.
    [LabelOverride("Soldier Color")] [Tooltip("The material color of this players soldiers.")]
    public Color m_cSoldierColor;

    // public mesh for when the rpg is selected.
    [LabelOverride("RPG Mesh")] [Tooltip("The mesh to use while the player is using the RPG.")]
    public Mesh m_mRPGSoldierMesh;

    // public mesh for when the grenade is selected.
    [LabelOverride("Grenade Mesh")] [Tooltip("The mesh to use while the player is using the Grenade.")]
    public Mesh m_mGrenadeSoldierMesh;

    // public array of materials for when the rpg is selected.
    [LabelOverride("Material")] [Tooltip("The materials to use while the player is using the RPG.")]
    public Material[] m_amRPGMaterials;
    
    // public array of materials for when the grenade is selected.
    [LabelOverride("Material")] [Tooltip("The materials to use while the player is using the RPG.")]
    public Material[] m_amGrenadeMaterials;

    // Leave a space in the inspector
    [Space]
    //--------------------------------------------------------------------------------------

    // SPAWNING //
    //--------------------------------------------------------------------------------------
    // Title for this section of public values.
    [Header("Soldier Spawn Points:")]

    // public array for the spawn postion for the players soldiers.
    [LabelOverride("Spawn Point")] [Tooltip("Spawn postion for the soldier spawn, pass in an empty gameobject.")]
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

    // Leave a space in the inspector
    [Space]
    //--------------------------------------------------------------------------------------

    // PUBLIC HIDDEN //
    //--------------------------------------------------------------------------------------
    // public array of gameobjects for player soldiers.
    [HideInInspector]
    public GameObject[] m_agSoldierList;

    // int for keeping track of the respawns.
    [HideInInspector]
    public int m_nMaxRespawnCounter;

    // int for how many turns until respawn.
    [HideInInspector]
    public int m_nRespawnCounter;

    // private int for current soldiers turn.
    [HideInInspector]
    public int m_nSoldierTurn;

    // public bool for if the grenade has shot or not.
    [HideInInspector]
    public bool m_bGrenadeShot = false;
    //--------------------------------------------------------------------------------------

    // PRIVATE VALUES //
    //--------------------------------------------------------------------------------------
    // pool size. how many soldiers allowed on screen at once.
    private int m_nPoolSize;
    
    // An int for how many active soldier there is.
    private int m_nActiveSoldiers;
    
    // bool for if the mouse is held or not.
    private bool m_bMouseHeld = false;
    //--------------------------------------------------------------------------------------

    // GETTERS & SETTERS //
    //--------------------------------------------------------------------------------------
    // Active Soldiers getter.
    public int GetActiveSoldiers()
    {
        return m_nActiveSoldiers;
    }
    //--------------------------------------------------------------------------------------

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
                m_agSoldierList[i].GetComponent<Renderer>().materials[o].SetColor("_PlasticColor", m_cSoldierColor);
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
                if (!PauseManager.m_sbPaused)
                {
                    // Update the mouse face function in soldier.
                    sCurrentSoldier.FaceMouse();
                    
                    // if not over a button or the mouse is held
                    if (!EventSystem.current.IsPointerOverGameObject() || m_bMouseHeld)
                    {
                        // Get the mouse input functions.
                        MouseDown(sCurrentSoldier);
                        m_bMouseHeld = MouseHeld(sCurrentSoldier);
                        MouseUp(sCurrentSoldier);
                    }
                    
                    // if the mouse is not held.
                    if (!m_bMouseHeld)
                    {
                        // Switch soldier weapon on key presses.
                        SwitchWeapon(sCurrentSoldier);

                        // Move the soldier.
                        SoldierMovement(sCurrentSoldier);
                    }

                    // if mouse held.
                    else if (m_bMouseHeld)
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
                SoldierActor s = m_agSoldierList[i].GetComponent<SoldierActor>();

                // soldier is alive.
                if (s.m_fCurrentHealth > 0)
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

                // increment the active solider int.
                ++m_nActiveSoldiers;

                // Set health text color.
                m_agSoldierList[i].GetComponentInChildren<SoldierHealthText>().GetComponent<TextMesh>().color = m_cPlayerColor;

                // return the soldier.
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
            o.transform.rotation = m_gRespawnPoint.transform.rotation;

            // Switch mesh, materials, etc to the RPG
            SwitchMesh(o.GetComponent<SoldierActor>(), EWeaponType.EWEP_RPG, m_mRPGSoldierMesh, m_amRPGMaterials);

            // reset the spawn counter.
            m_nRespawnCounter = 0;

            // increment the max soldier respawn counter.
            m_nMaxRespawnCounter++;
        }
    }

    //--------------------------------------------------------------------------------------
    // CheckRespawn: Check if the player can respawn a solider.
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
                // Increment respawn counter each turn.
                m_nRespawnCounter++;
            }

            // if respawn counter is the same as respawn rate.
            if (m_nRespawnCounter == m_nRespawnRate)
            {
                // Return true if the solider respawns.
                return true;
            }
        }

        // if no respawn can be done then return false.
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
            m_gTeddyBase.GetComponent<Teddy>().m_fCurrentHealth = 0;

            return true;
        }

        // Check Teddy current health for the player.
        else if (!m_gTeddyBase.GetComponent<Teddy>().IsAlive())
        {
            return true;
        }

        // else no gamover and exit function.
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
        // Check if there is active soldiers so the while loop doesnt go forever.
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

        // while the soldier turn is less than the amount of soldier and current soldier is not active.
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
    }
    
    //--------------------------------------------------------------------------------------
    // SwitchWeapon: Function for switching the current soldiers weapon.
    //
    // Param:
    //		sCurrentSoldier: A SoldierActor object for which soldier wants to move.
    //--------------------------------------------------------------------------------------
    private void SwitchWeapon(SoldierActor sCurrentSoldier)
    {
        // if the 1 key is pressed.
        if (Input.GetButtonDown("SwapRocket"))
        {
            // Switch mesh, materials, etc to the RPG
            SwitchMesh(sCurrentSoldier, EWeaponType.EWEP_RPG, m_mRPGSoldierMesh, m_amRPGMaterials);
        }

        // if the 2 key is pressed.
        else if (Input.GetButtonDown("SwapGrenade") && sCurrentSoldier.m_nGotGrenade > 0)
        {
            // Switch mesh, materials, etc to the grenade
            SwitchMesh(sCurrentSoldier, EWeaponType.EWEP_GRENADE, m_mGrenadeSoldierMesh, m_amGrenadeMaterials);
        }
    }
    
    //--------------------------------------------------------------------------------------
    // SwitchRPGMouse: Function for switching the current soldiers weapon to the RPG when
    // using the mouse to click the button.
    //--------------------------------------------------------------------------------------
    public void SwitchRPGMouse()
    {
        // if the statemachines is currently in the action state.
        if (StateMachine.GetState() == ETurnManagerStates.ETURN_ACTION)
        {
            // Get the soldier object and script.
            GameObject gCurrentSoldier = GetSoldier(m_nSoldierTurn);
            SoldierActor sCurrentSoldier = gCurrentSoldier.GetComponent<SoldierActor>();

            // Switch mesh, materials, etc to the RPG
            SwitchMesh(sCurrentSoldier, EWeaponType.EWEP_RPG, m_mRPGSoldierMesh, m_amRPGMaterials);
        }
    }

    //--------------------------------------------------------------------------------------
    // SwitchGrenadeMouse: Function for switching the current soldiers weapon to the grenade when
    // using the mouse to click the button.
    //--------------------------------------------------------------------------------------
    public void SwitchGrenadeMouse()
    {
        // if the statemachines is currently in the action state.
        if (StateMachine.GetState() == ETurnManagerStates.ETURN_ACTION)
        {
            // Get the soldier object and script.
            GameObject gCurrentSoldier = GetSoldier(m_nSoldierTurn);
            SoldierActor sCurrentSoldier = gCurrentSoldier.GetComponent<SoldierActor>();

            // Switch mesh, materials, etc to the grenade
            SwitchMesh(sCurrentSoldier, EWeaponType.EWEP_GRENADE, m_mGrenadeSoldierMesh, m_amGrenadeMaterials);
        }
    }
    
    //--------------------------------------------------------------------------------------
    // SwitchMesh: Function to switch the mesh, materials, weapon of the current solider.
    //
    // Param:
    //		sCurrentSoldier: SoldierActor for the current soldier being used.
    //      eWeapon: EWeaponType for the weapon to swap the current soldier to.
    //      mMesh: Mesh to switch the current soldier to.
    //      mMaterials: Materials array for all the current soldier materials to switch.
    //--------------------------------------------------------------------------------------
    void SwitchMesh(SoldierActor sCurrentSoldier, EWeaponType eWeapon, Mesh mMesh, Material[] mMaterials)
    {
        // Switch the current soldiers weapon
        sCurrentSoldier.m_eCurrentWeapon = eWeapon;

        // Change soldier mesh
        sCurrentSoldier.GetComponent<SkinnedMeshRenderer>().sharedMesh = mMesh;

        // Change the color of each material to the m_cSoldierColor.
        sCurrentSoldier.GetComponent<SkinnedMeshRenderer>().materials = mMaterials;

        // loop through each material on the soliders.
        for (int o = 0; o < sCurrentSoldier.GetComponent<SkinnedMeshRenderer>().materials.Length; ++o)
        {
            // Change the color of each material to the m_cSoldierColor.
            sCurrentSoldier.GetComponent<SkinnedMeshRenderer>().materials[o].SetColor("_PlasticColor", m_cSoldierColor);

            // Apply red glow around the current soldier.
            sCurrentSoldier.GetComponent<SkinnedMeshRenderer>().materials[o].SetFloat("_Outline_Width", 0.02f);

            // Set the color of the outline.
            sCurrentSoldier.GetComponent<SkinnedMeshRenderer>().materials[o].SetColor("_Outline_Color", m_cPlayerColor);
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
        if (Input.GetButtonDown("Fire1") && TurnManager.m_sfTimer > 1)
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
        if (Input.GetButton("Fire1") && TurnManager.m_sfTimer > 1)
        {
            // Run the soldier MouseHeld function.
            sCurrentSoldier.MouseHeld();

            // if the grenade was equipped set grenade to shot.
            if (sCurrentSoldier.m_eCurrentWeapon == EWeaponType.EWEP_GRENADE)
                m_bGrenadeShot = true;

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
        if (Input.GetButtonUp("Fire1") || TurnManager.m_sfTimer < 1)
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