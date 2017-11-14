using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeRotation : MonoBehaviour
{
    public Quaternion x;

	// Use this for initialization
	void Awake()
    {
       
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.rotation = x;
	}
}
