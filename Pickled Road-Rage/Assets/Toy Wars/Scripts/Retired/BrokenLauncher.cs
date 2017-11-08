//--------------------------------------------------------------------------------------
// Purpose: A weapon for the player to access through a Soldier
//
// Description: Inheriting from MonoBehaviour. Used to instantiate rockets
//
// Author: Callan Davies
//--------------------------------------------------------------------------------------

// Using, etc
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BrokenLauncher : MonoBehaviour
{
    [Header("Firing Variables")]
    // speed for the moving of the target line
    public float m_fAimSpeed;

    // The furthest the target can go.
    public float m_fMaxLength;

    // Prefab for the Rocket object.
    [LabelOverride("Rocket")]
    [Tooltip("Prefab for instantiating the rockets")]
    public GameObject m_gRocketBlueprint;

    [LabelOverride("Rocket Arc Height")]
    [Tooltip("How high the peak of the rockets arc will be")]
    public float m_fArcHeight;

    public float angle;
    // count for how many parts of the line will show
    [LabelOverride("Line Nodes")]
    [Range(1, 10)]
    public int m_nLineNodes = 10;

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

    // line renderer
    public LineRenderer m_lrLine;

    float grav;

    float radAng;

    public float m_fVelocity;


    //--------------------------------------------------------------------------------------
    // initialization.
    //--------------------------------------------------------------------------------------
    void Awake()
    {
        m_bChargingShot = false;
        m_bIsAscending = true;
        // initilize rocket list with size
        m_agRocketList = new GameObject[m_nPoolsize];
        m_lrLine = GetComponent<LineRenderer>();
        // go through each rocket
        for (int i = 0; i < m_nPoolsize; ++i)
        {
            // Instantiate and set active state
            m_agRocketList[i] = Instantiate(m_gRocketBlueprint);
            m_agRocketList[i].SetActive(false);
        }

        grav = Mathf.Abs(Physics.gravity.y);
        m_lrLine.useWorldSpace = false;
    }

    void Start()
    {
        m_fArcHeight = transform.localRotation.x;
    }

    void Update()
    {
        RenderArc();
        //transform.localRotation = Quaternion.Euler(angle, 0, 0);
    }

    //--------------------------------------------------------------------------------------
    // MouseDown: Restes variables of the Launcher when the mouse button is pressed.
    //--------------------------------------------------------------------------------------
    public void MouseDown()
    {
        // Reset CastingLine
        m_v3CastingLine = transform.position + transform.forward * 0.25f;
        m_bIsAscending = true;
    }

    //--------------------------------------------------------------------------------------
    // MouseHeld: Charges the shot until it is no longer called.
    //--------------------------------------------------------------------------------------
    public void MouseHeld()
    {
        // It is charging a shot
        m_bChargingShot = true;
        // If the counter should be ascending and the CastingLine.magnitude is less than MaxLength
        if (m_bIsAscending && Vector3.Distance(m_v3CastingLine, transform.position) <= m_fMaxLength)
        {
            // Increase the CastingLine forwards by AimSpeed
            m_v3CastingLine += transform.forward * m_fAimSpeed * Time.deltaTime;
            //m_fArcHeight += m_fAimSpeed * Time.deltaTime;
            // If the magnitude is higher or equal to MaxLength
            if (Vector3.Distance(m_v3CastingLine, transform.position) >= m_fMaxLength)
            {
                // then it should not be ascening anymore
                m_bIsAscending = false;
            }
        }

        else
        {
            // it musn't be ascending if it got here
            m_bIsAscending = false;
            // Decrease the CastingLine forwards by AimSpeed
            m_v3CastingLine -= transform.forward * m_fAimSpeed * Time.deltaTime;
            //m_fArcHeight -= m_fAimSpeed * Time.deltaTime;
            // if the magnitude is lower than a set number
            if (Vector3.Distance(m_v3CastingLine, transform.position) <= 0.2f)
            {
                // start ascending
                m_bIsAscending = true;
            }
        }
        // Draw the Aiming line
        /*  GetComponent<LineRenderer>().positionCount = m_nLineNodes + 1;
          for (int iIndex = 0; iIndex < m_nLineNodes + 1; iIndex++)
          {
              Vector3 Point = BezierCurve.CalculateBezier(transform.position, m_v3CastingLine, (float)iIndex * 0.1f, m_fArcHeight);
              GetComponent<LineRenderer>().SetPosition(iIndex, Point); */
        //}
    }

    //--------------------------------------------------------------------------------------
    // MouseUp: Spawns a bullet if the Mouse had been held beforehand.
    //--------------------------------------------------------------------------------------
    public void MouseUp()
    {
        if (m_bChargingShot)
        {
            SpawnBullet();
        }
    }

    //--------------------------------------------------------------------------------------
    // SpawnBullet: Resets and Spawns a Rocket.       
    //--------------------------------------------------------------------------------------
    public void SpawnBullet()
    {
        // if this is called while a shot is being charged
        if (m_bChargingShot)
        {
            // Allocate a Rocket from the pool and set it to active
            GameObject gRocket = Allocate();
            // if the rocket is active
            if (gRocket.activeInHierarchy)
            {
                // Reset all of the Rockets variables
                gRocket.GetComponent<Rocket>().m_gSpawnPoint = gameObject;
                gRocket.GetComponent<Rigidbody>().position = gRocket.GetComponent<Rocket>().m_gSpawnPoint.transform.position;
                gRocket.GetComponent<Rocket>().m_fArcHeight = m_fArcHeight;
                gRocket.GetComponent<Rocket>().transform.position = gRocket.GetComponent<Rocket>().m_gSpawnPoint.transform.position;
                gRocket.GetComponent<Rocket>().m_fPower = m_fCharge;
                //m_v3CastingLine.y = 0;
                gRocket.GetComponent<Rocket>().m_v3Target = m_v3CastingLine;
                gRocket.GetComponent<Rocket>().m_fCurrentActivateTimer = gRocket.GetComponent<Rocket>().m_fMaxActivateTimer;
                gRocket.GetComponent<Rocket>().m_fLerpTime = 0.0f;

                gRocket.GetComponent<Rigidbody>().velocity = transform.forward * 10.0f;
            }
            // Reset variables for the firing functions
            m_bChargingShot = false;
            m_v3CastingLine = new Vector3(0, 0, 0);
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

                //return the rockets
                return m_agRocketList[i];
            }
        }
        //if all fail, return null
        return null;
    }


    void RenderArc()
    {
        Ray ray = new Ray(transform.position, Vector3.forward * 100.0f);
        Vector3 offset = new Vector3(0, 0.018f, 0);
        Debug.DrawRay(transform.position, (transform.forward + offset) * 100.0f, Color.red);
        m_lrLine.positionCount = m_nLineNodes + 1;
        m_lrLine.SetPositions(CalcArcArray());
    }

    private Vector3[] CalcArcArray()
    {
        // declare and initilize vector3 array
        Vector3[] v3Trajectory = new Vector3[m_nLineNodes + 1];

        radAng = -Mathf.Deg2Rad * angle;
        // calculate the distance travelled (d = v^2 * Sin(2 * angle)) / g
        float fDis = m_fVelocity * m_fVelocity * Mathf.Sin(2 * radAng) / grav;
        // loop through the number of size in the line renderer
        for (int iIndex = 0; iIndex <= m_nLineNodes; ++iIndex)
        {
            // divide iIndex by number of size of positions in the line renderer
            float t = (float)iIndex / (float)m_nLineNodes;
            float z = t * fDis;
            float y = transform.localPosition.y + (z * Mathf.Tan(radAng) - ((grav * z * z) / (2 * m_fVelocity * m_fVelocity * Mathf.Cos(radAng) * Mathf.Cos(radAng))));

            v3Trajectory[iIndex] = new Vector3(transform.localPosition.x, y, z + transform.localPosition.z);
        }



        return v3Trajectory;
    }

}


