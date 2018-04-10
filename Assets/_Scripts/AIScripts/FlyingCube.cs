using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class FlyingCube : MonoBehaviour {
    
    [SerializeField]
    protected float moveSpeed = 1;
    [SerializeField]
    protected float jumpForce = 10;

    private Rigidbody2D rb;

    [SerializeField]
    private Transform target;

    public float updateRate = 2f;

    //Caching
    private Seeker seeker;

    //The Calculated path
    public Path path;

    [HideInInspector]
    public bool pathIsEnded = false;

    public ForceMode2D fMode;

    //The max distance from the AI to a waypoint for it to continue to the next waypoint
    public float nextWayPointDistance = 3;

    // The way point we are currently moving towards.
    private int currentWayPoint = 0;

    // Use this for initialization
    void Start () {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        if (target != null)
            seeker.StartPath(transform.position, target.position, OnPathComplete);

        StartCoroutine(UpdatePath());
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        if (path == null)
            return;

        if (currentWayPoint >= path.vectorPath.Count)
        {
            if (pathIsEnded)
                return;

            Debug.Log("End of Path reached.");
            pathIsEnded = true;
            return;
        }

        pathIsEnded = false;

        //Direction to the next waypoint
        Vector2 dir = (path.vectorPath[currentWayPoint] - transform.position).normalized;
        
        if (path.vectorPath[currentWayPoint].y - transform.position.y > 0)
        {
            Debug.Log("Jumping");
            dir.y = jumpForce;
        } else
        {
            dir.y = 0;
        }

        dir *= moveSpeed * Time.fixedDeltaTime;
        Debug.Log(dir);
        //TODO: Move the AI
        rb.AddForce(dir,fMode);
        //rb.velocity = dir;

        float dist = Vector3.Distance(transform.position, path.vectorPath[currentWayPoint]);
        if (dist < nextWayPointDistance)
        {
            currentWayPoint++;
            return;
        }
    }

    public void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWayPoint = 0;
        }
    }

    IEnumerator UpdatePath()
    {
        if (target == null)
        {
            //TODO: insert a player search here.
            yield return null;
        }

        if (target != null)
        {
            seeker.StartPath(transform.position, target.transform.position, OnPathComplete);
            Debug.Log("Starting Path");
        }

        yield return new WaitForSeconds(1f / updateRate);
        StartCoroutine(UpdatePath());
    }
}
