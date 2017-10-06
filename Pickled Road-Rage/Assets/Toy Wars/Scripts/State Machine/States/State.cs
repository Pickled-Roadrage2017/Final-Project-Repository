﻿// Using, etc
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//--------------------------------------------------------------------------------------
// State object. Base class for the StateMachine state classes.
//--------------------------------------------------------------------------------------
public class State
{
    //--------------------------------------------------------------------------------------
    // initialization.
    //--------------------------------------------------------------------------------------
    public State()
    {
		
	}

    //--------------------------------------------------------------------------------------
    // Update: Function that calls each frame to update game objects.
    //--------------------------------------------------------------------------------------
    public virtual void OnUpdate()
    {
		
	}

    //--------------------------------------------------------------------------------------
    // onEnter: A virtual function that runs first when a state is loaded.
    //
    // Param:
    //		sMachine: a reference to the state machine.
    //--------------------------------------------------------------------------------------
    public virtual void OnEnter()
    {

    }

    //--------------------------------------------------------------------------------------
    // onExit: A virtual function that runs on state exit.
    //
    // Param:
    //		sMachine: a reference to the state machine.
    //--------------------------------------------------------------------------------------
    public virtual void OnExit()
    {

    }
}
