using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//-------------------------------------------------------------------------------------------------
// Rocket: Inheriting from Weapon.cs. Used for propelling rockets and their collison/damage numbers
//
//
// Author: Callan Davies
//-------------------------------------------------------------------------------------------------
public class TestingRocket : Weapon
{
    //A timer to stop the rocket from colliding with its Launcher
    [LabelOverride("Maximum time for Activation")]
    [Tooltip("Seconds it takes for the missle to not be within collision range of its Launcher")]
    public float m_fMaxActivateTimer = 2;

    [LabelOverride("Mask for Knockback and/or damage")]
    [Tooltip("Set this to the Unit layer, so the Rocket doesn't knockback objects that should be stationary")]
    public LayerMask m_UnitMask;

    // The force that hits back all Unit layered objects
    [Header("Explosion Variables")]
    [LabelOverride("Force of Explosion")]
    [Tooltip("The force that hits back all Unit layered objects")]
    public float m_ExplosionForce = 1000f;

    [LabelOverride("Direct Hit Modifier")]
    [Tooltip("The velocity is multiplied by this to reach an acceptable knockback")]
    public float m_fHitMultiplier = 0.5f;

    // Radius for the Area of Effect Explosion that should follow any Collision
    [LabelOverride("Radius of Explosion")]
    [Tooltip("Radius for the Area of Effect Explosion that should follow any Collision")]
    public float m_fExplosionRadius = 5f;

    // how fast the rocket will reach its target
    public float m_fSpeed;

    // pointer to the RocketLauncher so it knows where to spawn
    [HideInInspector]
    public GameObject m_gSpawnPoint;

    // The direction the Rocket should move
    [HideInInspector]
    public Vector3 m_v3Target;

    // Starts at m_fMaxActivateTimer and ticks down to zero, then resetting upon being set Inactive
    [HideInInspector]
    public float m_fCurrentActivateTimer;

    // its own rigidbody
    private Rigidbody m_rbRocket;

    // time it will take for the rocket to reach its target position.
    private float m_fLerpTime = 0;

    // Use this for initialization
    void Awake()
    {
        // m_gRocketLauncher = m_gSpawnPoint.GetComponent<TestingLauncher>();
        // set the rockets power variable to the power variable of the Launcher
        // m_fPower = m_gRocketLauncher.m_fCharge;
        // get the rockets rigidbody
        m_rbRocket = GetComponent<Rigidbody>();
        // set the direction for the rocket to do towards
        //m_v3MoveDirection = m_gSpawnPoint.transform.forward;
        // initilize ActivateTimer
        m_fCurrentActivateTimer = m_fMaxActivateTimer;
	}
	
	// Update is called once per frame
	void Update()
    {
        m_fCurrentActivateTimer -= 1;
        m_fLerpTime += Time.deltaTime * m_fSpeed;
        if (m_fLerpTime > 1.0f)
            m_fLerpTime = 1.0f;

        transform.position = Vector3.Lerp(m_gSpawnPoint.transform.position, m_v3Target, m_fLerpTime);
    }

    //--------------------------------------------------------------------------------------
    // OnTriggerEnter: When a rocket Collides (Is Trigger)
    //
    // Param: other is the other object it is colliding with at call
    //
    //--------------------------------------------------------------------------------------
    private void OnTriggerEnter(Collider other)
    {

        Debug.Log("COLLISION");
        // if the rocket can now activate
        if (m_fCurrentActivateTimer <= 0)
        {

            if (other.tag == "Soldier")
            {

                Rigidbody rbTarget = other.GetComponent<Rigidbody>();
                rbTarget = other.GetComponent<Rigidbody>();
                rbTarget.AddForce(m_rbRocket.velocity * m_fHitMultiplier, ForceMode.Impulse);
            }

            // Collect all possible colliders 
            Collider[] aColliders = Physics.OverlapSphere(transform.position, m_fExplosionRadius, m_UnitMask);

            for (int i = 0; i < aColliders.Length; i++)
            {
                Rigidbody rbTarget = aColliders[i].GetComponent<Rigidbody>();

                //if it does not have a rigidbody
                if (!rbTarget)
                {
                    continue;
                }


                if (aColliders[i].gameObject.tag == "Soldier")
                {
                    // TODO: Explosion particle effect
                    SoldierActor gtarget = rbTarget.GetComponent<SoldierActor>();
                    gtarget.TakeDamage(CalculateDamage(aColliders[i].transform.position));
                    // add explosive force
                    rbTarget.AddExplosionForce(m_ExplosionForce, transform.position, m_fExplosionRadius, 3.0f, ForceMode.Impulse);
                }
                else if (aColliders[i].gameObject.tag == "Teddy")
                {
                    Teddy gtarget = rbTarget.GetComponent<Teddy>();
                    gtarget.TakeDamage(CalculateDamage(aColliders[i].transform.position));
                }

            }
            gameObject.SetActive(false);
           // m_gRocketLauncher.m_bRocketAlive = false;
        }
    }

    //--------------------------------------------------------------------------------------
    //  CalculateDamage: Calculates the damage so being further from the explosion results in less damage
    //
    // Returns: the damage for the Soldiers within range to take
    //
    //--------------------------------------------------------------------------------------
    private float CalculateDamage(Vector3 v3TargetPosition)
    {
        // create a vector from the shell to the target
        Vector3 explosionToTarget = v3TargetPosition - transform.position;

        // Calculated the distance from the shell to the target
        float explosionDistance = explosionToTarget.magnitude;

        // calculate the proportion of the Maximum distance the target is away
        float relativeDistance = (m_fExplosionRadius - explosionDistance) / m_fExplosionRadius;

        // Calculate damage as this proportion of the maximum possible damage
        float fDamage = relativeDistance * m_fDamage;

        fDamage = Mathf.Max(0f, fDamage);

        return fDamage;
    }
}
