//--------------------------------------------------------------------------------------
// Purpose: Swap the weapon UI for each player.
//
// Description: This script will swap the current weapon UI when a weapon is changed or
// player is changed. Attach to the parent of the Unselected and selected UI objects.
//
// Author: Thomas Wiltshire.
//--------------------------------------------------------------------------------------

// Using, etc
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//--------------------------------------------------------------------------------------
// WeaponSwapUI object. Inheriting from MonoBehaviour. 
//--------------------------------------------------------------------------------------
public class WeaponSwapUI : MonoBehaviour
{
    // SELECTED //
    //--------------------------------------------------------------------------------------
    // Title for this section of public values.
    [Header("Selected:")]

    // public gameobject for the selected RPG object.
    [LabelOverride("Selected RPG Object")] [Tooltip("The gameobject to use for the selected state for the RPG.")]
    public GameObject m_gSelectedRPG;

    // public gameobject for the selected Grenade object. 
    [LabelOverride("Selected Grenade Object")] [Tooltip("The gameobject to use for the selected state for the Grenade.")]
    public GameObject m_gSelectedGrenade;

    // Leave a space in the inspector
    [Space]
    //--------------------------------------------------------------------------------------

    // UNSELECTED //
    //--------------------------------------------------------------------------------------
    // Title for this section of public values.
    [Header("Unselected:")]

    // public gameobject for the unselected RPG object. 
    [LabelOverride("Unselected RPG Object")] [Tooltip("The gameobject to use for the unselected state for the RPG.")]
    public GameObject m_gUnselectedRPG;

    // public gameobject for the unselected Grenade object.
    [LabelOverride("Unselected Grenade Object")] [Tooltip("The gameobject to use for the unselected state for the Grenade.")]
    public GameObject m_gUnselectedGrenade;

    // Leave a space in the inspector
    [Space]
    //--------------------------------------------------------------------------------------

    // PLAYER //
    //--------------------------------------------------------------------------------------
    // Title for this section of public values.
    [Header("Player:")]

    // public gameobject for the player object.
    [LabelOverride("Player Object")] [Tooltip("The Player object for this UI.")]
    public GameObject m_gPlayer;
    //--------------------------------------------------------------------------------------
    
    //--------------------------------------------------------------------------------------
    // Initialization.
    //--------------------------------------------------------------------------------------
    void Awake()
    {
        // Set the RPG UI selected object to active and unslected to inactive.
        m_gSelectedRPG.SetActive(true);
        m_gUnselectedRPG.SetActive(false);

        // Set the Grenade UI objects to inactive. 
        m_gSelectedGrenade.SetActive(false);
        m_gUnselectedGrenade.SetActive(true);
    }

    //--------------------------------------------------------------------------------------
    // Update: Function that calls each frame to update game objects.
    //--------------------------------------------------------------------------------------
    void Update()
    {
        // Get the current solider.
        GameObject gCurrentSoldier = m_gPlayer.GetComponent<Player>().GetSoldier(m_gPlayer.GetComponent<Player>().m_nSoldierTurn);
        SoldierActor sCurrentSoldier = gCurrentSoldier.GetComponent<SoldierActor>();

        // If it is the players current turn.
        if (m_gPlayer.GetComponent<Player>().m_nPlayerNumber == TurnManager.m_snCurrentTurn)
        {
            // if the RPG is selected.
            if (sCurrentSoldier.m_eCurrentWeapon == EWeaponType.EWEP_RPG)
            {
                // Set RPG selected to active.
                m_gSelectedRPG.SetActive(true);
                m_gUnselectedRPG.SetActive(false);

                // Set grenade unselected to active.
                m_gSelectedGrenade.SetActive(false);
                m_gUnselectedGrenade.SetActive(true);
            }

            // if the grenade is selected and is able to be selected.
            if (sCurrentSoldier.m_eCurrentWeapon == EWeaponType.EWEP_GRENADE)
            {
                // Set grenade selected to active.
                m_gSelectedGrenade.SetActive(true);
                m_gUnselectedGrenade.SetActive(false);

                // Set RPG unselected to active.
                m_gSelectedRPG.SetActive(false);
                m_gUnselectedRPG.SetActive(true);
            }
        }

        // if it isnt the players current turn
        else
        {
            // Set grenade unselected
            m_gSelectedGrenade.SetActive(false);
            m_gUnselectedGrenade.SetActive(false);

            // Set RPG unselected
            m_gSelectedRPG.SetActive(false);
            m_gUnselectedRPG.SetActive(false);
        }
    }
}
