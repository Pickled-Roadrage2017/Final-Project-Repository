// Using, etc
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//--------------------------------------------------------------------------------------
// Purpose: Scripting for the Bears
//
// Description: Inheriting from MonoBehaviour, 
// All of the funtionality of the Teddy is in here
//
// Author: Thomas Wiltshire
// Edited: Callan Davies
//--------------------------------------------------------------------------------------
public class Teddy : MonoBehaviour
{
    [Header("Health Variables")]
    [LabelOverride("Teddy Max Health")][Tooltip("Teddy bear Maximum health.")]
    public float m_fMaxHealth;

    // health that will be set to MaxHealth in Awake
    [LabelOverride("Current Teddy Health")]
    [Tooltip("For displaying the current health, will be made private when we have a diplay for it somewhere on-screen")]
    public float m_fCurrentHealth;

    [Header("Throwing Variables")]
    // Speed for the rotation of the aiming arrow
    [LabelOverride("Facing Speed")]
    public float m_fRotSpeed;

    // public float for the teddy throwing charge.
    [LabelOverride("Current Charge")][Tooltip("The charge of the Teddy throwing mechanic.")]
    public float m_fCharge;

    // Minimum power for a shot
    [LabelOverride("Charge Minimum")][Tooltip("Minimum charge for the Charge, be sure that this matches the 'min value' variable in the Sliders inspector")]
    public float m_fMinCharge = 1f;

    // Float for Max Charge
    [LabelOverride("Charge Maximum")][Tooltip("Maximum charge for the Charge, be sure that this matches the 'max value' variable in the Sliders inspector")]
    public float m_fMaxCharge = 2f;

    // Speed for the slider
    [LabelOverride("Charge Speed")]
    [Tooltip("Speed that the charge increased by per update")]
    public float m_fChargeSpeed = 0.0f;

    // This will be null, unless the teddy is holding a soldier
   //[LabelOverride("Held Soldier")]
    //public GameObject m_gSoldier;

    // An empty game object where a held soldier will appear
    //[LabelOverride("Hand Transform")][Tooltip("An empty game object where a held soldier will appear")]
   // public GameObject m_gBearHand;

    // The blueprint for the teddy projectile
    //[LabelOverride("Teddy Projectile Prefab")]
    //public GameObject m_gProjectileBlueprint;

    //[LabelOverride("Teddy Canvas")][Tooltip("Canvas object for the bear to access")]
    //public Canvas m_cTeddyCanvas;

    //[Header("Slider Variables")]
    // Slider for the aiming arrow
    //[LabelOverride("UI Slider")][Tooltip("Should be set to the Slider underneath the Soldier")]
    //public Slider m_sAimSlider;

    // pivot for the canvas
    //public GameObject m_gPivot;

    // Boolean for the slider bar to bounce between m_fMinCharge and m_fMaxCharge
    //private bool m_bIsAscending;

    // boolean for if the soldier is in the firing function
    //private bool m_bFiring;

    // boolean for if the soldier is currently charging up a shot
   // private bool m_bChargingShot;

    // Pool of Projectile objects
    private GameObject[] m_agProjectileList;

    // pool for projectiles
    private int m_nPoolSize = 1;

    //--------------------------------------------------------------------------------------
    // Initialization.
    //--------------------------------------------------------------------------------------
    void Awake()
    {
        // initilize rocket list with size
        m_agProjectileList = new GameObject[m_nPoolSize];
     //   m_gBearHand.transform.Rotate(0, 0, 30);
        // go through each projectile
        for (int i = 0; i < m_nPoolSize; ++i)
        {
            // Instantiate and set active state
            //m_agProjectileList[i] = Instantiate(m_gProjectileBlueprint);
            //m_agProjectileList[i].GetComponent<TeddyProjectile>().m_gSpawnPoint = m_gBearHand;
           // m_agProjectileList[i].SetActive(false);
            
        }
        m_fCurrentHealth = m_fMaxHealth;
       /* m_bIsAscending = true;
        m_bFiring = false;
        m_bChargingShot = false;
        m_cTeddyCanvas.gameObject.SetActive(false);
        m_fCharge = m_fMinCharge; */
       // m_gPivot.transform.rotation = new Quaternion(0, 0, 0, 0);
    }

    //--------------------------------------------------------------------------------------
    // Update: Function that calls each frame to update game objects.
    //--------------------------------------------------------------------------------------
    void Update()
    {
    /*    if(Input.GetButton("Fire1"))
        {
            m_bFiring = true;
        }
        else
        {
            m_bFiring = false;
        }
        Throw(m_bFiring); */
        if(!IsAlive())
        {
            gameObject.SetActive(false);
        }
      //  m_gPivot.transform.rotation = new Quaternion(0, 0, 0, 0);
      //  m_gPivot.transform.rotation = FaceMouse();
    }

    //--------------------------------------------------------------------------------------
    // OnTriggerEnter: When something collides with the Teddy (Is Trigger)
    //
    // Param: other is the other object it is colliding with at call
    //
    //--------------------------------------------------------------------------------------
    void OnTriggerEnter(Collider other)
    {
        /*if(other.gameObject.tag == "Soldier")
        {
            if (m_gSoldier == null)
            {
                m_gSoldier = other.gameObject;
               // other.gameObject.SetActive(false);
                m_cTeddyCanvas.gameObject.SetActive(true);
            }
        } */
    }

