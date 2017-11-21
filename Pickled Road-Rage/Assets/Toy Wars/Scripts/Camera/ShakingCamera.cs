//--------------------------------------------------------------------------------------
// Purpose: Makes a Camera shaking effect for the projectiles to activate
//
// Description: Inheriting from MonoBehaviour, 
// The Camera shakes when it is set to true from elsewhere, then turns itself off with a timer
//
// Author: Callan Davies
//--------------------------------------------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakingCamera : MonoBehaviour
{
    // The duration of the shake
    [LabelOverride("Shake Duration")]
    [Tooltip("The duration of the shake")]
    public float m_fMaxDuration = 0.5f;

    // how severe the Shake is
    [LabelOverride("Shake Severity")]
    [Tooltip("how severe the Shake is.")]
    public float m_fAmount = 0.1f;

    // how quickly the camera will reach its destinations
    [LabelOverride("Lerp Speed")]
    [Tooltip("how quickly the camera will reach its destinations")]
    public float m_fLerpSpeed = 1.0f;

    // this bool can only become true from another script accessing its value
    [HideInInspector]
    public bool m_bIsShaking;

    // what the duration value will be at
    private float m_fCurrentDuration;

    private float m_fLerpTime;

    //--------------------------------------------------------------------------------------
    // Initialization.
    //--------------------------------------------------------------------------------------
    void Awake()
    {
        m_fCurrentDuration = 0.0f;
        m_fLerpTime = 0.0f;
	}

    //--------------------------------------------------------------------------------------
    // Update: Function that calls each frame to update game objects.
    //--------------------------------------------------------------------------------------
    void Update()
    {
        if (m_fLerpTime < 1.0f)
        {
            m_fLerpTime += Time.deltaTime * m_fLerpSpeed;
            if (m_fLerpTime > 1.0f)
            {
                m_fLerpTime = 1.0f;
            }
        }
        // if the camera should be shaking
        if (m_bIsShaking)
        {
            //Shake camera a little bit
            transform.localPosition = Vector3.Slerp(transform.localPosition, Random.insideUnitSphere * m_fAmount, m_fLerpTime);
            //Count up a timer 
            m_fCurrentDuration += Time.deltaTime;

            //If the timer is greater or equal to the Max Duration
            if (m_fCurrentDuration >= m_fMaxDuration)
            {

                //Set isShaking back to false
                m_bIsShaking = false;
                // and reset values
                m_fCurrentDuration = 0.0f;
                transform.localPosition =  Vector3.Slerp(transform.localPosition, Vector3.zero, m_fLerpTime);
            }
        }
    }
}


