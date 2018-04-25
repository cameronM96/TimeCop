using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// The following script was created by Cameron Mullins
namespace UnityStandardAssets._2D
{
    public class ScoreManager : MonoBehaviour
    {
        public Text scoreUIDisplay;
        public Text scoreBoard;
        public Image scoreBoardBackground;
        public float killScore = 0;
        public float basicEnemyValue = 10;
        public float specialEnemyValue = 30;

        private float basicEnemyKillCount = 0;
        private float specialEnemyKillCount = 0;
        private float totalNumberBasicEnemies;
        private float totalNumberSpecialEnemies;
        private float currentNumberBasicEnemies;
        private float currentNumberSpecialEnemies;
        private GameObject[] AICount;

        private float time = 0;
        private float timeScore;

        // Use this for initialization
        void Start()
        {
            //Find way to search for these items via their tags
            scoreUIDisplay = GetComponent<Text>();
            //scoreBoard = GetComponent<Text>();
            scoreBoard.enabled = false;
            scoreBoardBackground.enabled = false;
            killScore = 0;
            timeScore = 0;
            time = 0;

            AICount = GameObject.FindGameObjectsWithTag("AI");

            foreach (GameObject AI in AICount)
            {
                Enemy enemyScript = AI.GetComponent<Enemy>();
                if (enemyScript.knight || enemyScript.ninja || enemyScript.juggernaut)
                {
                    ++totalNumberSpecialEnemies;
                }
                else
                {
                    ++totalNumberBasicEnemies;
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            currentNumberSpecialEnemies = 0;
            currentNumberBasicEnemies = 0;
            AICount = null;
            AICount = GameObject.FindGameObjectsWithTag("AI");
            foreach (GameObject AI in AICount)
            {
                Enemy enemyScript = AI.GetComponent<Enemy>();
                if (enemyScript.knight || enemyScript.ninja || enemyScript.juggernaut)
                {
                    ++currentNumberSpecialEnemies;
                }
                else
                {
                    ++currentNumberBasicEnemies;
                }
            }

            basicEnemyKillCount = totalNumberBasicEnemies - currentNumberBasicEnemies;
            specialEnemyKillCount = totalNumberSpecialEnemies - currentNumberSpecialEnemies;

            time = Time.deltaTime;
            scoreUIDisplay.text = "Score: " + (CalcKillScore() + CalcTimeScore());
        }

        float CalcKillScore()
        {
            killScore = (basicEnemyKillCount * basicEnemyValue) + (specialEnemyKillCount * specialEnemyValue);

            return killScore;
        }

        float CalcTimeScore()
        {
            timeScore = 0f;
            // Here just incase we want it for later...

            return timeScore;
        }

        // This functions will enable the canvas that displays the breakdown for the score.
        // It will be triggered at the end of the level (simple on trigger enter event on the player)
        void DisplayScoreBoard()
        {
            // Suspend all the AI here so at the end of the level everything freezes? 
            //(maybe create another method and just call it at the same time)

            scoreBoard.text = "Enemies: \n";
            scoreBoard.text = "\t Basic Enemies Killed:\t" + basicEnemyKillCount + "\n";
            scoreBoard.text = "\t Special Enemies Killed:\t" + specialEnemyKillCount + "\n";
            scoreBoard.text = "\t Total Score: \t\t" + CalcKillScore();

            scoreBoardBackground.enabled = true;
            scoreBoard.enabled = true;
        }
    }
}

