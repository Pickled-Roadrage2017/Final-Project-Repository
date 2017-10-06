// Using, etc
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//--------------------------------------------------------------------------------------
// DelayState object. Inheriting from State. Delays the turn switching.
//--------------------------------------------------------------------------------------
public class DelayState : State
{
    // State machine instance
    StateMachine m_sStateMachine;

    //--------------------------------------------------------------------------------------
    // initialization.
    //--------------------------------------------------------------------------------------
    public DelayState(StateMachine sMachine)
    {
        // Set the instance of the statemachine.
        m_sStateMachine = sMachine;
    }

    //--------------------------------------------------------------------------------------
    // Update: Function that calls each frame to update game objects.
    //--------------------------------------------------------------------------------------
    public override void OnUpdate()
    {
        // Update the timer by deltatime.
        TurnManager.m_fTimer -= Time.deltaTime;

        // Once the timer ends.
        if (TurnManager.m_fTimer < 0)
        {
            // Push to the action state
            m_sStateMachine.ChangeState(ETurnManagerStates.ETURN_ACTION);
        }
    }

    //--------------------------------------------------------------------------------------
    // onEnter: A virtual function that runs first when a state is loaded.
    //
    // Param:
    //		sMachine: a reference to the state machine.
    //--------------------------------------------------------------------------------------
    public override void OnEnter()
    {
        // Reset the timer.
        TurnManager.m_fTimer = TurnManager.m_sfStaticDelayLength;




        // Get current players turn.
        //GameObject currentPlayer = m_sStateMachine.m_tTurnManger.GetPlayer(TurnManager.m_snCurrentTurn);

        

        //currentPlayer.SoldierTurnManager();




        // Change Player soldier.
        //GameObject[] player = GameObject.FindGameObjectsWithTag("Player");

        //Player.SoldierTurnManager(); // ASK RICHARD ABOUT THIS BECAUSE I USE THIS SCRIPT ON 2 OBJECTS.
    }

    //--------------------------------------------------------------------------------------
    // onExit: A virtual function that runs on state exit.
    //
    // Param:
    //		sMachine: a reference to the state machine.
    //--------------------------------------------------------------------------------------
    public override void OnExit()
    {
        // Set the delay back to 0
        TurnManager.m_fTimer = 0;
    }
}
