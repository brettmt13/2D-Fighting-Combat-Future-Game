using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOneAttacks : MonoBehaviour
{

    private Animator anim;
    public GameObject attackPoint;
    public float radius;
    public LayerMask enemyLayer;
    public PlayerMovement playerMovement;
    Controls playerInput;
    private Vector2 attackDirection;


    private void Awake()
    {
        playerInput = new Controls();
        playerInput.Enable();
        playerInput.Player.Enable();
    }

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerMovement.KBCounter <= 0)
        {
            playerInput.Player.Attack.performed += ctx => 
            {
                if(!playerMovement.inAttackState && playerMovement.IsGrounded()){
                    playerMovement.inAttackState = true;
                    attackDirection = ctx.ReadValue<Vector2>();
                    Debug.Log(attackDirection);
                    playerMovement.moveDir[0] = 0f;
                    if (attackDirection[0] > 0f && playerMovement.facingRight) // ftilt to right
                    {
                        playerMovement.playerInput.Player.Disable();
                        anim.SetBool("isFtilt", true);
                    }
                    else if(attackDirection[0] > 0f && !playerMovement.facingRight){ // facing left, ftilt to right
                        playerMovement.Flip(true);
                        playerMovement.playerInput.Player.Disable();
                        anim.SetBool("isFtilt", true);
                    }
                    else if(attackDirection[0] < 0f && !playerMovement.facingRight){ // ftilt to left
                        playerMovement.playerInput.Player.Disable();
                        anim.SetBool("isFtilt", true);                    
                    }
                    else if(attackDirection[0] < 0f && playerMovement.facingRight){ // facing right, ftilt to left
                        playerMovement.Flip(true);
                        playerMovement.playerInput.Player.Disable();
                        anim.SetBool("isFtilt", true);                   
                    }
                    else if(attackDirection[1] > 0f){ // up tilt
                        playerMovement.playerInput.Player.Disable();
                        anim.SetBool("isUptilt", true); 
                    }
                    else{
                        // put down tilt here! for now it just makes sure they aren't attacking
                        playerMovement.inAttackState = false;
                    }
                }
                else if(!playerMovement.inAerialState && !playerMovement.IsGrounded()){
                    playerMovement.inAerialState = true;
                    attackDirection = ctx.ReadValue<Vector2>();
                    Debug.Log(attackDirection[0]);
                    // do aerials, disable necessary inputs while aerialing, and make it so you don't change directions when aerialing
                     if (attackDirection[0] > 0f && playerMovement.facingRight) // fair to right
                    {
                        playerMovement.playerInput.Player.Jump.Disable();
                        playerMovement.playerInput.Player.Dash.Disable();
                        playerMovement.playerInput.Player.WJump.Disable();
                        anim.SetBool("isFair", true);
                        anim.SetBool("isJumping", false);
                    }
                    else if(attackDirection[0] > 0f && !playerMovement.facingRight){ // facing left, fair to right
                        playerMovement.Flip(false, true);
                        playerMovement.playerInput.Player.Jump.Disable();
                        playerMovement.playerInput.Player.Dash.Disable();
                        playerMovement.playerInput.Player.WJump.Disable();
                        anim.SetBool("isFair", true);
                        anim.SetBool("isJumping", false);
                    }
                    else if(attackDirection[0] < 0f && !playerMovement.facingRight){ // fair to left
                        playerMovement.playerInput.Player.Jump.Disable();
                        playerMovement.playerInput.Player.Dash.Disable();
                        playerMovement.playerInput.Player.WJump.Disable();
                        anim.SetBool("isFair", true);                    
                    }
                    else if(attackDirection[0] < 0f && playerMovement.facingRight){ // facing right, fair to left
                        playerMovement.Flip(false, true);
                        playerMovement.playerInput.Player.Jump.Disable();
                        playerMovement.playerInput.Player.Dash.Disable();
                        playerMovement.playerInput.Player.WJump.Disable();
                        anim.SetBool("isFair", true);                  
                    }
                    else if(attackDirection[1] > 0f){ // up air
                        playerMovement.playerInput.Player.Jump.Disable();
                        playerMovement.playerInput.Player.Dash.Disable();
                        playerMovement.playerInput.Player.WJump.Disable();
                        anim.SetBool("isUpair", true); 
                    }
                    else{ // dair
                        playerMovement.playerInput.Player.Jump.Disable();
                        playerMovement.playerInput.Player.Dash.Disable();
                        playerMovement.playerInput.Player.WJump.Disable();
                        anim.SetBool("isDair", true); 
                    }                   
                }
            };

            // playerInput.Player.Jump.performed += ctx => {
            //     if(playerMovement.jumps > 0 && !playerMovement.inAttackState && playerMovement.jumps > -1){
            //         anim.SetBool("isJumping", true);
            //     }
            // };x
        };
    }

