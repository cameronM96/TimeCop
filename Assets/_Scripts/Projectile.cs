using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Created By Cameron Mullins (doesn't work at the moment)
public class Projectile : MonoBehaviour {
    
    public Rigidbody2D rb;
    public float speed;
    public bool rainArrow = false;
    public AudioSource audioS;

	// Use this for initialization
	void Awake ()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(this.gameObject, 20f);
        audioS = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if(!rainArrow)
            rb.velocity = new Vector2(speed,0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        audioS.Play();
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        audioS.Play();
        Destroy(this.gameObject);
    }
}
