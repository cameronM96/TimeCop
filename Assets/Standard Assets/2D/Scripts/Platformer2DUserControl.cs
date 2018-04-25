using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

// Script made by Unity team and edited by Cameron Mullins
namespace UnityStandardAssets._2D
{
    [RequireComponent(typeof (PlatformerCharacter2D))]
    public class Platformer2DUserControl : MonoBehaviour
    {
        private PlatformerCharacter2D m_Character;
        private bool m_Jump;
        private bool attack = false;
        private int abilityNumber = 0;
        private bool right = true;

        private void Awake()
        {
            m_Character = GetComponent<PlatformerCharacter2D>();
        }
        
        private void Update()
        {
            if (!m_Jump)
            {
                // Read the jump input in Update so button presses aren't missed.
                m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
            }
        }
        
        private void FixedUpdate()
        {
            // Reset 
            attack = false;
            abilityNumber = 0;

            // Read the inputs.

            // The line below is commented out because this game does not use a crouch (created by Unity team).
            //bool crouch = Input.GetKey(KeyCode.LeftControl);
            bool crouch = false;

            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            
            // Don't do ability if one is currently in progress
            if (!m_Character.m_Anim.GetCurrentAnimatorStateInfo(0).IsName("UpperCut") ||
                !m_Character.m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Dash") ||
                !m_Character.m_Anim.GetCurrentAnimatorStateInfo(0).IsName("GroundSmash") ||
                !m_Character.m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Attack")) 
                {
                // This is the UpperCut ability
                if (Input.GetButton("Fire2") && Input.GetKey(KeyCode.W) && m_Character.ability1CD <= 0)
                    abilityNumber = 1;
                // This is the Dash Ability (right)
                if (Input.GetButton("Fire2") && Input.GetKey(KeyCode.A) && m_Character.ability2CD <= 0)
                {
                    abilityNumber = 2;
                    right = true;
                }
                // This is the Dash Ability (left)
                if (Input.GetButton("Fire2") && Input.GetKey(KeyCode.D) && m_Character.ability2CD <= 0)
                {
                    abilityNumber = 2;
                    right = false;
                }

                // This is the Ground Smash Ability
                if (Input.GetButton("Fire2") && Input.GetKey(KeyCode.S) && m_Character.ability3CD <= 0)
                    abilityNumber = 3;
                if (Input.GetButton("Fire1") && m_Character.attackCD <= 0)
                    attack = true;
            }

            // Pass all parameters to the character control script.
            if (abilityNumber != 0)
            {
                // Do abilities
                m_Character.Abilities(abilityNumber,right);
            }
            else if (attack)
            {
                // Do attack
                m_Character.Attack();
            }
            else
            {
                // Move character (if groundsmash isn't active)
                if (!m_Character.groundSmashActive || !m_Character.m_Anim.GetCurrentAnimatorStateInfo(0).IsName("Dash"))
                    m_Character.Move(h, crouch, m_Jump);
            }
            
            m_Jump = false;
        }
    }
}
