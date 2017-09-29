using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : Weapon {

    Rigidbody m_rbRocket;
    // pointer to the RocketLauncher so it knows where to spawn
    GameObject m_goLauncher;

    // The timer for the rocket to delete
    // NOTE: Only for testing, in game a rocket should 
    // always collide with something, making this obselete
    public float m_fLifespan;

    // Speed that the rocket travels 
    public float m_fRocketSpeed;

    Vector3 m_v3MoveDirection;
    // Use this for initialization
    void Start ()
    {
        m_goLauncher = GameObject.FindGameObjectWithTag("RocketLauncher");
        m_rbRocket = GetComponent<Rigidbody>();
        m_v3MoveDirection = m_goLauncher.transform.forward;
    }

    private void FixedUpdate()
    {
        m_fLifespan -= Time.deltaTime;

        if (m_fLifespan < 0)
        {
            Destroy(gameObject);
        }

        // Bullet recieves direction from the player
        m_rbRocket.velocity = m_v3MoveDirection * m_fRocketSpeed;
        

    }
}
