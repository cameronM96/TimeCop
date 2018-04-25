using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    
    public Rigidbody2D rb;
    public float speed;
    public bool rainArrow = false;

	// Use this for initialization
	void Start ()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(this.gameObject, 20f);
    }

    private void Update()
    {
        if(!rainArrow)
            rb.velocity = new Vector2(speed,0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(this.gameObject);
    }
}
