using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;


namespace UnityStandardAssets._2D
{
    public class ExitTo2 : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.tag == "Player")
            {
                SceneManager.LoadScene("SC_Feudal Tokyo");

            }
        }
    }
}
