using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreCollision : MonoBehaviour {

    [SerializeField]
    private GameObject[] others;

	// Use this for initialization
	private void Awake () {
        foreach (GameObject other in others) {
            Collider2D collider = other.GetComponent<Collider2D>();
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collider, true);
        }
    }
}
