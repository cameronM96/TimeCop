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
        private bool basicAttack;
        private bool specialAttack;

        private int selectedWeapon = 1;

        public HalberdAttack halberdAttack;
        public KatanaAttack katanaAttack;
        public PowerfistAttack powerfistAttack;

        public GameObject halberdWeapon;
        public GameObject katanaWeapon;
        public GameObject powerfistWeapon;

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

            if (Input.GetButtonDown("Fire1"))
            {
                basicAttack = true;
            }
            else if (Input.GetButtonDown("Fire2"))
            {
                specialAttack = true;
            }
            else
            {
                basicAttack = false;
                specialAttack = false;
            }

            if (Input.GetButtonUp("Number 1"))
            {
                selectedWeapon = 1;
                halberdWeapon.SetActive(true);
                katanaWeapon.SetActive(false);
                powerfistWeapon.SetActive(false);
                Debug.Log("selectedWeapon is now 1");
            }
            else if (Input.GetButtonUp("Number 2"))
            {
                selectedWeapon = 2;
                halberdWeapon.SetActive(false);
                katanaWeapon.SetActive(true);
                powerfistWeapon.SetActive(false);
                Debug.Log("selectedWeapon is now 2");
            }
            else if (Input.GetButtonUp("Number 3"))
            {
                selectedWeapon = 3;
                halberdWeapon.SetActive(false);
                katanaWeapon.SetActive(false);
                powerfistWeapon.SetActive(true);
                Debug.Log("selectedWeapon is now 3");
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

            if (basicAttack)
            {
                if (selectedWeapon == 1)
                {
                    halberdAttack.HalberdBasic(basicAttack);
                }
                else if (selectedWeapon == 2)
                {
                    katanaAttack.KatanaBasic(basicAttack);
                }
                else if (selectedWeapon == 3)
                {
                    powerfistAttack.PowerfistBasic(basicAttack);
                }
                else
                {
                    Debug.Log("ERROR: selected weapon INT is not a valid option. selectedWeapon == " + selectedWeapon);
                }
            }
            else if (specialAttack)
            {
                if (selectedWeapon == 1)
                {
                    halberdAttack.HalberdSpecial(specialAttack);                 
                }
                else if (selectedWeapon == 2)
                {
                    katanaAttack.KatanaSpecial(specialAttack);
                }
                else if (selectedWeapon == 3)
                {
                    powerfistAttack.PowerfistSpecial(specialAttack);
                }
                else
                {
                    Debug.Log("ERROR: selected weapon INT is not a valid option. selectedWeapon == " + selectedWeapon);
                }
            }
        }
    }
}