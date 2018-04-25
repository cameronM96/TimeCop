using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UnityStandardAssets._2D
{
    public class AbilityCooldown : MonoBehaviour
    {
        private PlatformerCharacter2D m_Character;
        public Image ability1;
        public Image abilityIcon1;
        public Image ability2;
        public Image abilityIcon2;
        public Image ability3;
        public Image abilityIcon3;

        // Use this for initialization
        void Start()
        {
            // Enable the frames for ability cooldowns if the player has learnt them
            m_Character = GameObject.FindGameObjectWithTag("Player").GetComponent<PlatformerCharacter2D>();
            if (m_Character.ability1Learnt)
            {
                ability1.enabled = true;
                abilityIcon1.enabled = true;
            } else
            {
                ability1.enabled = false;
                abilityIcon1.enabled = false;
            }

            if (m_Character.ability2Learnt)
            {
                ability2.enabled = true;
                abilityIcon2.enabled = true;
            } else
            {
                ability2.enabled = false;
                abilityIcon2.enabled = false;
            }

            if (m_Character.ability3Learnt)
            {
                ability3.enabled = true;
                abilityIcon3.enabled = true;
            } else
            {
                ability3.enabled = false;
                abilityIcon3.enabled = false;
            }
        }

        // Update is called once per frame
        void Update()
        {
            // Update UI
            if (m_Character.ability1Learnt)
            {
                ability1.enabled = true;
                abilityIcon1.enabled = true;
            }

            if (m_Character.ability2Learnt)
            {
                ability2.enabled = true;
                abilityIcon2.enabled = true;
            }

            if (m_Character.ability3Learnt)
            {
                ability3.enabled = true;
                abilityIcon3.enabled = true;
            }
            // Set the fill amount on the cooldown bar
            ability1.fillAmount = (m_Character.ability1CD / m_Character.abilityCD);
            ability2.fillAmount = (m_Character.ability2CD / m_Character.abilityCD);
            ability3.fillAmount = (m_Character.ability3CD / m_Character.abilityCD);
        }
    }
}
