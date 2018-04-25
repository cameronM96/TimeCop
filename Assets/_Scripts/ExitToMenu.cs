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
                SceneManager.LoadScene("Main Menu");

            }
        }
    }
}
