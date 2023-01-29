using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementGreen : MonoBehaviour
{
    public Controls playerInput;
    public Rigidbody2D rb;
    public Transform groundCheck;
    public LayerMask groundLayer;
    public Transform roofCheck;
    public LayerMask roofLayer;
    private bool isCling = true;
    public TrailRenderer tr;

    public Transform wallCheck;
    public LayerMask wallLayer;
    public Vector2 moveDir;
    public Vector2 wallDir;

    // character stats for resetting
    public const float ORIGINALGROUNDSPEED = 19.5f;
    public const float ORIGINALAIRSPEED = 11.5f;
    public const float ORIGINALJUMPSTAT = 29f;
    public const float ORIGINALFALLSPEED = -18f;
    public const float ORIGINALGRAVITYSCALE = 6f;
    public const float DECELLERATION = .7f;

    // changeable stats
    public float groundSpeed;
    public float airSpeed;
    public float jumpStat;
    public float fallSpeed;
    public int jumps = 1;
    public bool notMoving = true;

    // adding
    public bool facingRight = false;
    private bool canDash = true;
    private bool isDashing = true;
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
    public bool inAerialState;
    private Animator anim;
    public AudioSource source;
    public AudioClip moveAudio;
    public AudioClip jumpAudio;

    private void Awake(){
        anim = GetComponent<Animator>();
        playerInput = new Controls();
        playerInput.Enable();
        playerInput.Player.Enable();
    }

    private void Start(){
        groundSpeed = ORIGINALGROUNDSPEED;
        airSpeed = ORIGINALAIRSPEED;
        jumpStat = ORIGINALJUMPSTAT;
        fallSpeed = ORIGINALFALLSPEED;
        rb.gravityScale = ORIGINALGRAVITYSCALE;
        wallJumpingPower = new Vector2(groundSpeed, jumpStat);
    }

    // Update is called once per frame
    void Update()
    {
        if(IsWalled() || anim.GetBool("isKnocked")){
            jumps = 1;
            // anim.SetBool("isJumping", false);
        }
        if(IsGrounded() && !anim.GetBool("isJumping")){
            jumps = 1;
            // anim.SetBool("isJumping", false);
        }
        if(IsGrounded()){
            // anim.SetBool("isJumping", false);
            isCling = true;

            anim.SetBool("onGround", true);
            if(anim.GetBool("isFair")){
                anim.SetBool("isFair", false);
            }
            if(anim.GetBool("isUpair")){
                anim.SetBool("isUpair", false);
            }
            if(anim.GetBool("isDair")){
                anim.SetBool("isDair", false);
            }
            if(!inAttackState){
                if(!playerInput.Player.Jump.enabled){
                    playerInput.Player.Jump.Enable();
                }
                if(!playerInput.Player.Move.enabled && !inAttackState){
                    playerInput.Player.Move.Enable();
                }
                if(!playerInput.Player.Fall.enabled){
                    playerInput.Player.Fall.Enable();
                }
                if(!playerInput.Player.Dash.enabled){
                    playerInput.Player.Dash.Enable();
                }
                if(!playerInput.Player.WJump.enabled){
                    playerInput.Player.WJump.Enable();
                }
                if(!playerInput.Player.Attack.enabled){
                    playerInput.Player.Attack.Enable();
                }
            }

            // skid property
            if((notMoving) && (groundSpeed > 0)){
                groundSpeed -= DECELLERATION;
            }
            // make sure speed is correct
            if((!notMoving)){
                groundSpeed = ORIGINALGROUNDSPEED;
            }
            // if at a full stop
            if(groundSpeed <= 0f){
                moveDir[0] = 0f;
            }      
            if(inAerialState){
                inAerialState = false;
            }

            rb.velocity = new Vector2(moveDir[0] * groundSpeed, rb.velocity.y);
        }
        else if(!IsGrounded() && !IsRoofed()){
            anim.SetBool("onGround", false);
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
        RoofCling();

        if(!isWallJumping){
            Flip();
        }
    }

    void FixedUpdate()
    {
        if (KBCounter > 0)
        {
            anim.SetBool("isKnocked", true);
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
            if (KnockFromRight && !facingRight || !KnockFromRight && facingRight)
            {
                facingRight = !facingRight;
                Vector2 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }
        }
        else
        {
            anim.SetBool("isKnocked", false);
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

    private bool IsRoofed()
    {
        return Physics2D.OverlapCircle(roofCheck.position, 0.5f, roofLayer);
    }

    public bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.7f, wallLayer);
    }

    private void RoofCling()
    {
        if(IsRoofed() && !IsWalled() && isCling)
        {
            Debug.Log("Roof Cling");
            airSpeed = 0f;
            rb.gravityScale = 0f;
            isCling = false;
            rb.velocity = new Vector2(0,0);
        }
    }

    private void WallSlide()
    {
        if (IsWalled() && !IsGrounded())
        {   
            jumps = 1;
            isCling = true;
            airSpeed = 9f; // reset speed when hitting wall so there isn't an infinte multiplier
            isWallSliding = true;
            anim.SetBool("isSliding", true);
            rb.velocity = new Vector2(wallDir[0] * airSpeed * 5, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
            anim.SetBool("isSliding", false);
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
        source.PlayOneShot(moveAudio);
        rb.velocity = new Vector2(moveDir[0]*airSpeed, 0f);
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        rb.gravityScale = orignalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

    public void Flip(bool attacking = false, bool aerialAttack = false)
    {
        if(!inAerialState){ // don't flip when doing an aerial
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
        else if(aerialAttack){ // flip if reverse aerial attacking
            facingRight = !facingRight;
            Vector2 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;            
        }
    }

    public void endJump()
    {
        anim.SetBool("isJumping", false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(roofCheck.transform.position, 0.5f);
        // Gizmos.DrawWireSphere(ftiltHitbox2.transform.position, ftiltHitboxRadius);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if(playerInput.Player.enabled){
            if (context.performed){
                moveDir = context.ReadValue<Vector2>();
                wallDir = context.ReadValue<Vector2>();
                anim.SetBool("isMoving", true);
                if(IsGrounded()){
                    anim.SetBool("isRunning", true);
                    notMoving = false;
                    groundSpeed = ORIGINALGROUNDSPEED;
                    airSpeed = ORIGINALAIRSPEED;
                }
                else if(!IsGrounded()){
                    if(!isWallJumping && !IsRoofed()){
                        notMoving = false;
                        airSpeed = ORIGINALAIRSPEED;
                        groundSpeed = 0f;
                    }
                }
            }
        }
        if(context.canceled){
            notMoving = true;
            wallDir[0] = 0f;
            anim.SetBool("isRunning", false);
            anim.SetBool("isMoving", false);
        }

    }

    public void OnJump(InputAction.CallbackContext context){
        if(playerInput.Player.enabled){
            if(context.performed){
                anim.SetBool("isJumping", false);
                if(jumps > 0){
                    jumps = 0;
                    anim.SetBool("isJumping", true);
                    source.PlayOneShot(jumpAudio);
                    rb.velocity = new Vector2(moveDir[0] * groundSpeed, jumpStat);
                }

                    // if(IsGrounded()){
                    //     rb.velocity = new Vector2(moveDir[0] * groundSpeed, jumpStat);
                    //     jumps = 0;
                    //     // anim.SetBool("isJumping", false);
                    // }
                    // else if(!IsGrounded() && !IsWalled() && !IsRoofed()){
                    //     jumps = 0;
                    //     // change jump trajectory based off of air speed and joystick
                    //     if(notMoving){
                    //         airSpeed = 0f;
                    //         rb.velocity = new Vector2(airSpeed, jumpStat);
                    //     }

                    //     if(!notMoving){
                    //         if(!isWallJumping){
                    //             airSpeed = ORIGINALAIRSPEED;
                    //         }
                    //         rb.velocity = new Vector2(moveDir[0] * airSpeed, jumpStat);
                    //     } 
                    // }                  
                    
            } 
        }
    }

    public void OnFall(InputAction.CallbackContext context){
        if(context.performed){
            if(!IsGrounded() && !IsRoofed()){
                rb.velocity = new Vector2(moveDir[0] * airSpeed, fallSpeed);
            }
            else if(!IsWalled() && IsRoofed()){
                Debug.Log("Grav change");
                rb.gravityScale = ORIGINALGRAVITYSCALE;
                airSpeed = ORIGINALAIRSPEED;
                rb.velocity = new Vector2(moveDir[0] * airSpeed, fallSpeed);
                // jumps = 1;
                isCling = false;
                Debug.Log(rb.gravityScale);
                Debug.Log(airSpeed);
            }
        }
    }

    public void OnDash(InputAction.CallbackContext context){
        if(context.performed){
            if(canDash && !IsRoofed()){
                StartCoroutine(Dash());
            }
        }
    }

    public void OnWallJump(InputAction.CallbackContext context){
        if(context.performed){
            if (wallJumpingCounter > 0f)
            {
                isWallJumping = true;
                isCling = true;
                airSpeed = airSpeed * 2;
                rb.velocity = new Vector2(moveDir[0] * airSpeed * 1.13f, jumpStat);
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
}
