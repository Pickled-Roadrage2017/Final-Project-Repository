using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoldierHealthText : MonoBehaviour
{
    [HideInInspector]
    public TextMesh m_tmHealthText;
    // the soldier which is having its health represented
    public SoldierActor m_gSoldier;
    // health of the soldier converted to a string
    string m_sHealth;

    //public GameObject m_gParent;

	// Use this for initialization
	void Start ()
    {
      //  transform.SetParent(m_gParent.transform);
        m_tmHealthText = GetComponentInChildren<TextMesh>();
        m_tmHealthText = GetComponent<TextMesh>();
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