    //--------------------------------------------------------------------------------------
    // OnTriggerExit: After something collides with Teddy, then leaves the collision area
    //
    // Param: other is the other object it is now no longer colliding
    //
    //--------------------------------------------------------------------------------------
    void OnTriggerExit(Collider other)
    {
       /* if(m_gSoldier != null)
        {
            m_gSoldier.transform.SetParent(null);
            m_gSoldier = null;
        }  */
    }


 /*   public void SpawnProjectile()
    {
        if (m_bChargingShot)
        {
            // re-Initilise all Variables of the rocket
            GameObject gProjectile = Allocate();
            if (gProjectile.activeInHierarchy)
            {
                // Resets all of gProjeciles current values to their initial values
                gProjectile.GetComponent<TeddyProjectile>().m_gSpawnPoint = gameObject;
                gProjectile.GetComponent<Rigidbody>().position = gProjectile.GetComponent<TeddyProjectile>().m_gSpawnPoint.transform.position;
                gProjectile.GetComponent<TeddyProjectile>().transform.position = gProjectile.GetComponent<TeddyProjectile>().m_gSpawnPoint.transform.position;
                gProjectile.GetComponent<TeddyProjectile>().m_fPower = m_fCharge;
                gProjectile.GetComponent<TeddyProjectile>().m_fAirDrop = 0;
                gProjectile.GetComponent<Rigidbody>().velocity = Vector3.zero;
                gProjectile.GetComponent<TeddyProjectile>().m_v3MoveDirection = transform.forward;
                gProjectile.GetComponent<TeddyProjectile>().m_fCurrentActivateTimer = gProjectile.GetComponent<TeddyProjectile>().m_fMaxActivateTimer;
                gProjectile.GetComponent<TeddyProjectile>().m_gSoldier = m_gSoldier;
            }

            m_fCharge = m_fMinCharge;
            m_bChargingShot = false;
        }

    } */
 /*   public void Throw(bool bMouseDown)
    {
        if (bMouseDown)
        {
            m_sAimSlider.value = m_fCharge;
            m_bChargingShot = true;

            if (m_bIsAscending && m_fCharge <= m_fMaxCharge)
            {
                m_fCharge += m_fChargeSpeed * Time.deltaTime;
                if (m_fCharge >= m_fMaxCharge)
                {
                    m_bIsAscending = false;
                }
            }
            else
            {
                m_bIsAscending = false;
                m_fCharge -= m_fChargeSpeed * Time.deltaTime;
                if (m_fCharge <= m_fMinCharge)
                {
                    m_bIsAscending = true;
                }
            }
        }
        else if (m_bChargingShot)
        {
            SpawnProjectile();
        }
    } */

    //--------------------------------------------------------------------------------------
    // Allocate: For accessing a projeectile that isn't currently active in m_agPreojectileList 
    //           
    // Returns: A gamesobject from m_agProjectileList
    //
    //--------------------------------------------------------------------------------------
    GameObject Allocate()
    {
        // Instanciate a new Rocket Prefab
        for (int i = 0; i < m_nPoolSize; ++i)
        {
            //check if active
            if (!m_agProjectileList[i].activeInHierarchy)
            {
                //set active state
                m_agProjectileList[i].SetActive(true);



                //return the rocket
                return m_agProjectileList[i];
            }
        }
        //if all fail, return null
        return null;
    }

    //--------------------------------------------------------------------------------------
    // TakeDamage: Function for taking damage, for weapons to access
    //
    // Param: The amount of damage that the Teddy will take to m_fCurrentHealth
    //
    //--------------------------------------------------------------------------------------
    public void TakeDamage(float fDamage)
    {
        // Minus the Teddys currentHealth by the fDamage argument
        m_fCurrentHealth -= fDamage;
    }

  /*  public Quaternion FaceMouse()
    {
        if (!m_bFiring)
        {
            // Generate a plane that intersects the transform's position.
            Plane pSoldierPlane = new Plane(Vector3.up, transform.position);

            // Generate a ray from the cursor position
            Ray rMouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            float fHitDistance = 0.0f;
            // If the ray is parallel to the plane, Raycast will return false.
            if (pSoldierPlane.Raycast(rMouseRay, out fHitDistance))
            {
                // Get the point along the ray that hits the calculated distance.
                Vector3 v3TargetPos = rMouseRay.GetPoint(fHitDistance);

                // Determine the target rotation.  This is the rotation if the transform looks at the target point.
                Quaternion v3TargetRotation = Quaternion.LookRotation(v3TargetPos - transform.position);

                // Smoothly rotate towards the target point.
                m_gPivot.transform.rotation = Quaternion.Slerp(m_gPivot.transform.rotation, v3TargetRotation, m_fRotSpeed * Time.deltaTime);
            }
            return m_gPivot.transform.rotation;
        }
        else
        {
            return new Quaternion();
        }
    } */

    public bool IsAlive()
    {
        if(m_fCurrentHealth <= 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}

