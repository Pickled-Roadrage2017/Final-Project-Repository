using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : Weapon
{
    
    Rigidbody m_rbRocket;
    // pointer to the RocketLauncher so it knows where to spawn
    GameObject m_gLauncher;

    RocketLauncher m_gRocketLauncher;

    // The timer for the rocket to delete
    // NOTE: Should only be as high as the time it would take a rocket to travel across the entire map    
    [Tooltip("Set this to the time it would take a rocket to travel across the entire map")]
    public float m_fLifespan;

    // The direction the Rocket should move
    Vector3 m_v3MoveDirection;

    // Speed at which the rocket goes towards the ground
    [Tooltip("Speed at which the rocket goes towards the ground")]
    public float m_fFallSpeed;


    // Radius for the Area of Effect Explosion that should follow any Collision
    [Tooltip("Radius for the Area of Effect Explosion that should follow any Collision")]

    public float m_fExplosionRadius;

    // The value of how much the damage drops of per unit of distance(metres by default)
    [Tooltip("The value of how much the damage drop of per unit of distance(metres by default)")]
    public int m_fDamageDropoff;

    // Use this for initialization
    void Start()
    {
        m_gLauncher = GameObject.FindGameObjectWithTag("RocketLauncher");
        m_gRocketLauncher = m_gLauncher.GetComponent<RocketLauncher>();
        // Set the rockets power variable to the power variable of the Launcher which was passed down by the Soldier
        m_fPower = m_gRocketLauncher.m_fPower;
        m_rbRocket = GetComponent<Rigidbody>();
        m_v3MoveDirection = m_gLauncher.transform.forward;
    }

    private void FixedUpdate()
    {
        m_fLifespan -= Time.deltaTime;
        if (m_fLifespan < 0)
        {
            Destroy(gameObject);
        }

        // Bullet recieves direction from the player
    //    m_rbRocket.velocity = m_v3MoveDirection * m_fSpeed * Time.deltaTime;
        m_rbRocket.velocity = m_fPower * m_v3MoveDirection;
       // m_rbRocket.AddForce(-Vector3.up * m_fFallSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // If the Rocket hits Terrain (Everything that is not a Soldier or a Tessy)
        if (other.tag == "Terrain")
        {
            //TODO: Explosion Radius should allow you to damage a Soldier without directly hitting them

            Debug.Log("Hit Terrain");
            Destroy(gameObject);
        }
        // If the Rocket Collides with a Soldier, be it friend or foe
        if (other.tag == "Soldier")
        {
            // TODO: Subtract full damage value from others health
            // TODO: Explosion Radius should allow the damaging of more than one soldier, with the damage being less the further from the point of collision
            Debug.Log("Hit Soldier");
            
            Destroy(gameObject);
        }

        // When the Rocket collides with its firer
        if (other.tag == "CurrentSoldier")
        {
            // This should remain as a nothing, CurrentSoldier should never be able to shoot themself.
            // NOTE: CurrentSoldier should be able to damage themself with the Explosion Radius
            Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), other.GetComponent<Collider>());
            Debug.Log("Collided with Firer");
        }

        // If the Rocket collides with a Teddy Bear
        if (other.tag == "TeddyBear")
        {

            Destroy(gameObject);
        }
    }



}
