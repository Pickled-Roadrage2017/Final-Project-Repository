//--------------------------------------------------------------------------------------
// Purpose: Base State class for the state machine.
//
// Author: Thomas Wiltshire.
//--------------------------------------------------------------------------------------

// Using, etc
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//--------------------------------------------------------------------------------------
// State object. Base class for the StateMachine state classes.
//--------------------------------------------------------------------------------------
public class State
{
    // State machine instance
    protected StateMachine m_sStateMachine;

    // Turn manager instance
    protected TurnManager m_tTurnManager;

    //--------------------------------------------------------------------------------------
    // initialization.
    //--------------------------------------------------------------------------------------
    public State(StateMachine sMachine)
    {
        // Set the instance of the statemachine.
        m_sStateMachine = sMachine;

        // Set the instance of the turn manager.
        m_tTurnManager = m_sStateMachine.m_tTurnManger;
    }

    //--------------------------------------------------------------------------------------
    // Update: Function that calls each frame to update game objects.
    //--------------------------------------------------------------------------------------
    public virtual void OnUpdate()
    {
		
	}

    //--------------------------------------------------------------------------------------
    // onEnter: A virtual function that runs first when a state is loaded.
    //--------------------------------------------------------------------------------------
    public virtual void OnEnter()
    {

    }

    //--------------------------------------------------------------------------------------
    // onExit: A virtual function that runs on state exit.
    //--------------------------------------------------------------------------------------
    public virtual void OnExit()
    {

    }
    
    //--------------------------------------------------------------------------------------
    // GetPlayerObject: Get the Player object by index.
    //
    // Param:
    //		nPlayerIndex: The player index.
    // Return:
    //      GameObject: The player game object.
    //--------------------------------------------------------------------------------------
    protected GameObject GetPlayerObject(int nPlayerIndex)
    {
        // Get player object.
        return m_tTurnManager.GetPlayer(nPlayerIndex);
    }

    //--------------------------------------------------------------------------------------
    // GetPlayerScript: Get the Player script by index.
    //
    // Param:
    //		nPlayerIndex: The player index.
    // Return:
    //      Player: a player object with script.
    //--------------------------------------------------------------------------------------
    protected Player GetPlayerScript(int nPlayerIndex)
    {
        // Get the player script from the player gameobject.
        return GetPlayerObject(nPlayerIndex).GetComponent<Player>();
    }

    //--------------------------------------------------------------------------------------
    // GetSoldierObject: Get the Soldier object by player index and soldier index.
    //
    // Param:
    //		nPlayerIndex: The player index.
    //      nSoldierIndex: The soldier index.
    // Return:
    //      GameObject: The soldier game object.
    //--------------------------------------------------------------------------------------
    protected GameObject GetSoldierObject(int nPlayerIndex, int nSoldierIndex)
    {
        // Get the soldier object.
        return GetPlayerScript(nPlayerIndex).GetSoldier(nSoldierIndex);
    }

    //--------------------------------------------------------------------------------------
    // GetSoldierScript: Get the Soldier script by player index and soldier index.
    //
    // Param:
    //		nPlayerIndex: The player index.
    //      nSoldierIndex: The soldier index.
    // Return:
    //      SoldierActor:  a soldier object with script.
    //--------------------------------------------------------------------------------------
    protected SoldierActor GetSoldierScript(int nPlayerIndex, int nSoldierIndex)
    {
        // Get the Soldier script from the soldier gameobject.
        return GetSoldierObject(nPlayerIndex, nSoldierIndex).GetComponent<SoldierActor>();
    }
    
    //--------------------------------------------------------------------------------------
    // GetCurrentPlayerScript: Get the current player script.
    //
    // Return:
    //      Player: a player object with script.
    //--------------------------------------------------------------------------------------
    protected Player GetCurrentPlayerScript()
    {
        // Get current player.
        return GetPlayerScript(TurnManager.m_snCurrentTurn);
    }

    //--------------------------------------------------------------------------------------
    // GetCurrentSoldierScript: Get the current Soldier script.
    //
    // Return:
    //      SoldierActor: a soldier object with script.
    //--------------------------------------------------------------------------------------
    protected SoldierActor GetCurrentSoldierScript()
    {
        // Get the current soldier script
        return GetSoldierScript(TurnManager.m_snCurrentTurn, GetCurrentPlayerScript().m_nSoldierTurn);
    }
}
