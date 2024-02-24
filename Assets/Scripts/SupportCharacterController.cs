using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupportCharacterController : MonoBehaviour
{
    public float moveSpeed;
    public float jumpHeight;
    public float followDistance;
    public float resetDistance;
    public float accelerationFactor; //smaller values mean slower acceleration

    Rigidbody2D rigidBody;
    Rigidbody2D targetBody;
    //object to follow
    public GameObject targetGameObject;

    float distance;

    public GameObject sprite;
    bool facingRight = true;
    bool follow = true;
    bool grounded = true;
    float groundCheckRadius = 0.2f;
    public Transform groundCheck;
    public LayerMask groundLayer;

    bool nearWall;
    public Transform wallCheck;
    public LayerMask wallLayer;


    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
        targetBody = targetGameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        shouldFollow();
        move(follow);
        jump();
        climb(follow);
    }

    void FixedUpdate()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        Debug.Log("grounded: " + grounded);
    }

    void shouldFollow()
    {
        if(Input.GetButtonUp("Follow"))
            follow = !follow;
    }
    void jump()
    {
        // TODO: new jump logic goes here

    }

    void climb(bool follow)
    {
        if(follow)
        {
            float distance_y = (targetGameObject.transform.position.y - gameObject.transform.position.y);
            //TODO: add climbing logic
            
        }
    }

    void move(bool follow)
    {
        distance = Vector2.Distance(gameObject.transform.position, targetGameObject.transform.position);
        float distance_x = Mathf.Abs(targetGameObject.transform.position.x - gameObject.transform.position.x);

        if(distance_x > followDistance && follow)
        {
            animator.SetFloat("speed", 2f);
            int direction = 1;
            if( !facingRight)
            {
                direction = -1;
            }
            rigidBody.velocity = new Vector2(direction * moveSpeed, rigidBody.velocity.y);
        } else {
            animator.SetFloat("speed", 0);
        }
        SpriteDirection();
    }

    void SpriteDirection()
    {
        Vector2 orientation = sprite.transform.localEulerAngles;
        
        if(gameObject.transform.position.x < targetGameObject.transform.position.x)
        {
            orientation.y = 0;
            facingRight = true;
        } else {
            orientation.y = 180f;
            facingRight = false;
        }

        sprite.transform.eulerAngles = orientation;
    }
}
