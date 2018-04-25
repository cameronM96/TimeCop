using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UnityStandardAssets._2D
{
    // Script made by Unity team and edited by Cameron Mullins
    public class PlatformerCharacter2D : MonoBehaviour
    {
        [SerializeField] private float m_MaxSpeed = 10f;                    // The fastest the player can travel in the x axis.
        [SerializeField] private float m_JumpForce = 400f;                  // Amount of force added when the player jumps.
        [Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;  // Amount of maxSpeed applied to crouching movement. 1 = 100%
        [SerializeField] private bool m_AirControl = false;                 // Whether or not a player can steer while jumping;
        [SerializeField] private LayerMask m_WhatIsGround;                  // A mask determining what is ground to the character

        [HideInInspector]
        public Transform m_GroundCheck;     // A position marking where to check if the player is grounded.
        const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
        private bool m_Grounded;            // Whether or not the player is grounded.
        private Transform m_CeilingCheck;   // A position marking where to check for ceilings
        const float k_CeilingRadius = .01f; // Radius of the overlap circle to determine if the player can stand up
        [HideInInspector]
        public Animator m_Anim;             // Reference to the player's animator component.
        private Rigidbody2D m_Rigidbody2D;
        public bool m_FacingRight = true;   // For determining which way the player is currently facing.
        public int health = 1;
        public float attackCD = 0f;         // This is the Cooldown timer for the basic attack
        public float ability1CD = 0f;       // This is the Cooldown timer for the Upper Cut
        public float ability2CD = 0f;       // This is the Cooldown timer for the Dash
        public float ability3CD = 0f;       // This is the Cooldown timer for the Ground Smash
        public float abilityCD = 3f;        // Cooldown length for all abilities.
        public float attackCDReset = 2f;
        private GameObject startPoint;      // Respawn point
        private float respawndelay;
        public bool ability1Learnt;         // Determine if character has learnt the Upper Cut ability
        public bool ability2Learnt;         // Determine if character has learnt the Dash ability
        public bool ability3Learnt;         // Determine if character has learnt the Ground Smash ability
        [HideInInspector]
        public bool groundSmashActive = false;
        [HideInInspector]
        public bool dashActive = false;
        public float dashPower = 50f;
        [HideInInspector]
        public bool specialAI = false;

        // Audio
        public AudioSource audioSource;
        [SerializeField] private AudioClip rangedAttack;
        [SerializeField] private AudioClip meleeAttack;
        [SerializeField] private AudioClip running;
        [SerializeField] private AudioClip powerFist;
        [SerializeField] private AudioClip dash;
        [SerializeField] private AudioClip deathSound;

        private void Awake()
        {
            // Setting up references.
            m_GroundCheck = transform.Find("GroundCheck");
            m_CeilingCheck = transform.Find("CeilingCheck");
            m_Anim = GetComponent<Animator>();
            m_Rigidbody2D = GetComponent<Rigidbody2D>();
            startPoint = GameObject.FindGameObjectWithTag("StartPoint");
            audioSource = GetComponent<AudioSource>();

            if (this.gameObject.transform.localScale.x > 0)
            {
                m_FacingRight = true;
            } else
            {
                m_FacingRight = false;
            }
        }

        private void Update()
        {
            // Check if characters health is 0 or below and kill if it is
            if (health <= 0)
            {
                Death();
            }

            // Set the cooldown timers for all abilities and attacks
            attackCD -= Time.deltaTime;
            ability1CD -= Time.deltaTime;
            ability2CD -= Time.deltaTime;
            ability3CD -= Time.deltaTime;

            // Determine if running noise should be played.
            if (!dashActive)
            {
                if (m_Rigidbody2D.velocity.x != 0 && m_Grounded)
                {
                    audioSource.clip = running;
                    audioSource.loop = true;
                    audioSource.Play();
                }
                else
                {
                    audioSource.loop = false;
                }
            }
        }

        private void FixedUpdate()
        {
            m_Grounded = false;

            // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
            // This can be done using layers instead but Sample Assets will not overwrite your project settings.
            Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != gameObject)
                    m_Grounded = true;
            }

            // Checks if "GroundSmash" is active and to change it when player hits the ground.
            if (groundSmashActive && m_Grounded)
            {
                groundSmashActive = false;
                audioSource.clip = powerFist;
                audioSource.loop = false;
                audioSource.Play();
            }

            m_Anim.SetBool("Ground", m_Grounded);

            // Set the vertical animation
            m_Anim.SetFloat("vSpeed", m_Rigidbody2D.velocity.y);
        }

        // This update makes sure that the full attack animations play before turning it off again.
        private void LateUpdate()
        {
            if (!m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                m_Anim.SetBool("Attack", false);
            }
            
            if (!m_Anim.GetCurrentAnimatorStateInfo(0).IsName("UpperCut"))
            {
                m_Anim.SetBool("UpperCut", false);
            }

            if (!m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Dash"))
            {
                m_Anim.SetBool("Dash", false);
                dashActive = false;
            }

            if (!m_Anim.GetCurrentAnimatorStateInfo(0).IsName("GroundSmash"))
            {
                m_Anim.SetBool("GroundSmash", false);
            }
        }

        public void Move(float move, bool crouch, bool jump)
        {
            if (health > 0)
            {
                // If crouching, check to see if the character can stand up
                if (!crouch && m_Anim.GetBool("Crouch"))
                {
                    // If the character has a ceiling preventing them from standing up, keep them crouching
                    if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
                    {
                        crouch = true;
                    }
                }

                // Set whether or not the character is crouching in the animator
                m_Anim.SetBool("Crouch", crouch);

                //only control the player if grounded or airControl is turned on
                if (m_Grounded || m_AirControl)
                {
                    // Reduce the speed if crouching by the crouchSpeed multiplier
                    move = (crouch ? move * m_CrouchSpeed : move);

                    // The Speed animator parameter is set to the absolute value of the horizontal input.
                    m_Anim.SetFloat("Speed", Mathf.Abs(move));

                    // Move the character
                    m_Rigidbody2D.velocity = new Vector2(move * m_MaxSpeed, m_Rigidbody2D.velocity.y);

                    // If the input is moving the player right and the player is facing left...
                    if (move > 0 && !m_FacingRight)
                    {
                        // ... flip the player.
                        Flip();
                    }
                    // Otherwise if the input is moving the player left and the player is facing right...
                    else if (move < 0 && m_FacingRight)
                    {
                        // ... flip the player.
                        Flip();
                    }
                }
                // If the player should jump...
                if (m_Grounded && jump && m_Anim.GetBool("Ground"))
                {
                    // Add a vertical force to the player.
                    m_Grounded = false;
                    m_Anim.SetBool("Ground", false);
                    m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
                }
            }
        }

        private void Flip()
        {
            // Switch the way the player is labelled as facing.
            m_FacingRight = !m_FacingRight;

            // Multiply the player's x local scale by -1.
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }

        // The methods below were created by Cameron Mullins
        public void Attack()
        {
            if (attackCD <= 0f)
            {
                attackCD = attackCDReset;
                // Play attack animation, collider is attached to the animation and is triggered there.
                m_Rigidbody2D.velocity = new Vector2(0, 0);

                m_Anim.SetBool("Attack", true);
                if (rangedAttack != null)
                {
                    audioSource.clip = rangedAttack;
                }
                else
                {
                    audioSource.clip = rangedAttack;
                }
                audioSource.loop = false;
                audioSource.Play();
            }
        }

        public void Abilities(int ability, bool dir)
        {
            switch (ability)
            {
                case 1:
                    // Do Uppdercut Ability
                    if (ability1CD <= 0f && ability1Learnt)
                    {
                        m_Anim.SetBool("UpperCut", true);
                        m_Rigidbody2D.AddForce(new Vector2(m_Rigidbody2D.velocity.x, m_JumpForce * 1.5f));
                        ability1CD = abilityCD;
                        audioSource.clip = dash;
                        audioSource.loop = false;
                        audioSource.Play();
                    }
                    break;
                case 2:
                    // Do dash Ability
                    if (ability2CD <= 0f && ability2Learnt)
                    {
                        //if ((m_FacingRight && dir == false) || (!m_FacingRight && dir ==  true))
                        //    Flip();
                        dashActive = true;
                        m_Anim.SetBool("Dash", true);
                        if (m_FacingRight)
                        {
                            m_Rigidbody2D.velocity = new Vector2(dashPower * m_MaxSpeed, m_Rigidbody2D.velocity.y);
                        }
                        else
                        {
                            m_Rigidbody2D.velocity = new Vector2((dashPower * m_MaxSpeed) * -1f, m_Rigidbody2D.velocity.y);
                        }
                        ability2CD = abilityCD;
                        audioSource.clip = dash;
                        audioSource.loop = false;
                        audioSource.Play();
                    }
                    break;
                case 3:
                    // Do GroundSmash Ability
                    if (ability3CD <= 0f && ability3Learnt)
                    {
                        m_Anim.SetBool("GroundSmash", true);
                        m_Rigidbody2D.AddForce(new Vector2(m_Rigidbody2D.velocity.x, m_JumpForce * -1f));
                        ability3CD = abilityCD;
                        groundSmashActive = true;
                        audioSource.clip = dash;
                        audioSource.loop = false;
                        audioSource.Play();
                    }
                    break;
                default:
                    break;
            }
        }

        // This is triggered when the players attack collides with a projectile...
        private void Death()
        {
            // When health hits 0 or less play the death animations and destroy object shortly after.
            m_Anim.SetBool("death", true);
            audioSource.clip = deathSound;
            audioSource.loop = false;
            audioSource.Play();
            // Instead of destroy probably just teleport to start if it's the player...
            if (this.gameObject.tag != "Player")
            {

                Destroy(this.gameObject, 3.0f);
            }
            else
            {
                // Set player back to start
                Debug.Log("Respawning");
                StartCoroutine(Respawn());
            }
        }

        // This doesn't work...
        IEnumerator Respawn()
        {
            yield return new WaitForSeconds(3);
            Debug.Log("Respawn Complete");
            health = 1;
            this.gameObject.transform.position = startPoint.transform.position;
            m_Anim.SetBool("death", false);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            // Checks if the player is hit by the AI
            if (this.gameObject.tag == "Player")
            {
                if (other.tag == "AIAttack" || other.tag == "Projectile")
                {
                    health -= 1;
                }

                // Check if player has picked up a scroll to learn a new ability
                if (other.tag == "Ability1Scroll")
                {
                    ability1Learnt = true;
                    Destroy(other.gameObject);
                }

                if (other.tag == "Ability2Scroll")
                {
                    ability2Learnt = true;
                    Destroy(other.gameObject);
                }

                if (other.tag == "Ability3Scroll")
                {
                    ability3Learnt = true;
                    Destroy(other.gameObject);
                }
            }

            // Checks if the AI is hit by the player
            if (this.gameObject.tag == "AI")
            {
                if (other.tag == "Weapon")
                {
                    health -= 1;
                    if (!(health <= 0))
                    {
                        m_Anim.SetBool("Hurt", true);
                    }
                }
            }
        }
    }   
}
