using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : Weapon
{
    // Gets decreased by m_fDropIterator each Update, then works towards the arc of the rocket
    [HideInInspector]
    public float m_fAirDrop;
    // How much m_fAirTime gets increased per frame
    [Tooltip("This is added into the downward pull of the arc, increasing this will decrease the time it takes to start falling")]
    public float m_fDropIterator = 0.05f;

    //A timer to stop the rocket from colliding with its Launcher
    [Tooltip("Seconds it takes for the missle to not be within collision range of its Launcher")]
    public float m_fActivateTimer  = 2;

    [HideInInspector]
    public float m_fCurrentActivateTimer;

    Rigidbody m_rbRocket;
    // pointer to the RocketLauncher so it knows where to spawn
    [HideInInspector]
    public GameObject m_gSpawnPoint;

    RocketLauncher m_gRocketLauncher;

    // The timer for the rocket to delete
    // NOTE: Should only be as high as the time it would take a rocket to travel across the entire map    
    [Tooltip("Set this to the time it would take a rocket to travel across the entire map")]
    public float m_fMaxLifespan;

    // The current lifepsan is what is effected in the update, to allow easier reinitilisation
    [Tooltip("Not Advisible to edit this, only public so you can see how much time the rocket has left")]
    public float m_fCurrentLifespan;

    // The direction the Rocket should move
    [HideInInspector]
    public Vector3 m_v3MoveDirection;

    // Radius for the Area of Effect Explosion that should follow any Collision
    [Tooltip("Radius for the Area of Effect Explosion that should follow any Collision")]
    public float m_fExplosionRadius = 5f;

    [Tooltip("Set this to the Unit layer, so the Rocket doesn't effect objects that should be stationary")]
    public LayerMask m_UnitMask;
    
    //Of no use currently
    //public ParticleSystem m_psExplosionParticles;

    // The force that hits back all Unit layered objects
    [Tooltip("The force that hits back all Unit layered objects")]
    public float m_ExplosionForce = 1000f;

    // Initiliazation
    void Start()
    {
        m_gRocketLauncher = m_gSpawnPoint.GetComponent<RocketLauncher>();
        // Set the rockets power variable to the power variable of the Launcher which was passed down by the Soldier
        m_fPower = m_gRocketLauncher.m_fPower;
        m_rbRocket = GetComponent<Rigidbody>();
        m_v3MoveDirection = m_gSpawnPoint.transform.forward;
        m_fAirDrop = 0;
        m_fCurrentLifespan = m_fMaxLifespan;
        m_fCurrentActivateTimer = m_fActivateTimer;
    }

    private void FixedUpdate()
    {
        m_fCurrentActivateTimer -= 1;
        // Decreases the airDrop value by the iterator, this allows the fake "gravity" of the arc
        m_fAirDrop -= m_fDropIterator;
        m_fCurrentLifespan -= Time.deltaTime;
        if (m_fCurrentLifespan < 0)
        {
            gameObject.SetActive(false);
            m_gRocketLauncher.m_bRocketAlive = false;
        }

        m_rbRocket.AddForce(m_v3MoveDirection * m_fPower);
         //m_rbRocket.velocity = m_v3MoveDirection * m_fPower;
        // This will start pulling the rocket downwards after it reaches the initial y value of it
        m_rbRocket.AddForce(new Vector3(0, m_fAirDrop, 0));
        // Makes the rocket point towards its y-axis
        transform.LookAt(transform.position + m_rbRocket.velocity);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(m_fCurrentActivateTimer <= 0)
        { 
          
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
                    // TODO: Explosion effect
                    SoldierActor gtarget = rbTarget.GetComponent<SoldierActor>();

                    if (!gtarget)
                    {
                        continue;
                    }
                    gtarget.TakeDamage(CalculateDamage(aColliders[i].transform.position));

                }
                gameObject.SetActive(false);
                m_gRocketLauncher.m_bRocketAlive = false;
            
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
