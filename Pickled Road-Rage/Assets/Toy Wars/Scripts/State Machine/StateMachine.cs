// Using, etc
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//--------------------------------------------------------------------------------------
// Enum ETurnManagerStates. Used for the turnmanager state names.
//--------------------------------------------------------------------------------------
public enum ETurnManagerStates
{
    ETURN_DELAY,
    ETURN_ACTION 
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

    //--------------------------------------------------------------------------------------
    // initialization.
    //--------------------------------------------------------------------------------------
    public StateMachine(TurnManager tTurnManager)
    {
		
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
        m_CurrentStack.Peek().OnUpdate(this);
    }

    //--------------------------------------------------------------------------------------
    // PushState: A function to push a state onto the statemachine stack.
    //
    // Param:
    //		nStateIndex: An int index for the state you want to push.
    //--------------------------------------------------------------------------------------
    void PushState(ETurnManagerStates eStateIndex)
    {
        // Check if there is anything on the current stack. 
        if (m_CurrentStack.Count > 0)
        {
            // Run the onExit function for the top stack.
            m_CurrentStack.Peek().OnExit(this);
        }

        // Push the new state to the stack.
        m_CurrentStack.Push(m_asStateList[(int)eStateIndex]);

        // Run the onEnter function for the state on the top of the stack.
        m_CurrentStack.Peek().OnEnter(this);
    }

    //--------------------------------------------------------------------------------------
    // AddState: A function to add states to the statemachine array.
    //
    // Param:
    //		nStateIndex: An int index for the state.
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
            m_CurrentStack.Peek().OnExit(this);
        }

        // Pop the current stack off the stack.
        m_CurrentStack.Pop();

        // Check if there is anything on the current stack. 
        if (m_CurrentStack.Count > 0)
        {
            // run the onEnter function for the top stack.
            m_CurrentStack.Peek().OnEnter(this);
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
            m_CurrentStack.Peek().OnExit(this);
        }

        // Pop all that is in the current stack off the stack.
        while (m_CurrentStack.Count > 0)
        {
            m_CurrentStack.Pop();
        }
    }

    //--------------------------------------------------------------------------------------
    // ChangeState: // FINISH THIS COMMENT
    //--------------------------------------------------------------------------------------
    public void ChangeState(ETurnManagerStates eStateIndex)
    {        
        // Check if there is anything on the current stack. 
        if (m_CurrentStack.Count > 0)
        {
            // Pop the top state
            PopState();
        }        
        
        // Push the newly seletec state. 
        PushState(eStateIndex);
    }





    // RETURN THE STATE FOR CHECKING THE STATE ON THE UI SCRIPT.
    //public ETurnManagerStates GetState()
    //{
    //    //return m_CurrentStack.Peek();
    //}
}
