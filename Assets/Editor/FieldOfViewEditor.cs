﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// The following script was created by Cameron Mullins
// This script creates the lines seen in the editor to display the AI's field of view
[CustomEditor (typeof (FOV))]
public class FieldOfViewEditor : Editor {

    void OnSceneGUI ()
    {
        FOV fow = (FOV)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fow.transform.position, Vector3.forward, Vector3.up, 360, fow.viewRadius);
        Vector3 viewAngleA = fow.DirFromAngle(-fow.viewAngle / 2, false);
        Vector3 viewAngleB = fow.DirFromAngle(fow.viewAngle / 2, false);

        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleA * fow.viewRadius);
        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleB * fow.viewRadius);

        Handles.color = Color.red;
        foreach (GameObject visibleTarget in fow.visibleTargets)
        {
            Handles.DrawLine(fow.transform.position, visibleTarget.transform.position);
        }
    }
}
