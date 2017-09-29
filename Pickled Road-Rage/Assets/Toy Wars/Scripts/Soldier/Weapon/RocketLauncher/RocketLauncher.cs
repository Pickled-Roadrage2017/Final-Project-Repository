using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncher : Weapon
{
    // Prefab for the Rocket object
    public GameObject m_gRocketPrefab;
    // The object that the rocket should spawn from 
    public GameObject m_gRocketSpawn;

    //
    GameObject m_goRocket;

    public float m_fSpawnRadius;

    public float m_fRocketLifespan;

    Rigidbody m_rbRocket;

    // Use this for initialization
    void Awake()
    {
        
    }
	
	// Update is called once per frame
	void Update()
    {
        if (m_fRocketLifespan <= 0)
        {
            Destroy(m_goRocket);
        }
        else
        {
            m_fRocketLifespan -= 1;
        }

	}


    public override void Fire()
    {

        // Instanciate a new Rocket Prefab
        float spawn_angle = Random.Range(0, 2 * Mathf.PI);
        Vector3 spawn_direction = new Vector3(Mathf.Sin(spawn_angle), 0, Mathf.Cos(spawn_angle));
        spawn_direction *= m_fSpawnRadius;
        m_goRocket = Instantiate(m_gRocketPrefab, m_gRocketSpawn.transform.position, Quaternion.identity);
        m_goRocket.transform.Rotate(0, Random.Range(m_gRocketSpawn.transform.rotation.eulerAngles.y, m_gRocketSpawn.transform.rotation.eulerAngles.y), 0);
    } 
}
