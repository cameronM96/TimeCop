using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    
    public Rigidbody2D rb;
    public float speed;

	// Use this for initialization
	void Start ()
    {
        //rb = GetComponent<Rigidbody2D>();
        Destroy(this.gameObject, 20f);
    }

    private void Update()
    {
        //rb.velocity = new Vector2(speed, 0);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(this.gameObject);
    }
}
