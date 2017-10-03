using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : Weapon
{
    // How long the Rocket has been alive
    public float m_fCurrentDropSpeed;
    // How much m_fAirTime gets increased per frame
    public float m_fDropIterator = 0.05f;

    Rigidbody m_rbRocket;
    // pointer to the RocketLauncher so it knows where to spawn
    GameObject m_gLauncher;

    RocketLauncher m_gRocketLauncher;

    // The timer for the rocket to delete
    // NOTE: Should only be as high as the time it would take a rocket to travel across the entire map    
    [Tooltip("Set this to the time it would take a rocket to travel across the entire map")]
    public float m_fLifespan;

    // The direction the Rocket should move
    Vector3 m_v3MoveDirection;

    // Speed at which the rocket goes towards the ground
    [Tooltip("Speed at which the rocket goes towards the ground")]
    public float m_fFallSpeed;


    // Radius for the Area of Effect Explosion that should follow any Collision
    [Tooltip("Radius for the Area of Effect Explosion that should follow any Collision")]
    public float m_fExplosionRadius = 5f;

    public LayerMask m_UnitMask;
    public ParticleSystem m_psExplosionParticles;

    public float m_ExplosionForce = 1000f;

    // Use this for initialization
    void Start()
    {
        m_gLauncher = GameObject.FindGameObjectWithTag("RocketLauncher");
        m_gRocketLauncher = m_gLauncher.GetComponent<RocketLauncher>();
        // Set the rockets power variable to the power variable of the Launcher which was passed down by the Soldier
        m_fPower = m_gRocketLauncher.m_fPower;
        m_rbRocket = GetComponent<Rigidbody>();
        m_v3MoveDirection = m_gLauncher.transform.forward;
        m_fCurrentDropSpeed = 0;
    }

    private void FixedUpdate()
    {
        m_fCurrentDropSpeed -= m_fDropIterator;
        m_fLifespan -= Time.deltaTime;
        if (m_fLifespan < 0)
        {
            Destroy(gameObject);
        }

        // Bullet recieves direction from the Soldier
       // m_rbRocket.velocity = m_fPower * m_v3MoveDirection;
        m_rbRocket.AddForce(m_v3MoveDirection * m_fPower);
        m_rbRocket.AddForce(new Vector3(0, m_fCurrentDropSpeed, 0));
        transform.LookAt(transform.position + m_rbRocket.velocity);
    }

    private void OnTriggerEnter(Collider other)
    {
        // If the Rocket hits Terrain (Everything that is not a Soldier or a Tessy)
        if (other.tag == "Terrain")
        {
            //TODO: Explosion Radius should allow you to damage a Soldier without directly hitting them
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

                // add explosive force
                // TODO: Replace this line with more predictible coded "physics"
                rbTarget.AddExplosionForce(m_ExplosionForce, transform.position, m_fExplosionRadius);

                SoldierActor gtarget = rbTarget.GetComponent<SoldierActor>();

                if (!gtarget)
                {
                    continue;
                }
                gtarget.TakeDamage(CalculateDamage(aColliders[i].transform.position));
                
            }

            Debug.Log("Hit Terrain");
            Destroy(gameObject);
        }
        // If the Rocket Collides with a Soldier, be it friend or foe
        if (other.tag == "Soldier")
        {
            // TODO: Subtract full damage value from others health
            // TODO: Explosion Radius should allow the damaging of more than one soldier, with the damage being less the further from the point of collision
            Debug.Log("Hit Soldier");
            
            Destroy(gameObject);
        }

        // When the Rocket collides with its firer
        if (other.tag == "CurrentSoldier")
        {
            // This should remain as a nothing, CurrentSoldier should never be able to shoot themself.
            // NOTE: CurrentSoldier should be able to damage themself with the Explosion Radius
            Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), other.GetComponent<Collider>());
            Debug.Log("Collided with Firer");
        }

        // If the Rocket collides with a Teddy Bear
        if (other.tag == "TeddyBear")
        {

            Destroy(gameObject);
        }

       
    }


    private void Explosion()
    {

    }

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
