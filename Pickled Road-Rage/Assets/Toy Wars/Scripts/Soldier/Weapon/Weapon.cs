using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [LabelOverride("Soldier Layer")]
    [Tooltip("Set this to the Unit layer, so the weapons know they can knockback these objects")]
    public LayerMask m_lmUnitMask;

    [LabelOverride("Teddy Layer")]
    [Tooltip("Set this to the Bear layer, so the weapons know that this cannot be knocked back")]
    public LayerMask m_lmTeddyMask;

    [LabelOverride("Environment Layer")]
    [Tooltip("Anything on this layer will block an explosion")]
    public LayerMask m_lmEnvironmentMask;

    [LabelOverride("Damage")][Tooltip("How many Health Points the projectile will subtract")]
    public float m_fDamage;
    // Power for the Rocket and Grenade, Minigun will not use this variable 
    [HideInInspector]
    public float m_fPower;

    // Fire function for overriding by every Weapon type
    public virtual void Fire(float fCharge)
    {

    }


    //--------------------------------------------------------------------------------------
    //  CalculateDamage: Calculates the damage so being further from the explosion results in less damage
    //
    //  Returns: the damage for the Soldiers/Teddy within range to take
    //
    //--------------------------------------------------------------------------------------
    public float CalculateDamage(Vector3 v3TargetPosition, float fExplosionRadius)
    {
        // create a vector from the shell to the target
        Vector3 v3ExplosionToTarget = v3TargetPosition - transform.position;

        // Calculated the distance from the shell to the target
        float fExplosionDistance = v3ExplosionToTarget.magnitude;

        // calculate the proportion of the Maximum distance the target is away
        float fRelativeDistance = (fExplosionRadius - fExplosionDistance) / fExplosionRadius;

        // Calculate damage as this proportion of the maximum possible damage
        float fDamage = fRelativeDistance * m_fDamage;

        fDamage = Mathf.Max(0f, fDamage);

        return fDamage;
    }
}
