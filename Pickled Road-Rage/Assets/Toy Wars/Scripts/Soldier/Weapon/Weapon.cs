using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [LabelOverride("Mask for Knockback")]
    [Tooltip("Set this to the Unit layer, so the Grenade doesn't knockback objects that should be stationary")]
    public LayerMask m_UnitMask;

    [LabelOverride("Damage")][Tooltip("How many Health Points the projectile will subtract")]
    public float m_fDamage;
    // Power for the Rocket and Grenade, Minigun will not use this variable 
    [HideInInspector]
    public float m_fPower;

    // Fire function for overriding by every Weapon type
    public virtual void Fire(float fCharge)
    {

    }
}
