using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncher : MonoBehaviour
{
   
    // Prefab for the Rocket object
    [Tooltip("Prefab for instantiating the rockets")]
    public GameObject m_gRocketBlueprint;
    // The object that the rocket should spawn from
    [Tooltip("This gameObjects transform will be the spawn point for the bullet")] 
    public GameObject m_gRocketSpawn;
    // float variable passed on for the firing from the Soldier
    [HideInInspector]
    public float m_fPower;

    // Boolean for the locking of fire if a rocket is still alive
    [HideInInspector]
    public bool m_bRocketAlive;

    [Range(0,-100)]
    [Tooltip("This tilts the RocketLauncher, which in turn makes the rockets arc deeper or shallower")]
    public float m_fRocketXRot;

    // Pool for the Rockets (should always be 1)
    int m_nPoolsize = 1;

    //
    private GameObject[] m_agRocketList;

    void Awake()
    {
        // initilize rocket list with size
        m_agRocketList = new GameObject[m_nPoolsize];

        // go through each rocket
        for (int i = 0; i < m_nPoolsize; ++i)
        {
            // Instantiate and set active state
            m_agRocketList[i] = Instantiate(m_gRocketBlueprint);
            m_agRocketList[i].SetActive(false);
        }
        gameObject.transform.Rotate(m_fRocketXRot, 0, 0);
    }

    void Update()
    {
       
    }

    public void Fire(float fCharge)
    {
        m_fPower = fCharge;
        m_fRocketXRot = -m_fPower;

        
        // re-Initilise all Variables of the rocket
        GameObject gRocket = Allocate();
        if (gRocket.activeInHierarchy)
        {
            m_bRocketAlive = true;
            gRocket.GetComponent<Rocket>().m_gSpawnPoint = gameObject;
            gRocket.GetComponent<Rocket>().m_fCurrentLifespan = gRocket.GetComponent<Rocket>().m_fMaxLifespan;
            gRocket.GetComponent<Rigidbody>().position = gRocket.GetComponent<Rocket>().m_gSpawnPoint.transform.position;
            gRocket.GetComponent<Rocket>().transform.position = gRocket.GetComponent<Rocket>().m_gSpawnPoint.transform.position;
            gRocket.GetComponent<Rocket>().m_fPower = fCharge;
            gRocket.GetComponent<Rocket>().m_fAirDrop = 0;
            gRocket.GetComponent<Rigidbody>().velocity = Vector3.zero;
            gRocket.GetComponent<Rocket>().m_v3MoveDirection = transform.forward;
            gRocket.GetComponent<Rocket>().m_fCurrentActivateTimer = gRocket.GetComponent<Rocket>().m_fActivateTimer;
        }
        
    } 

   GameObject Allocate()
    {
        // Instanciate a new Rocket Prefab
        for (int i = 0; i < m_nPoolsize; ++i)
        {
            //check if active
            if (!m_agRocketList[i].activeInHierarchy)
            {
                //set active state
                m_agRocketList[i].SetActive(true);
                

               
                //return the rocket
                return m_agRocketList[i];
            }
        }
        //if all fail, return null
        return null;
    }
}
