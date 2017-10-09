using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//--------------------------------------------------------------------------------------
// SoldierActor: The controller that PlayerActor will access when it is its turn to act
//
// PlayerActor has a Uni-Directional Association with SoldierActor
// (SoldierActor does not know about the relationship)
//
// SoldierActor has a Bi-Directional Association with the WeaponClass and all of its children
//  SoldierActor will pass down it's m_fCharge value to its currentWeapons m_fPower
//
//--------------------------------------------------------------------------------------
public class SoldierActor : MonoBehaviour
{
    // Slider for the aiming arrow
    [Tooltip("Should be set to the Slider underneath the Soldier")]
    public Slider m_sAimSlider;

    // Speed for the slider
    [Tooltip("Speed that the Slider moves by per update")]
    public float m_fSliderSpeed = 0.0f;

    // Is the Charge bar meant to be ascending?
    bool m_bIsAscending;

    // Minimum power for a shot
    [Tooltip("Minimum charge for the Charge, be sure that this matches the 'min value' variable in the Sliders inspector")]
    public float m_fMinCharge = 15f;

    // Float for Max Charge
    [Tooltip("Maximum charge for the Charge, be sure that this matches the 'max value' variable in the Sliders inspector")]
    public float m_fMaxCharge = 30;

    // boolean for if the soldier is in the firing function
    private bool m_bFiring;

    // Speed at which the Soldier moves
    [Tooltip("The Speed at which the Soldier moves")]
    public float m_fSpeed;

    //Speed at which the soldier rotates towards the mouse
    [Tooltip("The Speed at which the Soldier rotates towards the mouse")]
    public float m_fRotSpeed;

    // The Soldiers health when initilizied
    [Tooltip("The Soldiers health when initilizied")]
    public float m_fMaxHealth = 100;

    // Intiger value for the Soldiers current weapon
    // 0 = RocketLauncher
    // 1 = Minigun
    // 2 = Grenade
    [HideInInspector]
    public int m_nCurrentWeapon;

    // Values for the inventory count of this Soldier
    public int m_nGrenadeCount;
    public int m_nGotMinigun;
    public int m_nGotGrenade;

    // Current Charge to pass on to the weapons firing Power (m_fPower) 
    [Tooltip("Current Charge to pass on to the weapons firing Power")]
    public float m_fCharge = 1;

    // boolean for if the soldier is currently charging up a shot
    private bool m_bChargingShot;


    // This Soldiers rigidBody for movement purposes
    private Rigidbody m_rbRigidBody;

    // A boolean for living, will be false when the soldier dies
    private bool m_bAlive;

    // A Number so the PlayerActor can know which soldier to move
    public int m_nSoldierNumber;

    // The Soldiers current health,
    // (Will be equal to m_nMaxHealth until it takes damage
    public float m_fCurrentHealth;

    RocketLauncher m_gLauncherScript;

    public GameObject m_gRocketLauncher;

    void Start()
    {
        m_rbRigidBody = GetComponent<Rigidbody>();
        m_gLauncherScript = m_gRocketLauncher.GetComponent<RocketLauncher>();
        // Soldiers Current health should always start at MaxHealth
        m_fCurrentHealth = m_fMaxHealth;
        // Soldier should start off as alive
        m_bAlive = true;
        m_bFiring = false;
        m_bIsAscending = true;
        m_bChargingShot = false;
        m_fCharge = m_fMinCharge;
    }

    void Update()
    {
      Move();
      m_rbRigidBody.freezeRotation = true;
      FaceMouse();

      Fire(m_fCharge);
      // Makes the Slider represent the charge
      m_sAimSlider.value = m_fCharge;   
      // As health is a float, anything below one will be displayed as 0 to the player
      if(m_fCurrentHealth < 1)
      {
       Die();
      }
    }


    //--------------------------------------------------------------------------------------
    //  SwitchWeapon: Call when the Player wants to change the Soldiers currentWeapon
    //
    // Returns: Void
    //
    // 
    //
    //--------------------------------------------------------------------------------------
    private void SwitchWeapon()
    {

    }


    //--------------------------------------------------------------------------------------
    // Fire: Call when the Player commands the Soldier to fire
    //
    //--------------------------------------------------------------------------------------
    public void Fire(float fCharge)
    {
        
        if (m_gLauncherScript.m_bRocketAlive == false)
        {
            //m_sAimSlider.value = m_fMinCharge;

            if (Input.GetButtonDown("Fire1"))
            {
                m_bFiring = true;
                m_fCharge = m_fMinCharge;
            }
            if (Input.GetButton("Fire1"))
            {
                m_bChargingShot = true;
                if (m_bFiring)
                {
                    // If current weapon is rocketLauncher
                    if (m_nCurrentWeapon == 0)
                    {
                        if (m_bIsAscending && m_fCharge <= m_fMaxCharge)
                        {
                            m_fCharge += m_fSliderSpeed /* Time.deltaTime*/;
                            if (m_fCharge >= m_fMaxCharge)
                            {
                                m_bIsAscending = false;
                            }
                        }
                        else
                        {
                            m_bIsAscending = false;
                            m_fCharge -= m_fSliderSpeed /* Time.deltaTime*/;
                            if (m_fCharge <= m_fMinCharge)
                            {
                                m_bIsAscending = true;
                            }
                        }
                    }
                }
            }
            
            if (Input.GetButtonUp("Fire1"))
            {
                if (m_bChargingShot)
                {
                    // If current weapon is rocketLauncher
                    if (m_nCurrentWeapon == 0)
                    {
                        m_gLauncherScript.Fire(m_fCharge);
                        m_fCharge = m_fMinCharge;
                    }

                    m_bFiring = false;
                    m_bChargingShot = false;
                }
            }
        }
    }


    //--------------------------------------------------------------------------------------
    // Move: Uses the Soldiers RigibBody to move with a drag value, 
    //       making it so the Soldier doesn't stop instantly
    // 
    //--------------------------------------------------------------------------------------
    public void Move()
    {
        if (!m_bFiring)
        {
            float fMoveHorizontal = Input.GetAxis("Horizontal");
            float fMoveVertical = Input.GetAxis("Vertical");
            Vector3 v3Movement = new Vector3(fMoveHorizontal, 0, fMoveVertical);
            m_rbRigidBody.velocity = v3Movement * m_fSpeed;
        }
    }

    //--------------------------------------------------------------------------------------
    // FaceMouse: will make the Soldier rotate towards the mouse only along it's y axis
    //
    //
    // Returns: The Quaternion for the Soldier to rotate to the mouse
    //
    // NOTE: m_fRotSpeed is the speed modifier for it, the speed will affect how quickly the Soldier rotates 
    // 
    //--------------------------------------------------------------------------------------
    private Quaternion FaceMouse()
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


    //--------------------------------------------------------------------------------------
    // TakeDamage: Function for taking damage, for weapons to access
    //
    //
    //--------------------------------------------------------------------------------------
    public void TakeDamage(float fDamage)
    {
        m_fCurrentHealth -= fDamage;
    }

    //--------------------------------------------------------------------------------------
    // Die: Function for the soldier dying
    //
    //
    //--------------------------------------------------------------------------------------
    private void Die()
    {
        // TODO: Animation and such
        gameObject.SetActive(false);
        m_fCurrentHealth = m_fMaxHealth;
    }
}
