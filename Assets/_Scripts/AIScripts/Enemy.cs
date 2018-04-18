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
        private bool m_Jump = false;
        private bool m_Crouch = false;

        private IEnemyState currentState;

        public float h = 0; // This is the same as GetAxis("Horizontal")

        private Rigidbody2D rb;

        public GameObject Target { get; set; }

        public float updateRate = 2f;

        [SerializeField]
        protected float jumpForce = 10;

        //Caching
        private Seeker seeker;
        private FOV fieldOfView;

        //The Calculated path
        public Path path;
        private int currentWayPoint = 0;
        //The max distance from the AI to a waypoint for it to continue to the next waypoint
        public float nextWayPointDistance = 3;

        [HideInInspector]
        public bool pathIsEnded = false;

        // Use this for initialization
        public void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            ChangeState(new IdleState());

            m_Character = GetComponent<PlatformerCharacter2D>();

            seeker = GetComponent<Seeker>();
            fieldOfView = GetComponent<FOV>();
            if (Target != null)
                seeker.StartPath(transform.position, Target.transform.position, OnPathComplete);

            StartCoroutine(UpdatePath());
        }

        // Update is called once per frame
        void Update()
        {
            currentState.Execute();

            foreach (GameObject target in fieldOfView.visibleTargets)
            {
                // There will only ever be 1 possible target (as there is only 1 player)
                // but in future you could put logic here to determine what to do if there is more.
                Target = target;
            }
        }

        private void FixedUpdate()
        {
            if (path != null)
            {
                if (currentWayPoint >= path.vectorPath.Count)
                {
                    if (pathIsEnded)
                        return;

                    Debug.Log("End of Path reached.");
                    pathIsEnded = true;
                    return;
                }

                pathIsEnded = false;

                if (path.vectorPath[currentWayPoint].y - transform.position.y > 0)
                {
                    Debug.Log("Jumping");
                    m_Jump = true;
                }
                else
                {
                    m_Jump = false;
                }

                float dist = Vector3.Distance(transform.position, path.vectorPath[currentWayPoint]);
                if (dist < nextWayPointDistance)
                {
                    currentWayPoint++;
                    return;
                }
            }

            Move(h,m_Jump);
        }

        public void ChangeState(IEnemyState newState)
        {
            if (currentState != null)
            {
                currentState.Exit();
            }

            currentState = newState;

            currentState.Enter(this);
            Debug.Log(currentState);
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

        public void Move(float h, bool jump)
        {
            m_Character.Move(h, m_Crouch, jump);
        }
    }
}
