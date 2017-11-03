//--------------------------------------------------------------------------------------
// Purpose: Make a canvas face towards the camera.
//
// Description: Drag this script onto a canvas you wish to have face towards the camera. 
// Once script is applied to a canvas then drag on which camera you would like it to
// face towards.
// 
// Author: Thomas Wiltshire.
//--------------------------------------------------------------------------------------

// Using, etc
using UnityEngine;
using System.Collections;

//--------------------------------------------------------------------------------------
// CanvasFaceCamera object. Inheriting from MonoBehaviour. Used for making Canvas 
// face the camera.
//--------------------------------------------------------------------------------------
public class CanvasFaceCamera : MonoBehaviour
{
    // public camera object for the camera to face at.
    [LabelOverride("Camera to Face")] [Tooltip("What camera would you like the Canvas to face towards.")]
    public Camera m_cCamera;

    //--------------------------------------------------------------------------------------
    // Update: Function that calls each frame to update game objects.
    //--------------------------------------------------------------------------------------
    void Update()
    {
        // Face the camera.
        transform.LookAt(transform.position + m_cCamera.transform.rotation * Vector3.forward,
        m_cCamera.transform.rotation * Vector3.up);
    }
}