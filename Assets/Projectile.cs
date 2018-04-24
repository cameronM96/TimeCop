using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    
    public Rigidbody2D rb;
    public float speed;

	// Use this for initialization
	void Start ()
    {
        rb = GetComponent<Rigidbody2D>();

    }

    private void Update()
    {
        rb.velocity = new Vector2(speed, 0);
    }
}
