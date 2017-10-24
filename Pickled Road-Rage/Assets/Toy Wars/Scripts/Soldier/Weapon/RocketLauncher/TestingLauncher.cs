using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestingLauncher : MonoBehaviour
{
    [Header("Firing Variables")]
    // Minimum power for a shot
    [Tooltip("Minimum charge for the Charge, be sure that this matches the 'min value' variable in the Sliders inspector")]
    public float m_fMinCharge = 1f;

    // speed for the moving of the target line
    public float m_fAimSpeed;
    // Float for Max Charge
    [Tooltip("Maximum charge for the Charge, be sure that this matches the 'max value' variable in the Sliders inspector")]
    public float m_fMaxCharge = 2;
    // speed of the charge variable
    public float m_fChargeSpeed;
    // The furthest the target can go.
    public float m_fMaxLength;
    // Prefab for the Rocket object.
    [LabelOverride("Rocket")]
    [Tooltip("Prefab for instantiating the rockets")]
    public GameObject m_gRocketBlueprint;

    [LabelOverride("RocketLauncher Tilt")]
    [Range(0, -100)]
    [Tooltip("This tilts the RocketLauncher, which in turn makes the rockets arc deeper or shallower")]
    public float m_fRocketXRot;

    // float variable passed on for the firing from the Soldier
    [HideInInspector]
    public float m_fCharge = 0;

    // Boolean for the locking of fire if a rocket is still alive
    [HideInInspector]
    public bool m_bRocketAlive;

    // Pool for the Rockets (should always be 1)
    private int m_nPoolsize = 1;

    // An array of Rockets for instantiating rockets
    private GameObject[] m_agRocketList;

    // Boolean for the slider bar to bounce between m_fMinCharge and m_fMaxCharge
    private bool m_bIsAscending;

    // boolean for if the soldier is currently charging up a shot
    private bool m_bChargingShot;

    // where the rocket will be fired towards
    public Vector3 m_v3CastingLine;


    //--------------------------------------------------------------------------------------
    // initialization.
    //--------------------------------------------------------------------------------------
    void Awake()
    {
        Cursor.visible = false;
        m_fChargeSpeed = m_fMinCharge;
        m_fCharge = m_fMinCharge;
        m_bChargingShot = false;
        
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

    public void SpawnBullet()
    {
        if (m_bChargingShot)
        {
            // re-Initilise all Variables of the rocket
            GameObject gRocket = Allocate();
            if (gRocket.activeInHierarchy)
            {
                // Resets all of gRockets current values to their initial values
                m_bRocketAlive = true;
                gRocket.GetComponent<TestingRocket>().m_gSpawnPoint = gameObject;
                gRocket.GetComponent<Rigidbody>().position = gRocket.GetComponent<TestingRocket>().m_gSpawnPoint.transform.position;
                gRocket.GetComponent<TestingRocket>().transform.position = gRocket.GetComponent<TestingRocket>().m_gSpawnPoint.transform.position;
                gRocket.GetComponent<TestingRocket>().m_fPower = m_fCharge;
                gRocket.GetComponent<TestingRocket>().m_v3Target = m_v3CastingLine;
                gRocket.GetComponent<TestingRocket>().m_fCurrentActivateTimer = gRocket.GetComponent<TestingRocket>().m_fMaxActivateTimer;
            }
            // reset the launchers charge value
            m_fCharge = m_fMinCharge;
            // launcher is no longer charging a shot
            m_bChargingShot = false;
        }

    }
    public void Fire(EMouseFiringState eMouseState)
    {
        if(eMouseState == EMouseFiringState.EMOUSE_DOWN)
        {
            m_v3CastingLine = transform.position + transform.forward;
        }

        if (eMouseState == EMouseFiringState.EMOUSE_HELD)
        {
            Debug.DrawLine(transform.position, m_v3CastingLine);
            m_bChargingShot = true;

            if (m_bIsAscending && m_v3CastingLine.magnitude <= m_fMaxLength)
            {
                m_v3CastingLine += transform.forward * m_fAimSpeed * Time.deltaTime;
                
                if (m_v3CastingLine.magnitude >= m_fMaxLength)
                {
                    m_bIsAscending = false;
                }
            }
            else
            {
                m_bIsAscending = false;
                m_v3CastingLine -= transform.forward * m_fAimSpeed * Time.deltaTime;
                if (m_v3CastingLine.magnitude <= m_fMaxLength)
                {
                    m_bIsAscending = true;
                }
            }
        }
        else if (m_bChargingShot)
        {
            SpawnBullet();
        }
    }
    //--------------------------------------------------------------------------------------
    // Allocate: For accessing a rocket that isn't currently active in m_agRocketList 
    //           
    // Returns: A Rocket from m_agRocketList
    //
    //--------------------------------------------------------------------------------------
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


