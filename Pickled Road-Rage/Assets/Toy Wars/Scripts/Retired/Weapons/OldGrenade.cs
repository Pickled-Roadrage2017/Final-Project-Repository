////--------------------------------------------------------------------------------------
//// Purpose: How the grenade weapon will act when spawned
////
//// Description: Inheriting from Weapon.cs, Used for propelling grenades and their collison/damage numbers 
////
//// Author: Callan Davies
////--------------------------------------------------------------------------------------

//// Using, etc
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//public class OldGrenade : Weapon
//{

//    //A timer to stop the grenade from colliding with its Launcher
//    [LabelOverride("Activation Timer")]
//    [Tooltip("Seconds it takes for the grenade to not be within collision range of its Launcher")]
//    public float m_fMaxActivateTimer = 2;

//    [Header("Grenade-specific Variables")]
//    [LabelOverride("Bounciness")]
//    [Tooltip("How bouncy the grenade is")]
//    public float m_fBounceFactor;

//    [LabelOverride("Grenade Fuse")]
//    [Tooltip("The grenade will explode when this reaches zero")]
//    public float m_fMaxFuseTimer = 4;

//    // The force that hits back all Unit layered objects
//    [Header("Explosion Variables")]
//    [LabelOverride("Explosion Force")]
//    [Tooltip("The force that hits back all Unit layered objects")]
//    public float m_ExplosionForce = 50.0f;

//    // Radius for the Area of Effect Explosion that should follow any Collision
//    [LabelOverride("Explosion Radius")]
//    [Tooltip("Radius for the Area of Effect Explosion that should follow any Collision")]
//    public float m_fExplosionRadius = 5f;

//    [LabelOverride("Direct Hit Modifier")]
//    [Tooltip("The velocity is multiplied by this to reach an acceptable knockback")]
//    public float m_fHitMultiplier = 0.5f;

//    // how fast the grenade will reach its target
//    [LabelOverride("Speed")]
//    [Tooltip("How quickly the grenade will reach its objective")]
//    public float m_fSpeed;

//    // pointer to the GrenadeLauncher so it knows where to spawn
//    [HideInInspector]
//    public GameObject m_gSpawnPoint;

//    // time it will take for the grenade to reach its target position.
//    [HideInInspector]
//    public float m_fLerpTime = 0.0f;

//    // The direction the grenade should move
//    [HideInInspector]
//    public Vector3 m_v3Target;

//    // Starts at m_fMaxActivateTimer and ticks down to zero, then resetting upon being set Inactive
//    [HideInInspector]
//    public float m_fCurrentActivateTimer;

//    // The height that the grenade will arc to, this is passed down from its Launcher
//    [HideInInspector]
//    public float m_fArcHeight;

//    // its own rigidbody
//    private Rigidbody m_rbGrenade;

//    // boolean for if the grenade should be counting down to explosion
//    private bool m_bFuseTicking;

//    // When this is less than or equal to zero, the grenade will explode
//    private float m_fFuseTimer;

//    //--------------------------------------------------------------------------------------
//    // initialization.
//    //--------------------------------------------------------------------------------------
//    void Awake()
//    {
//        m_rbGrenade = GetComponent<Rigidbody>();
//        m_fFuseTimer = m_fMaxFuseTimer;
//        m_bFuseTicking = false;
//    }

//    //--------------------------------------------------------------------------------------
//    // Update: Function that calls each frame to update game objects.
//    //--------------------------------------------------------------------------------------
//    void Update()
//    {
//        // ActivateTimer decreases by 1 each frame (Grenade can only collide when this is lower than 1)
//        m_fCurrentActivateTimer -= 1;

//        // if the timer has set off
//        if (m_fFuseTimer <= 0)
//        {
//            //explode
//            GrenadeExplode();
//            GrenadeDisable();
//        } 
//        // if the grenade has hit the ground
//        if (m_bFuseTicking)
//        {
//            m_fFuseTimer -= 1 * Time.deltaTime;
//        }

//        if (m_fLerpTime < 1.0f)
//        {
//            m_rbGrenade.isKinematic = true;
//            Vector3 v3Pos = transform.position;
//            transform.position = BezierCurve.CalculateBezier(m_gSpawnPoint.transform.position, m_v3Target, m_fLerpTime, m_fArcHeight);

//            m_fLerpTime += Time.deltaTime * m_fSpeed;
//            if (m_fLerpTime > 1.0f)
//            {
//                m_fLerpTime = 1.0f;
//                m_rbGrenade.isKinematic = false;
//                m_rbGrenade.velocity = (transform.position - v3Pos).normalized * m_fBounceFactor;
//                Debug.Log(m_rbGrenade.velocity);
//                m_bFuseTicking = true;
//            }
//        }
//    }

//    //--------------------------------------------------------------------------------------
//    //  CalculateDamage: Calculates the damage so being further from the explosion results in less damage
//    //
//    //  Returns: the damage for the Soldiers within range to take
//    //
//    //--------------------------------------------------------------------------------------
//    private float CalculateDamage(Vector3 v3TargetPosition)
//    {
//        // create a vector from the shell to the target
//        Vector3 v3ExplosionToTarget = v3TargetPosition - transform.position;

//        // Calculated the distance from the shell to the target
//        float fExplosionDistance = v3ExplosionToTarget.magnitude;

//        // calculate the proportion of the Maximum distance the target is away
//        float fRelativeDistance = (m_fExplosionRadius - fExplosionDistance) / m_fExplosionRadius;

//        // Calculate damage as this proportion of the maximum possible damage
//        float fDamage = fRelativeDistance * m_fDamage;

//        fDamage = Mathf.Max(0f, fDamage);

//        return fDamage;
//    }

//    //--------------------------------------------------------------------------------------
//    //  GrenadeExplode: Finds all colliders in the m_fExplosionRadius and calls damage functions
//    //
//    //--------------------------------------------------------------------------------------
//    private void GrenadeExplode()
//    {
//        // Collect all possible colliders 
//        Collider[] aColliders = Physics.OverlapSphere(transform.position, m_fExplosionRadius, m_lmUnitMask);

//        for (int i = 0; i < aColliders.Length; i++)
//        {
//            Rigidbody rbTarget = aColliders[i].GetComponent<Rigidbody>();

//            //if it does not have a rigidbody
//            if (!rbTarget)
//            {
//                continue;
//            }

//            // if an object in collision zone is a Soldier
//            if (aColliders[i].gameObject.tag == "Soldier")
//            {
//                // TODO: Explosion particle effect here

//                SoldierActor gtarget = rbTarget.GetComponent<SoldierActor>();

//                // Soldier will take damage based on position (See CalculateDamge function below)
//                gtarget.TakeDamage(CalculateDamage(aColliders[i].transform.position));

//                // add explosive force for knockback 
//                // NOTE: May be replaced with a non-rigidbody knockback
//                rbTarget.AddExplosionForce(m_ExplosionForce, transform.position, m_fExplosionRadius, 0.0f, ForceMode.Impulse);
//            }
//        }
//    }

//    //--------------------------------------------------------------------------------------
//    //  GrenadeDisable: Disables grenade and resets values
//    //--------------------------------------------------------------------------------------
//    private void GrenadeDisable()
//    {
//        m_fCurrentActivateTimer = m_fMaxActivateTimer;
//        m_fFuseTimer = m_fMaxFuseTimer;
//        m_bFuseTicking = false;
//        gameObject.SetActive(false);
//    }
//}
