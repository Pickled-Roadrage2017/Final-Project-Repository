using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//--------------------------------------------------------------------------------------
// Rocket: Inheriting from Weapon.cs. Used for propelling rockets and their collison/damage numbers
//--------------------------------------------------------------------------------------
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
    public float m_fMaxActivateTimer  = 2;

    // Starts at m_fMaxActivateTimer and ticks down to zero, then resetting upon being set Inactive
    [HideInInspector]
    public float m_fCurrentActivateTimer;

    // its own rigidbody
    Rigidbody m_rbRocket;
    // pointer to the RocketLauncher so it knows where to spawn
    [HideInInspector]
    public GameObject m_gSpawnPoint;

    //To be set to its SpawPoints rocketLauncher script in the Awake function
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
    
    // The force that hits back all Unit layered objects
    [Tooltip("The force that hits back all Unit layered objects")]
    public float m_ExplosionForce = 1000f;

    //--------------------------------------------------------------------------------------
    // initialization.
    //--------------------------------------------------------------------------------------
    void Awake()
    {
        // Gets the code of the spawning RocketLauncher GameObject
        m_gRocketLauncher = m_gSpawnPoint.GetComponent<RocketLauncher>();
        // Set the rockets power variable to the power variable of the Launcher which was passed down by the Soldier
        m_fPower = m_gRocketLauncher.m_fPower;
        // Get the rockets Rigidbody
        m_rbRocket = GetComponent<Rigidbody>();
        // Set the direction for the rocket to go to the forward vector of the RocketLauncher at the time of firing
        m_v3MoveDirection = m_gSpawnPoint.transform.forward;
        // Initilize AirDrop 
        m_fAirDrop = 0;
        // CurrentLifespan should start out as equal to MaxLifespan
        m_fCurrentLifespan = m_fMaxLifespan;
        // CurrentActivateTimer should start out as equal to the MaxActivateTimer
        m_fCurrentActivateTimer = m_fMaxActivateTimer;
    }

    //--------------------------------------------------------------------------------------
    // Update: Function that calls each frame to update game objects.
    //--------------------------------------------------------------------------------------
    private void Update()
    {
        m_fCurrentActivateTimer -= 1;
        // Decreases the airDrop value by the iterator, this allows the "gravity" of the arc
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
        Debug.Log("COLLISION");
        // if the rocket can now activate
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
                    // TODO: Replace this line with more predictible coded "physics" possibly
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
