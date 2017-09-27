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
    public float m_fSpawnRadius;

    // Use this for initialization
    void Awake()
    {
        
	}
	
	// Update is called once per frame
	void Update()
    {
       
	}


    public override void Fire()
    {
       
    }
}
