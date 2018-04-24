using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The following script was created by Cameron Mullins
public class FOV : MonoBehaviour {

    public float viewRadius; // view distance
    [Range(0,360)]
    public float viewAngle; // field of view

    public LayerMask targetMask;
    public LayerMask obstaclesMask;

    //[HideInInspector]
    public List<GameObject> visibleTargets = new List<GameObject>();
    
    void Start()
    {
        // This sets the refresh rate of this script
        StartCoroutine("FindTargetWithDelay", 0.2f);
    }

    IEnumerator FindTargetWithDelay(float delay)
    {
        // This is the refresh timer
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }

    void FindVisibleTargets()
    {
        visibleTargets.Clear();
        //Creates a physics cirlce around the AI to check of objects inside it
        Collider2D[] targetsInViewRadius = Physics2D.OverlapCircleAll(
            new Vector2(transform.position.x,transform.position.y), 
            viewRadius, 
            targetMask);

        // Loops through all objects found in the circle
        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;

            // Checks if that target is within the AI's field of vision.
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            if (Vector3.Angle (transform.up,dirToTarget) < viewAngle/2)
            {
                // determine if there are objects between the AI and the player using a raycast
                float dstToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics2D.Raycast ( transform.position, dirToTarget, dstToTarget, obstaclesMask))
                {
                    // Add target to visible targets (AI is aware of the player at this point)
                    visibleTargets.Add(target.gameObject);
                }
            }
        }
    }
    
    // This creates the AI's field of view (The cone that comes out in front of the AI)
    public Vector3 DirFromAngle (float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(
            Mathf.Sin(angleInDegrees * Mathf.Deg2Rad),
            Mathf.Cos(angleInDegrees * Mathf.Deg2Rad),
            0);
    }
}
