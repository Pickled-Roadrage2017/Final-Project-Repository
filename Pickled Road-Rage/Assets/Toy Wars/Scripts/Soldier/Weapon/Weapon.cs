using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Tooltip("How many Health Points the projectile will subtract")]
    public int m_nDamage;
    [Tooltip("How much power the Soldier has given the projectile(affects distance)")]
    public float m_fPower;
    [Tooltip("How fast the projectile travels")]
    public float m_fSpeed;

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
