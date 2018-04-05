using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour {

    public Animator myAnimator{get; private set;}

    [SerializeField]
    protected float moveSpeed = 5;

    protected bool facingRight;

    // Use this for initialization
    public virtual void Start ()
    {
        facingRight = true;
        myAnimator = GetComponent<Animator>();
    }

    public void ChangeDirection()
    {
        facingRight = !facingRight;
        transform.localScale = new Vector3(transform.localScale.x * -1, 1, 1);
    }
}

