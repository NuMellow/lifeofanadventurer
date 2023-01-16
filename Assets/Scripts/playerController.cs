using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{

    // Player Config
    public float jumpVelocity = 8f;
    public float fallMultiplier = 2.5f;

    // Movement variables
    public float maxSpeed;
    Rigidbody2D rigidBody;
    Animator animator;
    bool facingRight;

    //jumping variables
    bool grounded = false;
    float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float jumpHeight;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        if(grounded && Input.GetAxis("Jump") > 0)
        {
            grounded = false;
            animator.SetBool("isGrounded", grounded);
            rigidBody.AddForce(new Vector2(0, jumpHeight));
        }
    }

    void FixedUpdate()
    {

        //Check if we are grounded, if no then we are falling
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        animator.SetBool("isGrounded", grounded);

        animator.SetFloat("verticalSpeed", rigidBody.velocity.y);


        float move = Input.GetAxis("Horizontal");
        animator.SetFloat("speed", Mathf.Abs(move));

        rigidBody.velocity = new Vector2(move*maxSpeed, rigidBody.velocity.y);

        if(move < 0 && !facingRight)
        {
            flip();
        } else if (move > 0 && facingRight)
        {
            flip();
        }
    }

    void flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
