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
    public LayerMask wallLayer;
    public Transform groundCheck;
    public float jumpHeight;

    //climbing variables
    bool climbing = false;
    float wallCheckRadius = 0.2f;
    public float climbStrength;
    public Transform wallCheckUpper;
    public Transform wallCheckLower;
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

        // Debug.Log("ClimbStrengthAmount: " + climbStrengthAmount);
        if(grounded && climbStrengthAmount <= climbStrength)
        {
            climbStrengthAmount += 1;
        }

        float vMove = Input.GetAxis("Vertical");
        bool headTouchingWall = Physics2D.OverlapCircle(wallCheckUpper.position, wallCheckRadius, wallLayer); //TODO: change groundLayer to wallLayer when you add it
        bool bodyTouchingWall = Physics2D.OverlapCircle(wallCheckLower.position, wallCheckRadius, wallLayer);
        climbing = headTouchingWall || bodyTouchingWall; 
        animator.SetBool("isClimbing", climbing);
        if(climbing)
        {
            if (vMove > 0)
            {
                animator.SetBool("isClimbing", climbing);
                climbStrengthAmount = Mathf.Max(0, climbStrengthAmount - (Mathf.Abs(vMove) + .5f));
            }

            //give a little boost to climb onto the ground    
            if (!headTouchingWall && vMove > 0)
            {
                rigidBody.AddForce(new Vector2(0, jumpHeight));
            }
            
            if (climbStrengthAmount > 0)
                rigidBody.velocity = new Vector2(rigidBody.velocity.x, vMove*(maxSpeed/1.5f) );
            else
                animator.SetBool("isClimbing", false);
        }
    }

    bool isGrounded()
    {
        return grounded;
    }
    
    void flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
