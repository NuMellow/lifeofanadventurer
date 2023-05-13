using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{

    // Movement variables
    public float maxSpeed;
    Rigidbody2D rigidBody;
    Animator animator;
    bool facingRight;

    //jumping variables
    bool grounded = false;
    bool jumpCooledDown = true;
    float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float jumpHeight;

    //climbing variables
    bool climbing = false;
    float wallCheckRadius = 0.2f;
    public float climbStrength;
    public Transform wallCheck;
    float climbStrengthAmount;
    //TODO: should add a wall layer. For now will use ground layer

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        if(grounded && jumpCooledDown && Input.GetAxis("Jump") > 0 )
        {
            grounded = false;
            animator.SetBool("isGrounded", grounded);
            rigidBody.AddForce(new Vector2(0, jumpHeight));
        }

        if(!grounded && rigidBody.velocity.y < 0)
        {
            jumpCooledDown = false;
        }

        if(grounded && Input.GetAxis("Jump") <= 0)
        {
            jumpCooledDown = true;
        }
        // maybe TODO: Wall jump
        // else if(climbing && Input.GetButtonDown("Jump"))
        // {
        //     flip();
        //     rigidBody.AddForce(new Vector2(jumpHeight * 20, jumpHeight * 4));
        // }
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

        Debug.Log("ClimbStrengthAmount: " + climbStrengthAmount);
        if(!climbing && climbStrengthAmount <= climbStrength)
        {
            climbStrengthAmount += 1;
        }

        float vMove = Input.GetAxis("Vertical");
        climbing = Physics2D.OverlapCircle(wallCheck.position, wallCheckRadius, groundLayer); //TODO: change groundLayer to wallLayer when you add it
        animator.SetBool("isClimbing", climbing);
        if(climbing)
        {
            // float vMove = Input.GetAxis("Vertical");
            if (vMove > 0)
            {
                animator.SetBool("isClimbing", climbing);
                climbStrengthAmount = Mathf.Max(0, climbStrengthAmount - (Mathf.Abs(vMove) + .5f));
            }
                
            // else
            //     animator.SetBool("isClimbing", false);
            
            if (climbStrengthAmount > 0)
                rigidBody.velocity = new Vector2(rigidBody.velocity.x, vMove*(maxSpeed/1.5f) );
            else
                animator.SetBool("isClimbing", false);
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
