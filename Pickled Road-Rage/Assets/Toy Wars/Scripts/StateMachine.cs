// Using, etc
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//--------------------------------------------------------------------------------------
// StateMachine object. Inheriting from MonoBehaviour. // FINISH THIS COMMENT
//--------------------------------------------------------------------------------------
public class StateMachine : MonoBehaviour {

    private List<State> m_StateList;
    private Stack<State> m_CurrentStack;

    //--------------------------------------------------------------------------------------
    // initialization.
    //--------------------------------------------------------------------------------------
    void Start()
    {
		
	}

    //--------------------------------------------------------------------------------------
    // Update: Function that calls each frame to update game objects.
    //--------------------------------------------------------------------------------------
    void Update()
    {
        if (m_CurrentStack.Count <= 0)
            return;

        m_CurrentStack.Peek().Update();
    }

    //--------------------------------------------------------------------------------------
    // PushState: A function to push a state onto the statemachine stack.
    //
    // Param:
    //		nStateIndex: An int index for the state you want to push
    //--------------------------------------------------------------------------------------
    void PushState(int nStateIndex)
    {
        if (m_CurrentStack.Count > 0)
            m_CurrentStack.Peek().onExit(this);

        m_CurrentStack.Push(m_StateList[nStateIndex]);
        m_CurrentStack.Peek().onEnter(this);
    }

    //--------------------------------------------------------------------------------------
    // AddState: A function to render (or "draw") states to the screen.
    //
    // Param:
    //		nStateIndex: An int index for the state.
    //		pState: A pointer to a state.
    //--------------------------------------------------------------------------------------
    void AddState(int nStateIndex, State pState)
    {
        m_StateList.Insert(nStateIndex, pState);
    }

    //--------------------------------------------------------------------------------------
    // PopState: Take the current state off the stack.
    //--------------------------------------------------------------------------------------
    void PopState()
    {
        if (m_CurrentStack.Count > 0)
            m_CurrentStack.Peek().onExit(this);

        m_CurrentStack.Pop();

        if (m_CurrentStack.Count > 0)
            m_CurrentStack.Peek().onEnter(this);
    }

    //--------------------------------------------------------------------------------------
    // PopAll: Take all states off the stack.
    //--------------------------------------------------------------------------------------
    void PopAll()
    {
        if (m_CurrentStack.Count > 0)
            m_CurrentStack.Peek().onExit(this);

        while (m_CurrentStack.Count > 0)
        {
            m_CurrentStack.Pop();
        }
    }
}
