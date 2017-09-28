// Using, etc
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//--------------------------------------------------------------------------------------
// Teddy object. Inheriting from MonoBehaviour. Used for controling the Teddy base.
//--------------------------------------------------------------------------------------
public class Teddy : MonoBehaviour
{
    // public int for which player this is.
    [Tooltip("Teddy bear base health.")]
    public float m_fHealth;

    // public float for the teddy throwing charge.
    [Tooltip("The charge of the Teddy throwing mechanic.")]
    public float m_fCharge;

    //--------------------------------------------------------------------------------------
    // initialization.
    //--------------------------------------------------------------------------------------
    void Start()
    {
		// TODO.
	}

    //--------------------------------------------------------------------------------------
    // Update: Function that calls each frame to update game objects.
    //--------------------------------------------------------------------------------------
    void Update()
    {
		// TODO.
	}

    //--------------------------------------------------------------------------------------
    // Throw: Function for throwing a solider across the map.
    //--------------------------------------------------------------------------------------
    void Throw()
    {
        // TODO.
    }
}
