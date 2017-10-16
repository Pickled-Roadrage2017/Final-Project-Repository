using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//--------------------------------------------------------------------------------------
// Purpose: A projectile that will allow the throwing of soldiers
//
// Description: Inheriting from MonoBehaviour, 
// this will spawn an object that will be fired 
// and then move Teddys currently held Soldier to its landing spot 
//
// Author: Callan Davies
//--------------------------------------------------------------------------------------
public class TeddyProjectile : MonoBehaviour
{
    // How much m_fAirTime gets increased per frame
    [Header("'Gravity' Variables")]
    [LabelOverride("Drop Iterator")][Tooltip("This is added into the downward pull of the arc, increasing this will decrease the time it takes to start falling")]
    public float m_fDropIterator = 0.05f;

    //A timer to stop the rocket from colliding with its Launcher
    [Header("Firing Variables")]
    [LabelOverride("Maximum time for Activation")][Tooltip("Seconds it takes for the projectile to not be within collision range of its Teddy")]
    public float m_fMaxActivateTimer = 2;

    // Gets decreased by m_fDropIterator each Update, then works towards the arc of the soldier/projectile
    [HideInInspector]
    public float m_fAirDrop;

    // Starts at m_fMaxActivateTimer and ticks down to zero, then resetting upon being set Inactive
    [HideInInspector]
    public float m_fCurrentActivateTimer;

    // pointer to the Teddy so it knows where to spawn
    [HideInInspector]
    public GameObject m_gSpawnPoint;

    // The direction the projectile should move
    [HideInInspector]
    public Vector3 m_v3MoveDirection;

    // power passed down by the Teddy when throwing
    [HideInInspector]
    public float m_fPower;

    // the soldier that will be moved to where this projectile lands
    [HideInInspector]
    public GameObject m_gSoldier;

    //To be set to its SpawnPoints Teddy script in the Awake function
    private Teddy m_gTeddy;

    // its own rigidbody
    private Rigidbody m_rbProjectile;


    //--------------------------------------------------------------------------------------
    // initialization.
    //--------------------------------------------------------------------------------------
    void Awake()
    {
        // Gets the code of the spawning RocketLauncher GameObject
        m_gTeddy = GetComponentInParent <Teddy>();
        // Set the rockets power variable to the power variable of the Launcher which was passed down by the Soldier
        m_fPower = m_gTeddy.m_fCharge;
        // Get the rockets Rigidbody
        m_rbProjectile = GetComponent<Rigidbody>();
        // Set the direction for the rocket to go to the forward vector of the RocketLauncher at the time of firing
        m_v3MoveDirection = m_gSpawnPoint.transform.forward;
        // Initilize AirDrop 
        m_fAirDrop = 0;
        // CurrentActivateTimer should start out as equal to the MaxActivateTimer
        m_fCurrentActivateTimer = m_fMaxActivateTimer;
    }

    //--------------------------------------------------------------------------------------
    // Update: Function that calls each frame to update game objects.
    //--------------------------------------------------------------------------------------
    private void Update()
    {
        m_fCurrentActivateTimer -= 1;
        // Decreases the airDrop value by the iterator, this allows the "gravity" of the arc
        m_fAirDrop -= m_fDropIterator;

        // NOTE: Maybe replace this with velocity
        m_rbProjectile.AddForce(m_v3MoveDirection * m_fPower);
        
        // This will start pulling the rocket downwards after it reaches the initial y value of it
        m_rbProjectile.AddForce(new Vector3(0, m_fAirDrop, 0));
    }

    //--------------------------------------------------------------------------------------
    // OnTriggerEnter: When something collides with the Teddy (Is Trigger)
    //
    // Param: other is the other object it is colliding with at call
    //
    //--------------------------------------------------------------------------------------
    private void OnTriggerEnter(Collider other)
    {
        if (m_fCurrentActivateTimer <= 0)
        {
            m_gSoldier.transform.position = transform.position;
            gameObject.SetActive(false);
        }
    }
}
