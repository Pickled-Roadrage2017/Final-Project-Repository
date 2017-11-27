//--------------------------------------------------------------------------------------
// Purpose: A weapon for the player to access through a Soldier
//
// Description: Inheriting from MonoBehaviour. Used to instantiate Grenades
//
// Author: Callan Davies
//--------------------------------------------------------------------------------------

// Using, etc
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeLauncher : MonoBehaviour
{

    // Prefab for the Grenade object.
    [LabelOverride("Grenade Prefab")]
    [Tooltip("Prefab for instantiating the Grenades")]
    public GameObject m_gGrenadePrefab;

    [LabelOverride("Aiming Shadow")]
    [Tooltip("The gameobject that shall act as a faux shadow of the aiming line")]
    public GameObject m_gAimingShadow;

    [LabelOverride("Shot Sound")]
    [Tooltip("Sound to play as a Grenade is spawned")]
    public AudioClip m_acShotSound;

    [Header("Firing Variables")]
    [LabelOverride("Minimum Velocity")]
    [Tooltip("The lowest velocity that a grenade can be fired at")]
    public float m_fMinVelocity;

    [LabelOverride("Maximum Velocity")]
    [Tooltip("The highest velocity that a grenade can be fired at")]
    public float m_fMaxVelocity;

    // speed for the moving of the target line
    [LabelOverride("Aim Speed")]
    [Tooltip("Affects how long it will take for the velocity to hit Max Velocity, higher == faster")]
    public float m_fAimSpeed;

    [Space(10)]
    [LabelOverride("Line Angle")]
    [Tooltip("Affects the angle that the line draws by, default value is good")]
    public float m_fLineAngle = -20;

    // Pool for the grenades (should always be 1)
    private int m_nPoolsize = 1;

    // An array of grenade for instantiating rockets
    private GameObject[] m_agGrenadeList;

    // Boolean for the slider bar to bounce between m_fMinCharge and m_fMaxCharge
    private bool m_bIsAscending;

    // boolean for if the soldier is currently charging up a shot
    private bool m_bChargingShot;

    // line renderer that gives the player an idea of how the grenade will act upon firing
    private LineRenderer m_lrLine;

    // how many nodes should be in the LineRenderer
    private int m_nLineNodes = 10;

    // the force of gravity used to calculate the array
    private float m_fGravity;

    // The angle that the grenade will be shot at as a radius
    private float m_fRadiusAngle;

    // velocity that the grenade will be fired with
    private float m_fVelocity;

    // this GrenadeLaunchers audioSource
    private AudioSource m_asAudioSource;

    // this is the value that the AimingShadow will be reset to on release
    private Vector3 m_v3AimingShadowReset;

    //--------------------------------------------------------------------------------------
    // initialization.
    //--------------------------------------------------------------------------------------
    void Awake()
    {
        m_fVelocity = m_fMinVelocity;
        m_bChargingShot = false;
        m_bIsAscending = true;
        // initilize rocket list with size
        m_agGrenadeList = new GameObject[m_nPoolsize];
        m_lrLine = GetComponent<LineRenderer>();
        m_asAudioSource = GetComponent<AudioSource>();

        // go through each rocket
        for (int i = 0; i < m_nPoolsize; ++i)
        {
            // Instantiate and set active state
            m_agGrenadeList[i] = Instantiate(m_gGrenadePrefab);
            m_agGrenadeList[i].SetActive(false);
        }

        m_fGravity = Mathf.Abs(Physics.gravity.y);
        m_lrLine.useWorldSpace = false;
        m_v3AimingShadowReset = m_gAimingShadow.transform.localScale;
        
    }

    void Update()
    {
        
    }

    //--------------------------------------------------------------------------------------
    // MouseDown: Restes variables of the Launcher when the mouse button is pressed.
    //--------------------------------------------------------------------------------------
    public void MouseDown()
    {
        m_bIsAscending = true;
    }

    //--------------------------------------------------------------------------------------
    // MouseHeld: Charges the shot until it is no longer called.
    //--------------------------------------------------------------------------------------
    public void MouseHeld()
    {;
        // It is charging a shot
        m_bChargingShot = true;
        RenderArc();
        // If the counter should be ascending and the CastingLine.magnitude is less than MaxLength
        if (m_bIsAscending && m_fVelocity <= m_fMaxVelocity)
        {
            m_fVelocity += m_fAimSpeed * Time.deltaTime;
            m_gAimingShadow.transform.localScale += new Vector3(0,0,m_fAimSpeed * Time.deltaTime);
            // If the magnitude is higher or equal to MaxLength
            if (m_fVelocity >= m_fMaxVelocity)
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
            m_fVelocity -= m_fAimSpeed * Time.deltaTime;
            m_gAimingShadow.transform.localScale -= new Vector3(0, 0, m_fAimSpeed * Time.deltaTime);
            // if the magnitude is lower than a set number
            if (m_fVelocity <= m_fMinVelocity)
            {
                // start ascending
                m_bIsAscending = true;
            }
        }
    }

    //--------------------------------------------------------------------------------------
    // MouseUp: Spawns a bullet if the Mouse had been held beforehand.
    //--------------------------------------------------------------------------------------
    public void MouseUp()
    {
        if (m_bChargingShot)
        {
            m_asAudioSource.PlayOneShot(m_acShotSound);
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
            GameObject gGrenade = Allocate();
            gGrenade.GetComponent<Rigidbody>().velocity = transform.forward * m_fVelocity;
            // if the rocket is active
            if (gGrenade.activeInHierarchy)
            {
                // Reset all of the Rockets variables
                gGrenade.GetComponent<Grenade>().m_gSpawnPoint = gameObject;
                gGrenade.GetComponent<Rigidbody>().position = gGrenade.GetComponent<Grenade>().m_gSpawnPoint.transform.position;
                gGrenade.GetComponent<Rigidbody>().drag = 0;
                gGrenade.GetComponent<Grenade>().transform.position = gGrenade.GetComponent<Grenade>().m_gSpawnPoint.transform.position;
                gGrenade.GetComponent<Grenade>().m_fCurrentActivateTimer = gGrenade.GetComponent<Grenade>().m_fMaxActivateTimer;
                gGrenade.GetComponent<Grenade>().m_fLifespan = 600;
                gGrenade.GetComponent<Grenade>().m_bDisable = false;
            }
            // Reset variables for the firing functions
            m_bChargingShot = false;
            m_fVelocity = m_fMinVelocity;
            m_gAimingShadow.transform.localScale = m_v3AimingShadowReset;
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
            if (!m_agGrenadeList[i].activeInHierarchy)
            {
                //set active state
                m_agGrenadeList[i].SetActive(true);

                //return the rockets
                return m_agGrenadeList[i];
            }
        }
        //if all fail, return null
        return null;
    }


    void RenderArc()
    {
        Vector3 offset = new Vector3(0, 0.018f, 0);
        Debug.DrawRay(transform.position, (transform.forward + offset) * 100.0f, Color.red);
        m_lrLine.positionCount = m_nLineNodes + 1;
        m_lrLine.SetPositions(CalcArcArray());
    }

    private Vector3[] CalcArcArray()
    {
        // declare and initilize vector3 array
        Vector3[] v3Trajectory = new Vector3[m_nLineNodes + 1];
        float fDisplayVelocity = m_fVelocity * 3;
        m_fRadiusAngle = -Mathf.Deg2Rad * m_fLineAngle;
        // calculate the distance travelled (d = v^2 * Sin(2 * angle)) / g
        float fDis = fDisplayVelocity * fDisplayVelocity * Mathf.Sin(2 * m_fRadiusAngle) / m_fGravity;
        // loop through the number of size in the line renderer
        for (int iIndex = 0; iIndex <= m_nLineNodes; ++iIndex)
        {
            // divide iIndex by number of size of positions in the line renderer
            float t = (float)iIndex / (float)m_nLineNodes;
            float z = t * fDis;
            float y = transform.localPosition.y + (z * Mathf.Tan(m_fRadiusAngle) - ((m_fGravity * z * z) / (2 * fDisplayVelocity * fDisplayVelocity * Mathf.Cos(m_fRadiusAngle) * Mathf.Cos(m_fRadiusAngle))));

            v3Trajectory[iIndex] = new Vector3(transform.localPosition.x, y, z + transform.localPosition.z);
        }



        return v3Trajectory;
    }

}


