using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    public int m_nDamage;
    public float m_nPower;
    public float m_fSpeed;

	// Use this for initialization
	void Start()
    {
		
	}
	
	// Update is called once per frame
	void Update()
    {
	
	}

    public virtual void Fire() {}
}
