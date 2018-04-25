using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;


namespace UnityStandardAssets._2D
{
    public class ExitToMenu : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.tag == "Player")
            {
                ScoreManager scoreBoard = GameObject.FindGameObjectWithTag("Score").GetComponent<ScoreManager>();
                scoreBoard.DisplayScoreBoard();
                StartCoroutine(NextLevel());
            }
        }

        IEnumerator NextLevel()
        {
            yield return new WaitForSeconds(10);
            SceneManager.LoadScene("Main Menu");
        }
    }
}

