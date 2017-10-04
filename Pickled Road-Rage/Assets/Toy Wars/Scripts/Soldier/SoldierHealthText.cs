using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoldierHealthText : MonoBehaviour
{
    public TextMesh m_tmHealthText;
    public SoldierActor m_gSoldier;
    string m_sHealth;

	// Use this for initialization
	void Start ()
    {
        m_tmHealthText = GetComponentInChildren<TextMesh>();
        m_tmHealthText.transform.localScale = transform.localScale.Inverse();  	
	}
	
	// Update is called once per frame
	void Update ()
    {
        m_sHealth = Convert.ToString((int)m_gSoldier.m_fCurrentHealth);
        m_tmHealthText.transform.rotation = Quaternion.Euler(90, 0, 0);
        m_tmHealthText.text = m_sHealth;
	}

}

public static class Vector3Extentions
{
    public static Vector3 Inverse(this Vector3 v)
    {
        return new Vector3(1f / v.x, 1f / v.y, 1f / v.z);
    }
}
