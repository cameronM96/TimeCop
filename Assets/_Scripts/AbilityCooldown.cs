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
        public Image ability2;
        public Image ability3;

        // Use this for initialization
        void Start()
        {
            m_Character = GameObject.FindGameObjectWithTag("Player").GetComponent<PlatformerCharacter2D>();
        }

        // Update is called once per frame
        void Update()
        {
            ability1.fillAmount = (m_Character.ability1CD / m_Character.abilityCD);
            ability2.fillAmount = (m_Character.ability2CD / m_Character.abilityCD);
            ability3.fillAmount = (m_Character.ability3CD / m_Character.abilityCD);
        }
    }
}
