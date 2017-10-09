using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Tooltip("How many Health Points the projectile will subtract")]
    public float m_fDamage;
    [Tooltip("How much power the Soldier has given the projectile(affects distance)")]
    public float m_fPower;

	// Use this for initialization
	void Start()
    {
		
	}
	
	// Update is called once per frame
	void Update()
    {
	
	}

    // Fire function for overriding by every Weapon type
    public virtual void Fire(float fCharge) {}
}
