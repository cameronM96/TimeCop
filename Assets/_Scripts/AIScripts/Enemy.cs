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

        public bool dropScroll;
        [SerializeField]
        private GameObject scroll;

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
                m_Character.ability1Learnt = true;
            }
            else if (juggernaut)
            {
                m_Character.health = 3;
                m_Character.ability3Learnt = true;
            }
            else if (ninja)
            {
                m_Character.health = 1;
                m_Character.ability2Learnt = true;
            }
            else
            {
                m_Character.health = 1;
            }
            //m_Character.attackCD *= 2;

            // Determine the AI's attack range based on kind of AI.
            // These values will probably need to change or be altered based on scale...
            if (ninja || juggernaut)
            {
                attackRange = 3.0f * transform.localScale.x;
                m_Character.specialAI = true;
            }
            else if (knight)
            {
                attackRange = 3.0f * (transform.localScale.x * 2);
                m_Character.specialAI = true;
            }
            else
            {
                attackRange = 10.0f * transform.localScale.x;
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
                // No target in site
                Target = null;
            }
            
            jumpTimer -= Time.deltaTime;

            // If AI dies but hasn't dropped it's scroll yet, drop it now
            if (m_Character.health <= 0)
            {
                if (dropScroll)
                {
                    dropScroll = false;
                    Instantiate(scroll, transform.position, transform.rotation);
                }
            }
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

                // Determine if the AI is close enough to the node to move to the next node in the path.
                float dist = Vector3.Distance(transform.position, path.vectorPath[currentWayPoint]);
                if (dist < nextWayPointDistance)
                {
                    currentWayPoint++;
                    return;
                }
            }

            // Don't allow AI to do actions if it is hurt or dead
            if (!m_Character.m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Hurt") || 
                !m_Character.m_Anim.GetCurrentAnimatorStateInfo(0).IsName("death"))
            {
                // Do specialAbility
                if (specialAttack)
                {
                    specialAttack = false;
                    if (knight)
                    {
                        // UpperCut
                        m_Character.Abilities(1, m_Character.m_FacingRight);
                    }
                    else if (ninja)
                    {
                        // Dash
                        Move(h, true);
                        m_Character.Abilities(2, m_Character.m_FacingRight);
                    }
                    else if (juggernaut)
                    {
                        // GroundSmash
                        m_Character.Abilities(3, m_Character.m_FacingRight);
                    }

                    // This can only ever occur once per AI, but only 1 per level should be selected.
                    if (dropScroll)
                    {
                        dropScroll = false;
                        Instantiate(scroll, transform.position, transform.rotation);
                    }
                }

                // Do basic attack
                if (attack && m_Character.attackCD <= 0)
                {
                    m_Character.Attack();
                    if (!(knight || ninja || juggernaut))
                    {
                        GameObject clone;
                        clone = (Instantiate(projectilePrefab, projectilePoint.position, projectilePoint.rotation));

                        // Set speed based on direction AI is facing
                        if (!m_Character.m_FacingRight && (clone.GetComponent<Projectile>().speed > 0))
                        {
                            // Set the projectiles speed and x scale relative to the AI
                            clone.GetComponent<Projectile>().speed *= -1;
                            Vector3 theScale = transform.localScale;
                            //theScale.x *= -1;
                            clone.transform.localScale = theScale;
                        }
                    }
                    attack = false;
                }

                // Move AI
                if (!m_Character.m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                {
                    Move(h, m_Jump);
                }
            }
        }

        private void LateUpdate()
        {
            // play the "Hurt" animation
            if (knight || juggernaut)
            {
                if (!m_Character.m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Hurt"))
                {
                    m_Character.m_Anim.SetBool("Hurt", false);
                }
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
            
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            currentState.OnTriggerEnter2D(other);
        }

        public void OnPathComplete(Path p)
        {
            // Empty path if the end of the path is complete
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

        // Move the AI
        public void Move(float h, bool jump)
        {
            m_Character.Move(h, m_Crouch, jump);
        }
    }
}
