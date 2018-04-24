using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

namespace UnityStandardAssets._2D
{
    // The following script was created by Cameron Mullins
    [RequireComponent(typeof(PlatformerCharacter2D))]
    public class Enemy : MonoBehaviour
    {
        //Enemy is based off of these tutorials: https://arongranberg.com/astar/ , https://www.youtube.com/watch?v=4T7KHysRw84 , https://www.youtube.com/watch?v=XLgMzg30Qcg&t=14s
        private PlatformerCharacter2D m_Character;
        private bool m_Jump = false;
        private bool m_Crouch = false;

        private IEnemyState currentState;

        public float h = 0; // This is the same as GetAxis("Horizontal")

        public GameObject Target { get; set; }

        [SerializeField]
        private float updateRate = 2f;

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

        public ScoreManager scoreManager;
        public bool knight;
        public bool ninja;
        public bool juggernaut;
        [HideInInspector]
        public bool attack;
        public float attackCooldown;
        public float attackRange;
        [HideInInspector]
        public bool specialAttack;
        public float specialAttackCooldown;

        private float jumpTimer = 0.1f;
        //[SerializeField]
        //private float jumpTimerReset = 0.1f;

        // Projectile
        public GameObject projectilePrefab;
        [SerializeField]
        private Transform projectilePoint;

        // Use this for initialization
        public void Awake()
        {
            // Setting up references.
            ChangeState(new IdleState());

            m_Character = GetComponent<PlatformerCharacter2D>();

            seeker = GetComponent<Seeker>();
            fieldOfView = GetComponent<FOV>();
            if (Target != null)
                seeker.StartPath(m_Character.m_GroundCheck.transform.position, Target.transform.position, OnPathComplete);

            // Starts path finding script
            StartCoroutine(UpdatePath());

            // Set the health of the AI based on what kind of AI it is.
            if (knight)
            {
                m_Character.health = 2;
            }
            else if (juggernaut)
            {
                m_Character.health = 3;
            }
            else
            {
                m_Character.health = 1;
            }

            // Determine the AI's attack range based on kind of AI.
            // These values will probably need to change or be altered based on scale...
            if (knight || ninja || juggernaut)
            {
                attackRange = 4.0f;
            }
            else
            {
                attackRange = 5.0f;
            }
        }

        // Update is called once per frame
        void Update()
        {
            // Runs the behaviour statemachine.
            currentState.Execute();

            if (fieldOfView.visibleTargets.Count > 0)
            {
                foreach (GameObject target in fieldOfView.visibleTargets)
                {
                    // There will only ever be 1 possible target (as there is only 1 player)
                    // but in future you could put logic here to determine what to do if there is more.
                    Target = target;
                }
            }
            else
            {
                Target = null;
            }
            
            jumpTimer -= Time.deltaTime;
        }

        private void FixedUpdate()
        {
            // Determine if the AI has reached the end of the path.
            if (path != null)
            {
                if (currentWayPoint >= path.vectorPath.Count)
                {
                    if (pathIsEnded)
                        return;

                    //Debug.Log("End of Path reached.");
                    pathIsEnded = true;
                    return;
                }

                pathIsEnded = false;

                // This logic is flawed
                //if (path.vectorPath[currentWayPoint].y - transform.position.y > 0.0f)
                //{
                //    if (jumpTimer <= 0)
                //    {
                //        jumpTimer = jumpTimerReset;
                //        Debug.Log("Jumping");
                //        m_Jump = true;
                //    }
                //}
                //else
                //{
                //    m_Jump = false;
                //}

                // Determine if the AI is close enough to the node to move to the next node in the path.
                float dist = Vector3.Distance(transform.position, path.vectorPath[currentWayPoint]);
                if (dist < nextWayPointDistance)
                {
                    currentWayPoint++;
                    return;
                }
            }

            Debug.Log("Jumping: " + m_Jump);
            // Moves the AI
            if (attack)
            {
                m_Character.Attack();
                if (!(knight || ninja || juggernaut))
                {
                    GameObject projectile;
                    projectile = (Instantiate(projectilePrefab, projectilePoint.position, projectilePoint.rotation)) as GameObject;
                    // Add force to the bullet somehow
                }
                attack = false;
            }

            if (!m_Character.m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                Move(h, m_Jump);
            }
        }

        public void ChangeState(IEnemyState newState)
        {
            // Changes the behaviour state of the AI.
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
            // Updates path based on if a target is found.
            // If there is no Target, then path is null.
            if (Target == null)
            {
                yield return null;
            }

            // If a target is found, find the path between AI and the target.
            if (Target != null)
            {
                seeker.StartPath(transform.position, Target.transform.position, OnPathComplete);
                //Debug.Log("Starting Path");
            }

            yield return new WaitForSeconds(1f / updateRate);
            StartCoroutine(UpdatePath());
        }

        public void Move(float h, bool jump)
        {
            m_Character.Move(h, m_Crouch, jump);
        }

        private void Death ()
        {
            // Play Death animation

            if (knight || ninja || juggernaut)
            {
                ++scoreManager.specialEnemyKillCount;
            }
            else
            {
                ++scoreManager.basicEnemyKillCount;
            }
        }
    }
}
