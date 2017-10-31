//--------------------------------------------------------------------------------------
// Purpose: State Machine design pattern.
//
// Author: Thomas Wiltshire.
//--------------------------------------------------------------------------------------

// Using, etc
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//--------------------------------------------------------------------------------------
// Enum ETurnManagerStates. Used for the turnmanager state names.
//--------------------------------------------------------------------------------------
public enum ETurnManagerStates
{
    ETURN_START,
    ETURN_ACTION,
    ETURN_END,
    ETURN_GAMEOVER,
    ETURN_SPAWN
}

//--------------------------------------------------------------------------------------
// StateMachine object. Statemachine for the turn state changing.
//--------------------------------------------------------------------------------------
public class StateMachine
{
    // private dyanmic array of states.
    private List<State> m_asStateList = new List<State>();

    // private stack of states.
    private Stack<State> m_CurrentStack = new Stack<State>();

    // Current state value
    private static ETurnManagerStates m_eCurrentState;

    // TurnManager instance.
    public TurnManager m_tTurnManger;

    //--------------------------------------------------------------------------------------
    // Initialization: Constructor for the StateMachine.
    //
    // Param:
    //      tTurnManager: A reference to the TurnManager.
    //--------------------------------------------------------------------------------------
    public StateMachine(TurnManager tTurnManager)
    {
        // set the turn manager instance.
        m_tTurnManger = tTurnManager;
    }

    //--------------------------------------------------------------------------------------
    // Update: Function that calls each frame to update game objects.
    //--------------------------------------------------------------------------------------
    public void OnUpdate()
    {
        // Check stack count to make sure there is content.
        if (m_CurrentStack.Count <= 0)
            return;

        // Update the top of the stack.
        m_CurrentStack.Peek().OnUpdate();
    }

    //--------------------------------------------------------------------------------------
    // PushState: A function to push a state onto the statemachine stack.
    //
    // Param:
    //		eStateIndex: An index for the state you want to push.
    //--------------------------------------------------------------------------------------
    void PushState(ETurnManagerStates eStateIndex)
    {
        // Check if there is anything on the current stack. 
        if (m_CurrentStack.Count > 0)
        {
            // Run the onExit function for the top stack.
            m_CurrentStack.Peek().OnExit();
        }

        // Push the new state to the stack.
        m_CurrentStack.Push(m_asStateList[(int)eStateIndex]);

        // Run the onEnter function for the state on the top of the stack.
        m_CurrentStack.Peek().OnEnter();
    }

    //--------------------------------------------------------------------------------------
    // AddState: A function to add states to the statemachine array.
    //
    // Param:
    //		eStateIndex: An index for the state.
    //		sState: reference to the state you want to add.
    //--------------------------------------------------------------------------------------
    public void AddState(ETurnManagerStates eStateIndex, State sState)
    {
        // Insert the state into the state array.
        m_asStateList.Insert((int)eStateIndex, sState);
    }

    //--------------------------------------------------------------------------------------
    // PopState: Take the current state off the stack.
    //--------------------------------------------------------------------------------------
    void PopState()
    {
        // Check if there is anything on the current stack. 
        if (m_CurrentStack.Count > 0)
        {
            // Run the onExit function for the top stack.
            m_CurrentStack.Peek().OnExit();
        }

        // Pop the current stack off the stack.
        m_CurrentStack.Pop();

        // Check if there is anything on the current stack. 
        if (m_CurrentStack.Count > 0)
        {
            // run the onEnter function for the top stack.
            m_CurrentStack.Peek().OnEnter();
        }
    }

    //--------------------------------------------------------------------------------------
    // PopAll: Take all states off the stack.
    //--------------------------------------------------------------------------------------
    void PopAll()
    {
        // Check if there is anything on the current stack. 
        if (m_CurrentStack.Count > 0)
        {
            // Run the onExit function for the top stack.
            m_CurrentStack.Peek().OnExit();
        }

        // Pop all that is in the current stack off the stack.
        while (m_CurrentStack.Count > 0)
        {
            m_CurrentStack.Pop();
        }
    }

    //--------------------------------------------------------------------------------------
    // ChangeState: Change the machine state, pop top of the stack and push the new state.
    //
    // Param: 
    //      eStateIndex: An index for the state to change to.
    //--------------------------------------------------------------------------------------
    public void ChangeState(ETurnManagerStates eStateIndex)
    {        
        // Check if there is anything on the current stack. 
        if (m_CurrentStack.Count > 0)
        {
            // Pop the top state
            PopState();
        }

        // Set the current State value to eStateIndex
        m_eCurrentState = eStateIndex;

        // Push the newly seletec state. 
        PushState(eStateIndex);
    }

    //--------------------------------------------------------------------------------------
    // GetState: Return the current state the machine is in.
    //
    // Return: 
    //      ETurnManagerStates: Returns an enum of the current state the machine is in.
    //--------------------------------------------------------------------------------------
    public static ETurnManagerStates GetState()
    {
        // Return what the state is as an enum
        return m_eCurrentState;
    }
}