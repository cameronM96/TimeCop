using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets._2D
{
    [RequireComponent(typeof(PlatformerCharacter2D))]
    public class CharacterUserControls : MonoBehaviour
    {
        private PlatformerCharacter2D m_Character;
        private bool m_Jump;

        private bool keyRMB;
        private bool keyW;
        private bool keyS;
        private bool keyA;
        private bool keyD;
        private bool keyUp;
        private bool keyDown;
        private bool keyLeft;
        private bool keyRight;

        public GameObject playerCharacter;

        public float sidewaysDragonSpeed = 10f;
        public float risingDragonSpeed = 10f;
        public float fallingDragonSpeed = 10f;


        private void Awake()
        {
            m_Character = GetComponent<PlatformerCharacter2D>();          
        }


        private void Update()
        {
            //toggle RMB bool
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                keyRMB = true;
            }
            if (Input.GetKeyUp(KeyCode.Mouse1))
            {
                keyRMB = false;
            }
            // toggle W bool
            if (Input.GetKeyDown(KeyCode.W))
            {
                keyW = true;
            }
            if (Input.GetKeyUp(KeyCode.W))
            {
                keyW = true;
            }
            //toggle S bool
            if (Input.GetKeyDown(KeyCode.S))
            {
                keyS = true;
            }
            if (Input.GetKeyUp(KeyCode.S))
            {
                keyS = false;
            }
            //toggle A bool
            if (Input.GetKeyDown(KeyCode.A))
            {
                keyA = true;
            }
            if (Input.GetKeyUp(KeyCode.A))
            {
                keyA = false;
            }
            //toggle D bool
            if (Input.GetKeyDown(KeyCode.D))
            {
                keyD = true;
            }
            if (Input.GetKeyUp(KeyCode.D))
            {
                keyD = false;
            }
            //toggle up arrow bool
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                keyUp = true;
            }
            if (Input.GetKeyUp(KeyCode.UpArrow))
            {
                keyUp = false;
            }
            //toggle down arrow bool
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                keyDown = true;
            }
            if (Input.GetKeyUp(KeyCode.DownArrow))
            {
                keyDown = false;
            }
            //toggle left bool
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                keyLeft = true;
            }
            if (Input.GetKeyUp(KeyCode.LeftArrow))
            {
                keyLeft = false;
            }
            //toggle right bool
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                keyRight = true;
            }
            if (Input.GetKeyUp(KeyCode.RightArrow))
            {
                keyRight = false;
            }

            if (!m_Jump)
            {
                // Read the jump input in Update so button presses aren't missed.
                m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
            }

            if (keyRMB)
            {
                Debug.Log("Right Click");

                if (keyUp || keyW)
                {
                    Debug.Log("Rising Dragon ability used");
                    //do rising dragon ability
                    //play animation
                }
                else if (keyDown || keyS)
                {
                    Debug.Log("Falling Dragon ability used");
                    //do falling dragon ability 
                    //play animation
                }
                else if (keyRight || keyD)
                {
                    Debug.Log("Sideways Dragon ability used RIGHT");
                    //do sideways dragon ability to the right
                    //play animation
                }
                else if (keyLeft || keyA)
                {
                    Debug.Log("Sideways Dragon abiltiy used LEFT");
                    //do sideways dragon ability to the left
                    //play animation
                }
                else
                {
                    Debug.Log("No special ability used");
                }
            }

            if (Input.GetButtonDown("Fire1"))
            {
                //do basic attack

                //play baic attack animation
            }
            
        }


        private void FixedUpdate()
        {
            // Read the inputs.
            bool crouch = false;
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            // Pass all parameters to the character control script.
            m_Character.Move(h, crouch, m_Jump);
            m_Jump = false;
        }
    }
}