﻿////--------------------------------------------------------------------------------------
//// Purpose: A weapon for the player to access through a Soldier
////
//// Description: Inheriting from MonoBehaviour. Used to instantiate Grenades
////
//// Author: Callan Davies
////--------------------------------------------------------------------------------------

//// Using, etc
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//public class OldGrenadeLauncher : MonoBehaviour
//{
//    [Header("Firing Variables")]
//    // speed for the moving of the target line
//    public float m_fAimSpeed;

//    // The furthest the target can go.
//    public float m_fMaxLength;

//    // Prefab for the Grenade object.
//    [LabelOverride("Grenade")]
//    [Tooltip("Prefab for instantiating the grenades")]
//    public GameObject m_gGrenadePrefab;

//    [LabelOverride("Grenade Arc Height")]
//    [Tooltip("How high the peak of the grenade arc will be")]
//    public float m_fArcHeight;

//    // count for how many parts of the line will show
//    [LabelOverride("Line Nodes")]
//    [Range(1, 10)]
//    public int m_nLineNodes;

//    // float variable passed on for the firing from the Soldier
//    [HideInInspector]
//    public float m_fCharge = 0;

//    // Pool for the Rockets (should always be 1)
//    private int m_nPoolsize = 1;

//    // An array of Rockets for instantiating rockets
//    private GameObject[] m_agGrenadeList;

//    // Boolean for the slider bar to bounce between m_fMinCharge and m_fMaxCharge
//    private bool m_bIsAscending;

//    // boolean for if the soldier is currently charging up a throw
//    private bool m_bChargingShot;

//    // where the rocket will be fired towards
//    private Vector3 m_v3CastingLine;

//    //--------------------------------------------------------------------------------------
//    // initialization.
//    //--------------------------------------------------------------------------------------
//    void Awake()
//    {
//        m_bChargingShot = false;
//        m_bIsAscending = true;
//        // initilize grenade list with size
//        m_agGrenadeList = new GameObject[m_nPoolsize];

//        // go through each grenade
//        for (int i = 0; i < m_nPoolsize; ++i)
//        {
//            // Instantiate and set active state
//            m_agGrenadeList[i] = Instantiate(m_gGrenadePrefab);
//            m_agGrenadeList[i].SetActive(false);
//        }
//    }


//    void Update()
//    {

//    }

//    //--------------------------------------------------------------------------------------
//    // MouseDown: Restes variables of the Launcher when the mouse button is pressed.
//    //--------------------------------------------------------------------------------------
//    public void MouseDown()
//    {
//        // Reset CastingLine
//        m_v3CastingLine = transform.position + transform.forward * 0.25f;
//        m_bIsAscending = true;
//    }

//    //--------------------------------------------------------------------------------------
//    // MouseHeld: Charges the shot until it is no longer called.
//    //--------------------------------------------------------------------------------------
//    public void MouseHeld()
//    {
//        // It is charging a shot
//        m_bChargingShot = true;
//        // If the counter should be ascending and the CastingLine.magnitude is less than MaxLength
//        if (m_bIsAscending && Vector3.Distance(m_v3CastingLine, transform.position) <= m_fMaxLength)
//        {
//            // Increase the CastingLine forwards by AimSpeed
//            m_v3CastingLine += transform.forward * m_fAimSpeed * Time.deltaTime;
//            m_fArcHeight += m_fAimSpeed * Time.deltaTime;
//            // If the magnitude is higher or equal to MaxLength
//            if (Vector3.Distance(m_v3CastingLine, transform.position) >= m_fMaxLength)
//            {
//                // then it should not be ascening anymore
//                m_bIsAscending = false;
//            }
//        }
//        else
//        {
//            // it musn't be ascending if it got here
//            m_bIsAscending = false;
//            // Decrease the CastingLine forwards by AimSpeed
//            m_v3CastingLine -= transform.forward * m_fAimSpeed * Time.deltaTime;
//            m_fArcHeight -= m_fAimSpeed * Time.deltaTime;
//            // if the magnitude is lower than a set number
//            if (Vector3.Distance(m_v3CastingLine, transform.position) <= 0.2f)
//            {
//                // start ascending
//                m_bIsAscending = true;
//            }
//        }
//        // Draw the Aiming line
//        GetComponent<LineRenderer>().positionCount = m_nLineNodes;
//        for (int iIndex = 0; iIndex < m_nLineNodes; iIndex++)
//        {
//            Vector3 Point = BezierCurve.CalculateBezier(transform.position, m_v3CastingLine, (float)iIndex * 0.1f, m_fArcHeight);
//            GetComponent<LineRenderer>().SetPosition(iIndex, Point);
//        }
//    }

//    //--------------------------------------------------------------------------------------
//    // MouseUp: Spawns a bullet if the Mouse had been held beforehand.
//    //--------------------------------------------------------------------------------------
//    public void MouseUp()
//    {
//        if (m_bChargingShot)
//        {
//            SpawnBullet();
//        }
//    }

//    //--------------------------------------------------------------------------------------
//    // SpawnBullet: Resets and Spawns a Grenade.       
//    //--------------------------------------------------------------------------------------
//    public void SpawnBullet()
//    {
//        // if this is called while a shot is being charged
//        if (m_bChargingShot)
//        {
//            // Allocate a Rocket from the pool and set it to active
//            GameObject gGrenade = Allocate();
//            // if the rocket is active
//            if (gGrenade.activeInHierarchy)
//            {
//                // Reset all of the Rockets variables
//                gGrenade.GetComponent<OldGrenade>().m_gSpawnPoint = gameObject;
//                gGrenade.GetComponent<Rigidbody>().position = gGrenade.GetComponent<OldGrenade>().m_gSpawnPoint.transform.position;
//                gGrenade.GetComponent<OldGrenade>().m_fArcHeight = m_fArcHeight;
//                gGrenade.GetComponent<OldGrenade>().transform.position = gGrenade.GetComponent<OldGrenade>().m_gSpawnPoint.transform.position;
//                gGrenade.GetComponent<OldGrenade>().m_fPower = m_fCharge;
//                m_v3CastingLine.y = 0;
//                gGrenade.GetComponent<OldGrenade>().m_v3Target = m_v3CastingLine;
//                gGrenade.GetComponent<OldGrenade>().m_fCurrentActivateTimer = gGrenade.GetComponent<OldGrenade>().m_fMaxActivateTimer;
//                gGrenade.GetComponent<OldGrenade>().m_fLerpTime = 0.0f;
//            }
//            // Reset variables for the firing functions
//            m_bChargingShot = false;
//            m_v3CastingLine = new Vector3(0, 0, 0);
//        }

//    }
//    //--------------------------------------------------------------------------------------
//    // Allocate: For accessing a Grenade that isn't currently active in m_agGrenadeList 
//    //           
//    // Returns: A grenade from m_agGrenadeList
//    //
//    //--------------------------------------------------------------------------------------
//    GameObject Allocate()
//    {
//        // Instanciate a new Rocket Prefab
//        for (int i = 0; i < m_nPoolsize; ++i)
//        {
//            //check if active
//            if (!m_agGrenadeList[i].activeInHierarchy)
//            {
//                //set active state
//                m_agGrenadeList[i].SetActive(true);

//                //return the rocket
//                return m_agGrenadeList[i];
//            }
//        }
//        //if all fail, return null
//        return null;
//    }
//}
