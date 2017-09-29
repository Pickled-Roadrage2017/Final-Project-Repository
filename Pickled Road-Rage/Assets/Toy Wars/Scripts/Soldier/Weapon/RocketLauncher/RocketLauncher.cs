using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncher : MonoBehaviour
{
    // Prefab for the Rocket object
    public GameObject m_gRocketPrefab;
    // The object that the rocket should spawn from 
    public GameObject m_gRocketSpawn;

    public void Fire()
    {
        // Instanciate a new Rocket Prefab
        float spawn_angle = Random.Range(0, 2 * Mathf.PI);
        Vector3 spawn_direction = new Vector3(Mathf.Sin(spawn_angle), 0, Mathf.Cos(spawn_angle));
        GameObject m_goRocket = Instantiate(m_gRocketPrefab, m_gRocketSpawn.transform.position, Quaternion.identity);
        m_goRocket.transform.Rotate(0, Random.Range(m_gRocketSpawn.transform.rotation.eulerAngles.y, m_gRocketSpawn.transform.rotation.eulerAngles.y), 0);
    } 
}
