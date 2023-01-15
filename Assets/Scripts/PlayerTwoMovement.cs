using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTwoMovement : MonoBehaviour
{

    private float horizontal;
    private float speed = 8f;
    private float jumpingPower = 16f;
    private bool facingRight = true;

    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 12f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;

    private bool isWallJumping;
    private bool isWallSliding;
    private float wallSlidingSpeed = 2f;

    private float wallJumpingDirection;
    private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.4f;
    private Vector2 wallJumpingPower = new Vector2(8f, 16f);


    public Rigidbody2D rb;
    public Transform groundCheck;
    public Transform wallCheck;
    public LayerMask groundLayer;
    public LayerMask wallLayer;
    public TrailRenderer tr;

// Anim for animating and others for attacks
    private Animator anim;
    public GameObject attackPoint;
    public float radius;
    public LayerMask enemyLayer;

    // knockback stuff
    public float KBForceX;
    public float KBForceY;
    public float KBCounter;
    public float KBTotalTime;
    public bool KnockFromRight;

    void Start()
    {
        anim = GetComponent<Animator>();
    }


    // Update is called once per frame
    void Update()
    {
        if (KBCounter <= 0)
        {
            horizontal = Input.GetAxisRaw("Horizontal2");

            if (Input.GetButtonDown("Jump2") && IsGrounded())
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
            }

            if (Input.GetButtonUp("Jump2") && rb.velocity.y > 0f)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y*0.5f);
            }

            if (Input.GetButtonDown("Fire22") && canDash)
            {
                StartCoroutine(Dash());
            }

            WallSlide();
            WallJump();

            if (Input.GetButtonDown("Fire11"))
            {
                anim.SetBool("isAttacking", true);
            }
        }

        if(!isWallJumping)
        {
            Flip();
        }
        // areYouWalkingTho();
        
    }

    private void FixedUpdate()
    {
        if (KBCounter <= 0)
        {
            if (isDashing)
                {
                    return;
                }
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        }
        else
        {
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


        
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    

    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.7f, wallLayer);
    }

    private void Flip()
    {
        if (facingRight && horizontal < 0f || !facingRight && horizontal > 0f)
        {
            facingRight = !facingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    public void basicAttack()
    {
        Collider2D[] enemy = Physics2D.OverlapCircleAll(attackPoint.transform.position, radius, enemyLayer);
        foreach (Collider2D enemyGameobject in enemy)
        {
            Debug.Log("Hit Player 1");

            enemyGameobject.GetComponent<PlayerOneHP>().fromRight = (attackPoint.transform.position.x >= enemyGameobject.transform.position.x);

            enemyGameobject.GetComponent<PlayerOneHP>().TakeDamage(10, 30, 30 ,(float)0.3);
        }
    }


    public void endAttack()
    {
        anim.SetBool("isAttacking", false);
    }

    // private void OnDrawGizmos()
    // {
    //     Gizmos.DrawWireSphere(attackPoint.transform.position, radius);
    // }

    // private void areYouWalkingTho()
    // {
    //     if (horizontal != 0f)
    //     {
    //         anim.SetBool("isRunning", true);
    //     }
    //     else
    //     {
    //         anim.SetBool("isRunning", false);
    //     }
    // }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float orignalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x*dashingPower, 0f);
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        rb.gravityScale = orignalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;

    }

    private void WallSlide()
    {
        if (IsWalled() && !IsGrounded() && horizontal != 0f)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
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

        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f)
        {
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;

            if (transform.localScale.x != wallJumpingDirection)
            {
                facingRight = !facingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }
}
