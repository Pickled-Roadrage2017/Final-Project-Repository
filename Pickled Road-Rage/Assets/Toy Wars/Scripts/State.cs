// Using, etc
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//--------------------------------------------------------------------------------------
// State object. Inheriting from MonoBehaviour. // FINISH THIS COMMENT
//--------------------------------------------------------------------------------------
public class State : MonoBehaviour {

    //--------------------------------------------------------------------------------------
    // initialization.
    //--------------------------------------------------------------------------------------
    void Start()
    {
		
	}

    //--------------------------------------------------------------------------------------
    // Update: Function that calls each frame to update game objects.
    //--------------------------------------------------------------------------------------
    public virtual void Update()
    {
		
	}

    //--------------------------------------------------------------------------------------
    // onEnter: A virtual function that runs first when a state is loaded.
    //
    // Param:
    //		pMachine: a pointer to StateMachine.
    //--------------------------------------------------------------------------------------
    public virtual void onEnter(StateMachine pMachine)
    {

    }

    //--------------------------------------------------------------------------------------
    // onExit: A virtual function that runs on state exit.
    //
    // Param:
    //		pMachine: a pointer to StateMachine.
    //--------------------------------------------------------------------------------------
    public virtual void onExit(StateMachine pMachine)
    {

    }
}
