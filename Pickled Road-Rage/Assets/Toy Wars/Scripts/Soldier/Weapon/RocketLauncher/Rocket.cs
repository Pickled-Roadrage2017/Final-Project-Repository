using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {

    Rigidbody m_rbRocket;

    public float m_fLifespan;

    public float m_fRocketSpeed;
    // Use this for initialization
    void Start ()
    {
		
	}

    private void FixedUpdate()
    {
        // Bullet recieves direction from the player
        m_rbRocket.AddForce(transform.forward * m_fRocketSpeed /** Time.deltaTime*/);
        m_fLifespan -= Time.deltaTime;

        if (m_fLifespan < 0)
        {
            Destroy(gameObject);
        }

    }
}
