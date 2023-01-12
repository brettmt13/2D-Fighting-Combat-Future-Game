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

    void Start()
    {
        anim = GetComponent<Animator>();
    }


    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal2");

        if (Input.GetButtonDown("Jump2") && (IsGrounded() || IsWalled()))
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

        if (Input.GetButtonDown("Fire11"))
        {
            anim.SetBool("isAttacking", true);
        }
        Flip();
        // areYouWalkingTho();
        
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
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
            // enemyGameobject.GetComponent<EnemyHealth>().health -= 10;
            enemyGameobject.GetComponent<PlayerOneHP>().TakeDamage(10);
        }
    }


    public void endAttack()
    {
        anim.SetBool("isAttacking", false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackPoint.transform.position, radius);
    }

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
}
