using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestingLauncher : MonoBehaviour
{
    [Header("Firing Variables")]
    // speed for the moving of the target line
    public float m_fAimSpeed;

    // The furthest the target can go.
    public float m_fMaxLength;

    // Prefab for the Rocket object.
    [LabelOverride("Rocket")][Tooltip("Prefab for instantiating the rockets")]
    public GameObject m_gRocketBlueprint;

    [LabelOverride("Rocket Arc Height")][Tooltip("How high the peak of the rockets arc will be")]
    public float m_fArcHeight;

    // count for how many parts of the line will show
    [LabelOverride("Line Nodes")][Range(1,10)]
    public int m_nLineNodes;

    // float variable passed on for the firing from the Soldier
    [HideInInspector]
    public float m_fCharge = 0;

    // Pool for the Rockets (should always be 1)
    private int m_nPoolsize = 1;

    // An array of Rockets for instantiating rockets
    private GameObject[] m_agRocketList;

    // Boolean for the slider bar to bounce between m_fMinCharge and m_fMaxCharge
    private bool m_bIsAscending;

    // boolean for if the soldier is currently charging up a shot
    private bool m_bChargingShot;

    // where the rocket will be fired towards
    private Vector3 m_v3CastingLine;

    //--------------------------------------------------------------------------------------
    // initialization.
    //--------------------------------------------------------------------------------------
    void Awake()
    {
        m_bChargingShot = false;
        m_bIsAscending = true;
        // initilize rocket list with size
        m_agRocketList = new GameObject[m_nPoolsize];

        // go through each rocket
        for (int i = 0; i < m_nPoolsize; ++i)
        {
            // Instantiate and set active state
            m_agRocketList[i] = Instantiate(m_gRocketBlueprint);
            m_agRocketList[i].SetActive(false);
        }
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
                gRocket.GetComponent<TestingRocket>().m_gSpawnPoint = gameObject;
                gRocket.GetComponent<Rigidbody>().position = gRocket.GetComponent<TestingRocket>().m_gSpawnPoint.transform.position;
                gRocket.GetComponent<Rigidbody>().velocity = transform.forward * m_fCharge;
                gRocket.GetComponent<TestingRocket>().transform.position = gRocket.GetComponent<TestingRocket>().m_gSpawnPoint.transform.position;
                gRocket.GetComponent<TestingRocket>().m_fPower = m_fCharge;
                gRocket.GetComponent<TestingRocket>().m_v3Target = m_v3CastingLine;
                gRocket.GetComponent<TestingRocket>().m_fCurrentActivateTimer = gRocket.GetComponent<TestingRocket>().m_fMaxActivateTimer;
            }
            // launcher is no longer charging a shot
            m_bChargingShot = false;
            m_v3CastingLine = new Vector3(0, 0, 0);
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
                if (m_v3CastingLine.magnitude <= 0.2f)
                {
                    m_bIsAscending = true;
                }
            }

            GetComponent<LineRenderer>().positionCount = m_nLineNodes;
            for (int iIndex = 0; iIndex < m_nLineNodes; iIndex++)
            {
                Vector3 Point = CalculateBezier(transform.position, m_v3CastingLine, (float)iIndex * 0.1f);
                GetComponent<LineRenderer>().SetPosition(iIndex, Point);
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

    public Vector3 CalculateBezier(Vector3 v3Start, Vector3 v3End, float t)
    {
        t = Mathf.Clamp01(t);
        Vector3 v3Control = (v3Start + v3End) / 2;
        v3Control.y += 3.0f;

        Vector3 A1 = Vector3.Lerp(v3Start, v3Control, t);
        Vector3 A2 = Vector3.Lerp(v3Control, v3End, t);
        //Vector3 A3 = Vector3.Lerp(v3Control, v3End, t);

        //Vector3 B1 = Vector3.Lerp(A1, A2, t);
        //Vector3 B2 = Vector3.Lerp(A2, A3, t);

        return Vector3.Lerp(A1, A2, t);
    }
}


