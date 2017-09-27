using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
    // Speed at which the Soldier moves
    [Tooltip("The Speed at which the Soldier moves")]
    public float m_fSpeed;

    //Speed at which the soldier rotates
    public float m_fRotSpeed;

    // The Soldiers health when initilizied
    [Tooltip("The Soldiers health when initilizied")]
    public int m_nMaxHealth;

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
    public float m_fCharge;

    // This Soldiers rigidBody for movement purposes
    private Rigidbody m_rbRigidBody;

    // A boolean for living, will be false when the soldier dies
    private bool m_bAlive;

    // A Number so the PlayerActor can know which soldier to move
    private int m_nSoldierNumber;

    // The Soldiers current health,
    // (Will be equal to m_nMaxHealth until it takes damage
    private int m_nCurrentHealth;


    public RocketLauncher m_goRPG;
    void Start()
    {
        m_rbRigidBody = GetComponent<Rigidbody>();

        // Soldiers Current health should always start at MaxHealth
        m_nCurrentHealth = m_nMaxHealth;
        // Soldier should start off as alive
        m_bAlive = true;


    }
    //
    void FixedUpdate()
    {
        Move();
        m_rbRigidBody.freezeRotation = true;
        PlatformGetSoldierFireDirection();
        Vector3 v3MousePos = new Vector3(0, Input.GetAxis("Mouse X") * m_fRotSpeed, 0);
        transform.Rotate(v3MousePos);

        if(Input.GetButtonDown("Fire1"))
        {
            // If current weapon is rocketLauncher
            if(m_nCurrentWeapon == 0)
            {
                m_goRPG.Fire();
            }
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
    // Returns: Void
    //
    //
    // 
    //--------------------------------------------------------------------------------------
    private void Fire()
    {
        
    }


    //--------------------------------------------------------------------------------------
    // Move: Uses the Soldiers RigibBody to move with a drag value, 
    //       making it so the Soldier doesn't stop instantly
    //
    // Returns: Void
    //
    // NOTE: Soldier will never slow unless you set the Drag value in the RigidBody in the Inspector 
    // 
    //--------------------------------------------------------------------------------------
    private void Move()
    {
        float fMoveHorizontal = Input.GetAxis("Horizontal");
        float fMoveVertical = Input.GetAxis("Vertical");
        Vector3 v3Movement = new Vector3(fMoveHorizontal, 0, fMoveVertical);
        m_rbRigidBody.velocity = v3Movement * m_fSpeed;

    }

    private Vector3 PlatformGetSoldierFireDirection()
    {
        Vector3 v3MousePos = Input.mousePosition;
        // use the current camera to convert it to a ray
        Ray rMouseRay = Camera.main.ScreenPointToRay(v3MousePos);
        // create a plane that faces up at the same position as the player
        Plane pSoldierPlane = new Plane(Vector3.up, transform.position);
        // find out how far along the ray the intersection is
        float fRayDistance = 0;
        pSoldierPlane.Raycast(rMouseRay, out fRayDistance);
        //find the collision point from the distance
        Vector3 v3CastPoint = rMouseRay.GetPoint(fRayDistance);

        Vector3 v3ToCastPoint = v3CastPoint - transform.position;
        v3ToCastPoint.Normalize();

        return v3ToCastPoint;
    }
}