// GROUNDED ATTACKS

    public void startFTilt()
    {
        Collider2D[] enemy = Physics2D.OverlapCircleAll(attackPoint.transform.position, radius, enemyLayer);
        foreach (Collider2D enemyGameobject in enemy)
        {
            Debug.Log("Hit Player 2");
            enemyGameobject.GetComponent<PlayerTwoHP>().fromRight = (attackPoint.transform.position.x >= enemyGameobject.transform.position.x);
            enemyGameobject.GetComponent<PlayerTwoHP>().TakeDamage(10, 35, 20, (float)0.3);
        }
    }


    public IEnumerator endFTilt()
    {
        anim.SetBool("isFtilt", false);
        yield return new WaitForSeconds(0.3f);
        playerMovement.inAttackState = false;
    }


    public void startUpTilt()
    {
        // attackPoint.transform.position.y += 0.2f;
        Collider2D[] enemy = Physics2D.OverlapCircleAll(attackPoint.transform.position, radius, enemyLayer);
        foreach (Collider2D enemyGameobject in enemy)
        {
            Debug.Log("Hit Player 2");
            enemyGameobject.GetComponent<PlayerTwoHP>().fromRight = (attackPoint.transform.position.x >= enemyGameobject.transform.position.x);
            enemyGameobject.GetComponent<PlayerTwoHP>().TakeDamage(30, 10, 40, (float)0.3);
        }
    }


    public IEnumerator endUpTilt()
    {
        anim.SetBool("isUptilt", false);
        yield return new WaitForSeconds(0.3f);
        playerMovement.inAttackState = false;
        // attackPoint.transform.position.y -= 0.2f;
    }

    public void startDTilt()
    {
        Collider2D[] enemy = Physics2D.OverlapCircleAll(attackPoint.transform.position, radius, enemyLayer);
        foreach (Collider2D enemyGameobject in enemy)
        {
            Debug.Log("Hit Player 2");
            enemyGameobject.GetComponent<PlayerTwoHP>().fromRight = (attackPoint.transform.position.x >= enemyGameobject.transform.position.x);
            enemyGameobject.GetComponent<PlayerTwoHP>().TakeDamage(10, 35, 20, (float)0.3);
        }
    }


    public IEnumerator endDTilt()
    {
        anim.SetBool("isDtilt", false);
        yield return new WaitForSeconds(0.3f);
        playerMovement.inAttackState = false;
    }


// AERIAL ATTACKS

    public void startFAir()
    {
        Collider2D[] enemy = Physics2D.OverlapCircleAll(attackPoint.transform.position, radius, enemyLayer);
        foreach (Collider2D enemyGameobject in enemy)
        {
            Debug.Log("Hit Player 2");
            enemyGameobject.GetComponent<PlayerTwoHP>().fromRight = (attackPoint.transform.position.x >= enemyGameobject.transform.position.x);
            enemyGameobject.GetComponent<PlayerTwoHP>().TakeDamage(10, 40, 40, (float)0.3);
        }
    }


    public IEnumerator endFAir()
    {
        anim.SetBool("isFair", false);
        yield return new WaitForSeconds(0.3f);
        playerMovement.inAerialState = false;
        playerMovement.playerInput.Player.Jump.Enable();
        playerMovement.playerInput.Player.Dash.Enable();
        playerMovement.playerInput.Player.WJump.Enable();
    }


    public void startUpAir()
    {
        Collider2D[] enemy = Physics2D.OverlapCircleAll(attackPoint.transform.position, radius, enemyLayer);
        foreach (Collider2D enemyGameobject in enemy)
        {
            Debug.Log("Hit Player 2");
            enemyGameobject.GetComponent<PlayerTwoHP>().fromRight = (attackPoint.transform.position.x >= enemyGameobject.transform.position.x);
            enemyGameobject.GetComponent<PlayerTwoHP>().TakeDamage(10, 10, 35, (float)0.3);
        }
    }


    public IEnumerator endUpAir()
    {
        anim.SetBool("isUpair", false);
        yield return new WaitForSeconds(0.3f);
        playerMovement.inAerialState = false;
        playerMovement.playerInput.Player.Jump.Enable();
        playerMovement.playerInput.Player.Dash.Enable();
        playerMovement.playerInput.Player.WJump.Enable();
    }


    public void startDAir()
    {
        Collider2D[] enemy = Physics2D.OverlapCircleAll(attackPoint.transform.position, radius, enemyLayer);
        foreach (Collider2D enemyGameobject in enemy)
        {
            Debug.Log("Hit Player 2");
            enemyGameobject.GetComponent<PlayerTwoHP>().fromRight = (attackPoint.transform.position.x >= enemyGameobject.transform.position.x);
            enemyGameobject.GetComponent<PlayerTwoHP>().TakeDamage(10, 5, -35, (float)0.3);
        }
    }


    public IEnumerator endDAir()
    {
        anim.SetBool("isDair", false);
        yield return new WaitForSeconds(0.3f);
        playerMovement.inAerialState = false;
        playerMovement.playerInput.Player.Jump.Enable();
        playerMovement.playerInput.Player.Dash.Enable();
        playerMovement.playerInput.Player.WJump.Enable();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackPoint.transform.position, radius);
    }
}
