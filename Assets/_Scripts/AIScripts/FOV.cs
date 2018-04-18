using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOV : MonoBehaviour {

    public float viewRadius; // view distance
    [Range(0,360)]
    public float viewAngle; // field of view

    public LayerMask targetMask;
    public LayerMask obstaclesMask;

    //[HideInInspector]
    public List<GameObject> visibleTargets = new List<GameObject>();

    //TODO: Something in this script is cause it detected the player in areas within the radius it shouldn't
    // its definetly an issue with the trigonometry in either "FindVisibleTargets" or "DirFromAngle".

    void Start()
    {
        StartCoroutine("FindTargetWithDelay", 0.2f);
    }

    IEnumerator FindTargetWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }

    void FindVisibleTargets()
    {
        visibleTargets.Clear();
        Collider2D[] targetsInViewRadius = Physics2D.OverlapCircleAll(
            new Vector2(transform.position.x,transform.position.y), 
            viewRadius, 
            targetMask);

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            if (Vector3.Angle (transform.up,dirToTarget) < viewAngle/2)
            {
                float dstToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics2D.Raycast ( transform.position, dirToTarget, dstToTarget, obstaclesMask))
                {
                    visibleTargets.Add(target.gameObject);
                }
            }
        }
    }
    
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
