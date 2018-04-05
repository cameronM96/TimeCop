using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    public float jumpPower = 5;

    private Rigidbody2D rb;
    
    private bool isGrounded;
    private bool jump;

    [SerializeField]
    private Transform[] groundPoints;

    public float groundRadius;

    [SerializeField]
    private LayerMask whatIsGround;

	// Use this for initialization
	public override void Start ()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
	}

    private void Update()
    {
        HandleInput();
    }

    // Update is called once per frame
    void FixedUpdate ()
    {
        float horizontal = Input.GetAxis("Horizontal");

        isGrounded = IsGrounded();

        HandleMovement(horizontal);
        Flip(horizontal);
        ResetValues();
    }

    private void HandleInput()
    {
        if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            jump = true;
        }
    }

    private void HandleMovement(float horizontal)
    {
        if (isGrounded && jump)
        {
            rb.AddForce(new Vector2(0, jumpPower));
            isGrounded = false;
        }
        
        rb.velocity = new Vector2(horizontal * moveSpeed, rb.velocity.y); // x = -1, y = 0;
        myAnimator.SetFloat("speed", Mathf.Abs(horizontal));
    }

    private void Flip(float horizontal)
    {
        if(horizontal > 0 && !facingRight || horizontal < 0 && facingRight)
        {
            ChangeDirection();
        }
    }

    private void ResetValues ()
    {
        jump = false;
    }

    private bool IsGrounded()
    {
        if (rb.velocity.y <= 0)
        {
            foreach (Transform point in groundPoints)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, groundRadius, whatIsGround);

                for (int i =0; i < colliders.Length; i++)
                {
                    if(colliders[i].gameObject != gameObject)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
}
