//--------------------------------------------------------------------------------------
// Purpose: 
//
// Description: 
//
// Author: Callan Davies
//--------------------------------------------------------------------------------------

//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class BezierCurve
//{

//    //--------------------------------------------------------------------------------------
//    //  CalculateBezier: Calculates a bezier lerp from vector3 to another at the speed of a float
//    //
//    //  Params: 
//    //          v3Start: The start of the lerp 
//    //          v3End: The final destination of the lerp
//    //          fSpeed: The speed that the object will travel between the lerp points
//    //
//    //  Returns: 
//    //          A lerp which will make an object arc towards its destination
//    //
//    //--------------------------------------------------------------------------------------
//    public static Vector3 CalculateBezier(Vector3 v3Start, Vector3 v3End, float fSpeed, float fArcHeight)
//    {
//        // speed should never be higher than 1.0f
//        fSpeed = Mathf.Clamp01(fSpeed);
//        // the control if to be set halfway between the start and finish of the lerp
//        Vector3 v3Control = (v3Start + v3End) / 2;
//        // The height variable from the Launcher should raise the curve (e.g 0 will mean there is no curve)
//        v3Control.y = fArcHeight;

//        Debug.DrawLine(v3Start, v3Start + Vector3.up);
//        Debug.DrawLine(v3End, v3End + Vector3.up);
//        Debug.DrawLine(v3Control, v3Control + Vector3.up);

//        Vector3 A1 = Vector3.Lerp(v3Start, v3Control, fSpeed);

//        Vector3 A2 = Vector3.Lerp(v3Control, v3End, fSpeed);

//        return Vector3.Lerp(A1, A2, fSpeed);
//    }
//}
