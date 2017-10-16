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

    // This will be null, unless the teddy is holding a soldier
    [LabelOverride("Held Soldier")]
    public GameObject m_gSoldier;

    // An empty game object where a held soldier will appear
    [LabelOverride("Hand Transform")][Tooltip("An empty game object where a held soldier will appear")]
    public GameObject m_gBearHand;

    // The blueprint for the teddy projectile
    [LabelOverride("Teddy Projectile Prefab")]
    public GameObject m_gProjectileBlueprint;

    [LabelOverride("Teddy Canvas")][Tooltip("Canvas object for the bear to access")]
    public Canvas m_cTeddyCanvas;

    [Header("Slider Variables")]
    // Slider for the aiming arrow
    [LabelOverride("UI Slider")][Tooltip("Should be set to the Slider underneath the Soldier")]
    public Slider m_sAimSlider;

    // Speed for the slider
    [LabelOverride("Slider Speed")][Tooltip("Speed that the Slider moves by per update")]
    public float m_fSliderSpeed = 0.0f;

    // Boolean for the slider bar to bounce between m_fMinCharge and m_fMaxCharge
    private bool m_bIsAscending;

    // boolean for if the soldier is in the firing function
    private bool m_bFiring;

    // boolean for if the soldier is currently charging up a shot
    private bool m_bChargingShot;

    private GameObject m_gProjectile;


    //--------------------------------------------------------------------------------------
    // Initialization.
    //--------------------------------------------------------------------------------------
    void Awake()
    {
        //Initilize projectile list with size
        m_gProjectile = new GameObject();
        m_gProjectile.transform.SetParent(transform);
        //Instantiate and set active state
        m_gProjectile = Instantiate(m_gProjectileBlueprint);
        m_gProjectile.SetActive(false);

        m_fCurrentHealth = m_fMaxHealth;
        m_bIsAscending = true;
        m_bFiring = false;
        m_bChargingShot = false;
        m_cTeddyCanvas.gameObject.SetActive(false);
	}

    //--------------------------------------------------------------------------------------
    // Update: Function that calls each frame to update game objects.
    //--------------------------------------------------------------------------------------
    void Update()
    {
        //Throw();
        //m_cTeddyCanvas.transform.rotation = FaceMouse();
        //FaceMouse();
    }

    //--------------------------------------------------------------------------------------
    // Throw: Function for throwing a solider across the map.
    //--------------------------------------------------------------------------------------
    void Throw()
    {
        if(m_gSoldier != null)
        {
            m_cTeddyCanvas.gameObject.SetActive(true);
                // When leftMouseButton is pressed down
                if (Input.GetButtonDown("Fire1"))
                {
                    m_bFiring = true;
                    m_fCharge = m_fMinCharge;
                }
                // While leftMouseButton is pressed down
                if (Input.GetButton("Fire1"))
                {
                    m_bChargingShot = true;
                    if (m_bFiring)
                    {
                            if (m_bIsAscending && m_fCharge <= m_fMaxCharge)
                            {
                                m_fCharge += m_fSliderSpeed;
                                if (m_fCharge >= m_fMaxCharge)
                                {
                                    m_bIsAscending = false;
                                }
                            }
                            else
                            {
                                m_bIsAscending = false;
                                m_fCharge -= m_fSliderSpeed;
                                if (m_fCharge <= m_fMinCharge)
                                {
                                    m_bIsAscending = true;
                                }
                            }
                        
                    }
                }
                // When leftMouseButton is released
                if (Input.GetButtonUp("Fire1"))
                {
                    if (m_bChargingShot)
                    {
                      Fire(m_fCharge);
                      m_bFiring = false;
                      m_bChargingShot = false;
                    }
                m_cTeddyCanvas.gameObject.SetActive(false);
            }
        }
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
            m_gSoldier = other.gameObject;
            // Set the object parent to the Bearhand
            m_gSoldier.transform.parent.SetParent(m_gBearHand.transform);
            m_gSoldier.transform.position = m_gBearHand.transform.position;
            //other.gameObject.SetActive(false);
            m_cTeddyCanvas.gameObject.SetActive(true);
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
            m_gSoldier.transform.parent.SetParent(null);
            m_gSoldier = null;
        } */ 
    }


    public void Fire(float fCharge)
    {
        // re-Initilise all Variables of the projectile
        m_gProjectile = Allocate();
        if (m_gProjectile.activeInHierarchy)
        {
            m_gProjectile.GetComponent<TeddyProjectile>().m_gSpawnPoint = gameObject;
            m_gProjectile.GetComponent<Rigidbody>().position = m_gProjectile.GetComponent<TeddyProjectile>().m_gSpawnPoint.transform.position;
            m_gProjectile.GetComponent<TeddyProjectile>().transform.position = m_gProjectile.GetComponent<TeddyProjectile>().m_gSpawnPoint.transform.position;
            m_gProjectile.GetComponent<TeddyProjectile>().m_fPower = fCharge;
            m_gProjectile.GetComponent<TeddyProjectile>().m_fAirDrop = 0;
            m_gProjectile.GetComponent<Rigidbody>().velocity = Vector3.zero;
            m_gProjectile.GetComponent<TeddyProjectile>().m_v3MoveDirection = transform.forward;
            m_gProjectile.GetComponent<TeddyProjectile>().m_fCurrentActivateTimer = m_gProjectile.GetComponent<TeddyProjectile>().m_fMaxActivateTimer;
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
        //check if active
        if (!m_gProjectile.activeInHierarchy)
        {
            //set active state
            m_gProjectile.SetActive(true);

            //return the projectile
            return m_gProjectile;
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

    public Quaternion FaceMouse()
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
                transform.rotation = Quaternion.Slerp(transform.rotation, v3TargetRotation, m_fRotSpeed * Time.deltaTime);
            }
            return transform.rotation;
        }
        else
        {
            return new Quaternion();
        }
    }
}

