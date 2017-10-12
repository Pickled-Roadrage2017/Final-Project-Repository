// Using, etc
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//--------------------------------------------------------------------------------------
// Teddy object. Inheriting from MonoBehaviour. Used for controling the Teddy base.
//--------------------------------------------------------------------------------------
public class Teddy : MonoBehaviour
{
    [Tooltip("Teddy bear Maximum health.")]
    public float m_fMaxHealth;


    // health that will be set to MaxHealth in Awake
    [Tooltip("For displaying the current health, will be made private when we have a diplay for it somewhere on-screen")]
    public float m_fCurrentHealth;

    // public float for the teddy throwing charge.
    [Tooltip("The charge of the Teddy throwing mechanic.")]
    public float m_fCharge;

    // This will be null, unless the teddy is holding a soldier
    // public GameObject m_gSoldier;

    // An empty game object where a held soldier will appear
    [Tooltip("An empty game object where a held soldier will appear")]
    public GameObject m_gBearHand;

    // The blueprint fot the teddy projectile
    //public GameObject m_gProjectileBlueprint;

    public Canvas m_cTeddyCanvas;

    [Header("Slider Variables")]
    // Slider for the aiming arrow
    [Tooltip("Should be set to the Slider underneath the Soldier")]
    public Slider m_sAimSlider;

    // Speed for the slider
    [Tooltip("Speed that the Slider moves by per update")]
    public float m_fSliderSpeed = 0.0f;

    [Header("Firing Variables")]
    // Minimum power for a shot
    [Tooltip("Minimum charge for the Charge, be sure that this matches the 'min value' variable in the Sliders inspector")]
    public float m_fMinCharge = 15f;

    // Float for Max Charge
    [Tooltip("Maximum charge for the Charge, be sure that this matches the 'max value' variable in the Sliders inspector")]
    public float m_fMaxCharge = 30;

    // Boolean for the slider bar to bounce between m_fMinCharge and m_fMaxCharge
    private bool m_bIsAscending;

    // boolean for if the soldier is in the firing function
    private bool m_bFiring;

    // boolean for if the soldier is currently charging up a shot
    private bool m_bChargingShot;

    private GameObject m_gProjectile;


    //--------------------------------------------------------------------------------------
    // initialization.
    //--------------------------------------------------------------------------------------
    void Awake()
    {
        // initilize projectile list with size
       // m_gProjectile = new GameObject();
       // m_gProjectile.transform.SetParent(transform);
       // Instantiate and set active state
        //m_gProjectile = Instantiate(m_gProjectileBlueprint);
       // m_gProjectile.SetActive(false);

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
	}

    //--------------------------------------------------------------------------------------
    // Throw: Function for throwing a solider across the map.
    //--------------------------------------------------------------------------------------
    void Throw()
    {
      /*  if(m_gSoldier != null)
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
        }*/
    }


    //--------------------------------------------------------------------------------------
    // OnTriggerEnter: When something collides with the Teddy (Is Trigger)
    //
    // Param: other is the other object it is colliding with at call
    //
    //--------------------------------------------------------------------------------------
    void OnTriggerEnter(Collider other)
    {
     /*   if(other.gameObject.tag == "Soldier")
        {
            m_gSoldier = other.gameObject;
            // Set the object parent to the Bearhand
            m_gSoldier.transform.parent.SetParent(m_gBearHand.transform);
            m_gSoldier.transform.position = m_gBearHand.transform.position;
            //other.gameObject.SetActive(false);
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
}

