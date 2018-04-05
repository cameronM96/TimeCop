using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOV : MonoBehaviour {

    public float viewRadius; // view distance
    public float viewAngle; // field of view
    
    public Vector3 DirFromAngle (float angleInDegrees)
    {
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
