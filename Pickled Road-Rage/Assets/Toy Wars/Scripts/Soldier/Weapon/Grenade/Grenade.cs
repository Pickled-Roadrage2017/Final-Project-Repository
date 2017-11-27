//--------------------------------------------------------------------------------------
// Purpose: How the grenade weapon will act when spawned
//
// Description: Inheriting from Weapon.cs, Used for propelling grenades and their collison/damage numbers 
//
// Author: Callan Davies
//--------------------------------------------------------------------------------------

// Using, etc
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Grenade : Weapon
{
    public GameObject m_gExplosion;

    [Header("Sounds")]
    [LabelOverride("Fuse Sound")]
    [Tooltip("will play when the grenade is counting down to explosion")]
    public AudioClip m_acFuseSound;

    //may or may not be used
    [LabelOverride("Bounce Sound")]
    [Tooltip("will play when the grenade bounces")]
    public AudioClip m_acBounceSound;

    //A timer to stop the grenade from colliding with its Launcher
    [Space(10)]
    [LabelOverride("Activation Timer")]
    [Tooltip("Seconds it takes for the grenade to not be within collision range of its Launcher")]
    public float m_fMaxActivateTimer = 2;

    [Header("Grenade-specific Variables")]
    [LabelOverride("Grenade Fuse")]
    [Tooltip("The grenade will explode when this reaches zero")]
    public float m_fMaxFuseTimer = 4;

    [LabelOverride("Initial Bounce")]
    [Tooltip("The grenade will go upwards by this value on initial bounce")]
    public float m_fBounciness = 0;


    [LabelOverride("After-Bounce Drag")]
    [Tooltip("The grenades drag will be set to this after its first bounce")]
    public float m_fBounceDrag;

    // The force that hits back all Unit layered objects
    [Header("Explosion Variables")]
    [LabelOverride("Explosion Force")]
    [Tooltip("The force that hits back all Unit layered objects")]
    public float m_ExplosionForce = 50.0f;

    // Radius for the Area of Effect Explosion that should follow any Collision
    [LabelOverride("Explosion Radius")]
    [Tooltip("Radius for the Area of Effect Explosion that should follow any Collision")]
    public float m_fSoldierExplosionRadius = 5f;

    //Radius for the Area of Effect Explosion that will find only Teddys
    [LabelOverride("Teddy Explosion Radius")]
    [Tooltip("Radius for the Area of Effect Explosion that will find only Teddys")]
    public float m_fTeddyExplosionRadius = 10f;

    [LabelOverride("Direct Hit Modifier")]
    [Tooltip("The velocity is multiplied by this to reach an acceptable knockback")]
    public float m_fHitMultiplier = 0.5f;

    // pointer to the GrenadeLauncher so it knows where to spawn
    [HideInInspector]
    public GameObject m_gSpawnPoint;

    // Starts at m_fMaxActivateTimer and ticks down to zero, then resetting upon being set Inactive
    [HideInInspector]
    public float m_fCurrentActivateTimer;

    // boolean to tell the camera to activate a shake animation
    [HideInInspector]
    public bool m_bCameraShakeAni;

    // boolean to tell the grenade to disable itself.
    [HideInInspector]
    public bool m_bDisable = false;

    // its own rigidbody
    private Rigidbody m_rbGrenade;

    // boolean for if the grenade should be counting down to explosion
    private bool m_bFuseTicking;

    // When this is less than or equal to zero, the grenade will explode
    private float m_fFuseTimer;

    // this Grenades audioSource
    private AudioSource m_asAudioSource;

    [HideInInspector]
    public float m_fLifespan = 600;
    //--------------------------------------------------------------------------------------
    // initialization.
    //--------------------------------------------------------------------------------------
    void Awake()
    {
        m_asAudioSource = GetComponent<AudioSource>();
        m_rbGrenade = GetComponent<Rigidbody>();
        m_fFuseTimer = m_fMaxFuseTimer;
        m_bFuseTicking = false;
    }

    //--------------------------------------------------------------------------------------
    // Update: Function that calls each frame to update game objects.
    //--------------------------------------------------------------------------------------
    void Update()
    {
      
        // ActivateTimer decreases by 1 each frame (Grenade can only collide when this is lower than 1)
        m_fCurrentActivateTimer -= 1;
        m_fLifespan -= 1;
        // if the timer has set off
        if (m_fFuseTimer <= 0)
        {
            
            // if the grenade is not supposed to be disabled yet
            if (!m_bDisable)
            {
                // the camera should shake
                m_bCameraShakeAni = true;
                // and the grenade should explode
                GrenadeExplode();
            }
            // if the grenade should be disbabled this update.
            if (m_bDisable)
            {
                GrenadeDisable();
            }
           
        } 
        // if the grenade has hit the ground
        if (m_bFuseTicking)
        {
            m_fFuseTimer -= 1 * Time.deltaTime;
            
        }
        // if the camera should shake
        if (m_bCameraShakeAni)
        {
            Camera.main.GetComponent<ShakingCamera>().m_bIsShaking = true;
            m_bCameraShakeAni = false;
            m_bDisable = true;
        }

       if (m_fLifespan <= 0)
        {
            GrenadeDisable();
        }

    }


    //--------------------------------------------------------------------------------------
    // OnCollisionEnter: Called when a grenade bounces, applying bonus force on the intial bounce 
    //                   Sets m_bFuseTicking to true on initial bounce, 
    //                   which means the grenade is counting down to explosion.             
    //
    //--------------------------------------------------------------------------------------
    private void OnCollisionEnter()
    {
       
        if (m_fCurrentActivateTimer <= 0)
        {
            if (!m_bFuseTicking)
            {
                m_asAudioSource.PlayOneShot(m_acBounceSound);
                m_rbGrenade.AddForce(new Vector3(0, m_fBounciness, 0), ForceMode.Impulse);
                m_bFuseTicking = true;
                m_rbGrenade.drag = m_fBounceDrag;
            }
          
        }
        else
        {
            return;
        }
    }

    //--------------------------------------------------------------------------------------
    //  GrenadeExplode: Finds all colliders in the m_fExplosionRadius and calls damage functions,
    //                  and knockback in the Soldiers case.
    //
    //--------------------------------------------------------------------------------------
    private void GrenadeExplode()
    {
        // Collect all possible colliders 
        Collider[] aSoldierColliders = Physics.OverlapSphere(transform.position, m_fSoldierExplosionRadius, m_lmUnitMask);
        Collider[] aTeddyColliders = Physics.OverlapSphere(transform.position, m_fTeddyExplosionRadius, m_lmTeddyMask);

        for (int i = 0; i < aSoldierColliders.Length; i++)
        {
            Rigidbody rbTarget = aSoldierColliders[i].GetComponent<Rigidbody>();

            //if it does not have a rigidbody
            if (!rbTarget)
            {
                continue;
            }
            if (!Physics.Linecast(transform.position, rbTarget.position, m_lmEnvironmentMask))
            {
                SoldierActor gtarget = rbTarget.GetComponent<SoldierActor>();

                // Soldier will take damage based on position (See CalculateDamge function below)
                gtarget.TakeDamage(CalculateDamage(aSoldierColliders[i].transform.position,m_fSoldierExplosionRadius));

                // add explosive force for knockback 
                // NOTE: May be replaced with a non-rigidbody knockback
                if (CalculateDamage(aSoldierColliders[i].transform.position, m_fSoldierExplosionRadius) > 0)
                {
                    rbTarget.AddExplosionForce(m_ExplosionForce, transform.position, m_fSoldierExplosionRadius, 0.0f, ForceMode.Impulse);
                }
            }
        }

        for (int i = 0; i < aTeddyColliders.Length; i++)
        {
            Rigidbody rbTarget = aTeddyColliders[i].GetComponent<Rigidbody>();

            //if it does not have a rigidbody
            if (!rbTarget)
            {
                continue;
            }

            Teddy gtarget = rbTarget.GetComponent<Teddy>();

            // Teddy will take damage based on position (See CalculateDamge function below)
            gtarget.TakeDamage(CalculateDamage(aTeddyColliders[i].transform.position,m_fTeddyExplosionRadius));
        }
    }

    //--------------------------------------------------------------------------------------
    //  GrenadeDisable: Disables grenade and resets values
    //--------------------------------------------------------------------------------------
    private void GrenadeDisable()
    {
        m_bCameraShakeAni = false;
        // set off explosion effect.
        GameObject gExplosion = Instantiate(m_gExplosion);
        gExplosion.transform.SetParent(null);
        gExplosion.transform.position = transform.position;
        Destroy(gExplosion, 5f);
        m_fCurrentActivateTimer = m_fMaxActivateTimer;
            m_fFuseTimer = m_fMaxFuseTimer;
            m_bFuseTicking = false;
            gameObject.SetActive(false);
        
    }



        
    
}
