using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityStandardAssets._2D
{
    public class EnemySight : MonoBehaviour
    {

        [SerializeField]
        private Enemy enemy;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "Player")
            {
                enemy.Target = other.gameObject;
            }

        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.tag == "Player")
            {
                //enemy.Target = null;
            }
        }
    }
}
