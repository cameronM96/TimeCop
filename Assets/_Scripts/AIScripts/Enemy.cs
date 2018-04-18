using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

namespace UnityStandardAssets._2D
{
    [RequireComponent(typeof(PlatformerCharacter2D))]
    public class Enemy : MonoBehaviour
    {

        //Enemy is based off of these tutorials: https://arongranberg.com/astar/ , https://www.youtube.com/watch?v=4T7KHysRw84 , https://www.youtube.com/watch?v=XLgMzg30Qcg&t=14s
        private PlatformerCharacter2D m_Character;
        private bool m_Jump;

        private IEnemyState currentState;
        float h = 0; // This is the same as GetAxis("Horizontal")

        private Rigidbody2D rb;

        public GameObject Target { get; set; }

        public float updateRate = 2f;

        [SerializeField]
        protected float jumpForce = 10;

        //Caching
        private Seeker seeker;

        //The Calculated path
        public Path path;
        private int currentWayPoint = 0;
        //The max distance from the AI to a waypoint for it to continue to the next waypoint
        public float nextWayPointDistance = 3;

        [HideInInspector]
        public bool pathIsEnded = false;

        // Use this for initialization
        public void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            ChangeState(new IdleState());
            Target = GameObject.FindGameObjectWithTag("Player");

            seeker = GetComponent<Seeker>();
            if (Target != null)
                seeker.StartPath(transform.position, Target.transform.position, OnPathComplete);

            StartCoroutine(UpdatePath());
        }

        // Update is called once per frame
        void Update()
        {
            currentState.Execute();

            LookAtTarget();
        }

        private void FixedUpdate()
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

            //direction is calculated here
            //m_Character.Move(h, false, m_Jump);

            if (path.vectorPath[currentWayPoint].y - transform.position.y > 0)
            {
                Debug.Log("Jumping");
                rb.AddForce(new Vector2(0, jumpForce));
            }

            float dist = Vector3.Distance(transform.position, path.vectorPath[currentWayPoint]);
            if (dist < nextWayPointDistance)
            {
                currentWayPoint++;
                return;
            }
        }

        private void LookAtTarget()
        {
            if (Target != null)
            {
                float xDir = Target.transform.position.x - transform.position.x;

                if (xDir < 0 )
                {
                    h = 1;
                } else if (xDir > 0)
                {
                    h = -1;
                } else
                {
                    h = 0;
                }
            }
        }

        public void ChangeState(IEnemyState newState)
        {
            if (currentState != null)
            {
                currentState.Exit();
            }

            currentState = newState;

            currentState.Enter(this);
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            currentState.OnTriggerEnter(other);
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
            if (Target == null)
            {
                //TODO: insert a player search here.
                yield return null;
            }

            if (Target != null)
            {
                seeker.StartPath(transform.position, Target.transform.position, OnPathComplete);
                Debug.Log("Starting Path");
            }

            yield return new WaitForSeconds(1f / updateRate);
            StartCoroutine(UpdatePath());
        }
    }
}
