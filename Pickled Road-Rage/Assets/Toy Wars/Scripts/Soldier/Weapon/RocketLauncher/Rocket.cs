//-------------------------------------------------------------------------------------------------
// Purpose: How the rocket weapon will act when spawned
//
// Description: Inheriting from Weapon.cs. Used for propelling rockets and their collison/damage numbers
//
// Author: Callan Davies
//-------------------------------------------------------------------------------------------------

// Using, etc
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Rocket : Weapon
{
    public GameObject m_gExplosion;

    [Header("Sounds")]
    [LabelOverride("Airtime Sound")]
    [Tooltip("Will play while the rocket is airborne")]
    public AudioClip m_acAirtimeSound;

    [LabelOverride("Explosion Sound")]
    [Tooltip("will play when the rocket explodes")]
    public AudioClip m_acExplosionSound;

    //A timer to stop the rocket from colliding with its Launcher
    [Space(10)]
    [LabelOverride("Activation Timer")]
    [Tooltip("Seconds it takes for the missle to not be within collision range of its Launcher")]
    public float m_fMaxActivateTimer = 2;

    // The force that hits back all Unit layered objects
    [Header("Explosion Variables")]
    [LabelOverride("Force of Explosion")]
    [Tooltip("The force that hits back all Unit layered objects")]
    public float m_ExplosionForce = 50;

    [LabelOverride("Direct Hit Modifier")]
    [Tooltip("The velocity is multiplied by this to reach an acceptable knockback")]
    public float m_fHitMultiplier = 0.5f;

    // Radius for the Area of Effect Explosion that should follow any Collision
    [LabelOverride("Radius of Explosion")]
    [Tooltip("Radius for the Area of Effect Explosion that should follow any Collision")]
    public float m_fExplosionRadius = 5f;

    // pointer to the RocketLauncher so it knows where to spawn
    [HideInInspector]
    public GameObject m_gSpawnPoint;

    // Starts at m_fMaxActivateTimer and ticks down to zero, then resetting upon being set Inactive
    [HideInInspector]
    public float m_fCurrentActivateTimer;

    // its own rigidbody
    [HideInInspector]
    public Rigidbody m_rbRocket;

    // this Rockets audioSource
    private AudioSource m_asAudioSource;
    //--------------------------------------------------------------------------------------
    // initialization.
    //--------------------------------------------------------------------------------------
    void Awake()
    {
        m_rbRocket = GetComponent<Rigidbody>();
    }

    //--------------------------------------------------------------------------------------
    // Update: Function that calls each frame to update game objects.
    //--------------------------------------------------------------------------------------
    void Update()
    {
        // ActivateTimer decreases by 1 each frame (Rocket can only collide when this is lower than 1)
        m_fCurrentActivateTimer -= 1;

    }

    //--------------------------------------------------------------------------------------
    // OnTriggerEnter: When a rocket Collides (Is Trigger)
    //
    // Param: other is the other object it is colliding with at call
    //
    //--------------------------------------------------------------------------------------
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "SoldierWall")
        {
            Physics.IgnoreCollision(GetComponent<Collider>(), other);
            return;
        }
        // if the rocket can now activate
        if (m_fCurrentActivateTimer <= 0)
        {

            // Rocket Explodes, damaging anything within the radius
            GameObject gExplosion = Instantiate(m_gExplosion);
            gExplosion.transform.SetParent(null);
            gExplosion.transform.position = transform.position;
            RocketExplode();

                // if the rocket directly hits a Soldier, this will be called to apply knockback
                if (other.tag == "Soldier")
                {
                    Rigidbody rbTarget = other.GetComponent<Rigidbody>();
                    rbTarget = other.GetComponent<Rigidbody>();
                    // Directly knockback the Soldier, 
                    // NOTE: This soldier would of already taken damage from the RocketExplode() function
                    rbTarget.AddForce(m_rbRocket.velocity * m_fHitMultiplier, ForceMode.Impulse);
                }
                if (other.tag == "Teddy")
                {
                    other.GetComponent<Teddy>().TakeDamage(m_fDamage);
                }

                // Disable Rocket after it has completed all damage dealing and knockbacks
                RocketDisable();
            
        }
    }

    //--------------------------------------------------------------------------------------
    //  CalculateDamage: Calculates the damage so being further from the explosion results in less damage
    //
    //  Returns: the damage for the Soldiers within range to take
    //
    //--------------------------------------------------------------------------------------
    private float CalculateDamage(Vector3 v3TargetPosition)
    {
        // create a vector from the shell to the target
        Vector3 v3ExplosionToTarget = v3TargetPosition - transform.position;

        // Calculated the distance from the shell to the target
        float fExplosionDistance = v3ExplosionToTarget.magnitude;

        // calculate the proportion of the Maximum distance the target is away
        float fRelativeDistance = (m_fExplosionRadius - fExplosionDistance) / m_fExplosionRadius;

        // Calculate damage as this proportion of the maximum possible damage
        float fDamage = fRelativeDistance * m_fDamage;

        fDamage = Mathf.Max(0f, fDamage);

        return fDamage;
    }


    private void RocketExplode()
    {
        // Collect all possible colliders 
        Collider[] aSoldierColliders = Physics.OverlapSphere(transform.position, m_fExplosionRadius, m_UnitMask);

        for (int i = 0; i < aSoldierColliders.Length; i++)
        {
            Rigidbody rbTarget = aSoldierColliders[i].GetComponent<Rigidbody>();

            //if it does not have a rigidbody
            if (!rbTarget)
            {
                continue;
            }

            // if an object in collision zone is a Soldier
            if (aSoldierColliders[i].gameObject.tag == "Soldier")
            {
                // TODO: Explosion particle effect here

                SoldierActor gtarget = rbTarget.GetComponent<SoldierActor>();

                // Soldier will take damage based on position (See CalculateDamge function below)
                gtarget.TakeDamage(CalculateDamage(aSoldierColliders[i].transform.position));

                // add explosive force for knockback 
                // NOTE: May be replaced with a non-rigidbody knockback
                rbTarget.AddExplosionForce(m_ExplosionForce, transform.position, m_fExplosionRadius, 0.0f, ForceMode.Impulse);
            }
           
        }
    }

    //--------------------------------------------------------------------------------------
    //  RocketDisable: Disables rocket and allows for resetting of variables if needed
    //
    //--------------------------------------------------------------------------------------
    private void RocketDisable()
    {
        gameObject.SetActive(false);
    }
}
