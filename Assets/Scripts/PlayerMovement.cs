using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Controls playerInput;
    public Rigidbody2D rb;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public TrailRenderer tr;

    public Transform wallCheck;
    public LayerMask wallLayer;
    public Vector2 moveDir;
    public Vector2 wallDir;
    public float groundSpeed = 11f;
    public float airSpeed = 9f;
    public float jumpStat = 20f;
    public float fallSpeed = -14f;
    public int jumps = 2;
    public bool notMoving;

    // adding
    public bool facingRight = false;
    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 12f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;
    public bool isWallJumping;
    public bool isWallSliding;
    private float wallSlidingSpeed = 2f;
    public float wallJumpingDirection;
    private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.15f;
    private Vector2 wallJumpingPower;

    // knockback forces
    public float KBForceX;
    public float KBForceY;
    public float KBCounter;
    public float KBTotalTime;
    public bool KnockFromRight;
    public bool inAttackState;

    private Animator anim;

    private void Awake(){
        anim = GetComponent<Animator>();
        playerInput = new Controls();
        playerInput.Enable();
        playerInput.Player.Enable();
    }

    private void Start(){
        wallJumpingPower = new Vector2(groundSpeed, jumpStat);
    }

    // Update is called once per frame
    void Update()
    {
        playerInput.Player.Move.canceled += ctx => { 
            notMoving = true;
            wallDir[0] = 0f;
        };

        playerInput.Player.Move.performed += ctx => {
            moveDir = ctx.ReadValue<Vector2>();
            wallDir = ctx.ReadValue<Vector2>();

            if(IsGrounded()){
                notMoving = false;
                groundSpeed = 11f;
                airSpeed = 9f;
            }
            else if(!IsGrounded()){
                if(!isWallJumping){
                    notMoving = false;
                    airSpeed = 9f;
                    groundSpeed = 0f;
                }
            }
        };

        playerInput.Player.Jump.performed += ctx => {

            if(IsGrounded()){
                rb.velocity = new Vector2(moveDir[0] * groundSpeed, jumpStat);
                jumps = 1;
            }
            else if(!IsGrounded() && !IsWalled()){
                if(jumps > 0){
                    jumps = 0;
                    // change jump trajectory based off of air speed and joystick
                    if(notMoving){
                        airSpeed = 0f;
                        rb.velocity = new Vector2(airSpeed, jumpStat);
                    }

                    if(!notMoving){
                        if(!isWallJumping){
                            airSpeed = 9f;
                        }
                        rb.velocity = new Vector2(moveDir[0] * airSpeed, jumpStat);
                    }                   
                }
            } 
        };

        playerInput.Player.Fall.performed += ctx => {
            if(!IsGrounded()){
                rb.velocity = new Vector2(moveDir[0] * airSpeed, fallSpeed);
            }
        };

        playerInput.Player.Dash.performed += ctx => {
            if(canDash){
                StartCoroutine(Dash());
            }
        };

        if(IsGrounded()){
            jumps = 2;

            // skid property
            if((notMoving) && (groundSpeed > 0)){
                groundSpeed -= .2f;
            }
            // make sure speed is correct
            if((!notMoving)){
                groundSpeed = 11f;
            }
            // if at a full stop
            if(groundSpeed <= 0f){
                moveDir[0] = 0f;
            }      

            rb.velocity = new Vector2(moveDir[0] * groundSpeed, rb.velocity.y);
        }
        else if(!IsGrounded()){
            // if released joystick, land with no momentum
            if(notMoving){
                if(airSpeed > 0){
                    groundSpeed = 0f;
                }   
            }

            rb.velocity = new Vector2(moveDir[0] * airSpeed, rb.velocity.y);
        }

        WallSlide();
        WallJump();

        if(!isWallJumping){
            Flip();
        }
    }

    void FixedUpdate()
    {
        if (KBCounter > 0)
        {
            playerInput.Player.Disable();
            // KB*Time means it starts out high and decays quickly to 0, not linear
            if (KnockFromRight == true)
            {
                rb.velocity = new Vector2(-KBForceX*KBCounter, KBForceY*KBCounter);
            }
            else
            {
                rb.velocity = new Vector2(KBForceX*KBCounter, KBForceY*KBCounter);
            }
            KBCounter -= Time.deltaTime;
        }
        else
        {
            if (!playerInput.Player.enabled && !inAttackState)
            {
                playerInput.Player.Enable();
            }
        }
    }

    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.7f, wallLayer);
    }

    private void WallSlide()
    {
        if (IsWalled() && !IsGrounded())
        {   
            if(jumps < 2){
                jumps = 1;
            }
            airSpeed = 9f; // reset speed when hitting wall so there isn't an infinte multiplier
            isWallSliding = true;
            rb.velocity = new Vector2(wallDir[0] * airSpeed * 5, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void WallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        playerInput.Player.WJump.performed += ctx =>{
            if (wallJumpingCounter > 0f)
            {
                isWallJumping = true;
                airSpeed = airSpeed * 2;
                rb.velocity = new Vector2(moveDir[0] * airSpeed, jumpStat * 1.13f);
                wallJumpingCounter = 0f;

                if (transform.localScale.x != wallJumpingDirection)
                {
                    facingRight = !facingRight;
                    Vector2 localScale = transform.localScale;
                    localScale.x *= -1f;
                    transform.localScale = localScale;
                }

                Invoke(nameof(StopWallJumping), wallJumpingDuration);
            }     
        };
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float orignalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        airSpeed = 9f;
        airSpeed = airSpeed*1.6f;
        rb.velocity = new Vector2(moveDir[0]*airSpeed, 0f);
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        rb.gravityScale = orignalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

    public void Flip(bool attacking = false)
    {
        if(attacking){
            facingRight = !facingRight;
            Vector2 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;           
        }
        else if (facingRight && moveDir[0] < 0f || !facingRight && moveDir[0] > 0f)
        {
            facingRight = !facingRight;
            Vector2 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
}
