﻿//--------------------------------------------------------------------------------------
// Purpose: Display how many turns till solider respawn.
//
// Description: The RespawnTurnsUI script is gonna be used for displaying the current 
// players ammount of turns till a solider respawn. Attach to a text object and pass in
// the player for which you want to display the respawn turns for.
//
// Author: Thomas Wiltshire.
//--------------------------------------------------------------------------------------

// Using, etc
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//--------------------------------------------------------------------------------------
// RespawnTurnsUI object. Inheriting from MonoBehaviour. Show the number of turns for 
// respawn.
//--------------------------------------------------------------------------------------
public class RespawnTurnsUI : MonoBehaviour
{
    // public player object.
    [LabelOverride("Player Object")] [Tooltip("The player object for which this respawn counter belongs to?")]
    public Player m_pPlayer;

    // private text object for the text component.
    private Text m_tRespawnTurns;

    // int for the respawn count.
    private int m_nCount = 0;

    // int to be used for displaying the counter as text.
    private int m_nCountText = 0;

    //--------------------------------------------------------------------------------------
    // initialization.
    //--------------------------------------------------------------------------------------
    void Awake()
    {
        // Get text component.
        m_tRespawnTurns = GetComponent<Text>();
    }

    //--------------------------------------------------------------------------------------
    // Update: Function that calls each frame to update game objects.
    //--------------------------------------------------------------------------------------
    void Update()
    {
        // Get player script.
        Player pPlayer = m_pPlayer.GetComponent<Player>();

        // Count equals the RespawnCounter.
        m_nCount = pPlayer.m_nRespawnCounter;
        
        // If the count is 0 and there is dead soldier than the text counter is 2.
        if (m_nCount == 0 && pPlayer.GetActiveSoldiers() < pPlayer.m_agSoldierSpawn.Length)
            m_nCountText = 2;

        // If the count is 1 then the text counter is 1.
        if (m_nCount == 1)
            m_nCountText = 1;

        // If the count is 3, max respawns has been met or there are no dead soliders than the text counter is 0.
        if (m_nCount == 3 || pPlayer.m_nMaxRespawnCounter == pPlayer.m_nMaxRespawns || pPlayer.GetActiveSoldiers() > pPlayer.m_agSoldierSpawn.Length)
            m_nCountText = 0;

        // Display the respawn turn timer.
        m_tRespawnTurns.text = string.Format("{0}", m_nCountText);
    }
}