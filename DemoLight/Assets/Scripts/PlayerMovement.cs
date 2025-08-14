using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Collider2D coll;
    private SpriteRenderer sprite;
    private Animator anim;

    public float speed, jumpForce;
    public Transform groundCheck;
    public LayerMask ground;

    public bool isGround, isJump;
    private bool isJumpPressed;
    private int  jumpCount;

    //[SerializeField] private LayerMask jumpableGround;

    private float dirX = 0f;// 水平位移
    //[SerializeField] private float moveSpeed = 7f;
    //[SerializeField] private float jumpForce = 7f;


    private enum MovementState { idle, running, jumping, falling }

    [SerializeField] private AudioSource jumpSoundEffect;


    // Start is called before the first frame update    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

    }

    // Update is called once per frame
    private void Update()
    {
        //dirX = Input.GetAxisRaw("Horizontal");
        //rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);

        if (Input.GetButtonDown("Jump") && jumpCount>0)
        {
            isJumpPressed = true;

            //jumpSoundEffect.Play();
           // rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        
    }

    private void FixedUpdate()
    {
        IsGrounded();

        GroundMovement();

        Jump();

        UpdateAnimationState();
    }

    private void GroundMovement()
    {
        dirX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(dirX * speed, rb.velocity.y);
        UpdateAnimationState();//更新左右翻转和下落动画
    }

    private void Jump()
    {
        if (isGround)
        {
            jumpCount = 2;
            isJump=false;
        }
        if (isJumpPressed)
        {
            if (isGround)
            {
                isJump = true;
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                jumpCount--;
                isJumpPressed = false;
            }
            else
            {
                isJump = true;
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                jumpCount--;
                isJumpPressed = false;
            }
        }
    }

    private void UpdateAnimationState()
    {
        MovementState state;

        if (dirX > 0f)
        {
            state = MovementState.running;
            sprite.flipX = false;
        }
        else if (dirX < 0f)
        {
            state = MovementState.running;
            sprite.flipX = true;
        }
        else
        {
            state = MovementState.idle;
        }

        if (rb.velocity.y > .1f&&isJump)
        {
            state = MovementState.jumping;
        }
        else if (rb.velocity.y < -.1f)
        {
            isJump = false;
            state = MovementState.falling;
        }

        anim.SetInteger("state", (int)state);

    }

    private void IsGrounded()
    {
        isGround= Physics2D.OverlapCircle(groundCheck.position, 0.1f, ground);
    }
}







