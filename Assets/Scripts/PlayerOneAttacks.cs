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
            if (Input.GetButtonDown("Fire1"))
            {
                anim.SetBool("isFtilt", true);
            }
            if (Input.GetButtonDown("Fire3"))
            {
                anim.SetBool("isUptilt", true);
            }
        }

    }

    public void startFTilt()
    {
        Collider2D[] enemy = Physics2D.OverlapCircleAll(attackPoint.transform.position, radius, enemyLayer);
        foreach (Collider2D enemyGameobject in enemy)
        {
            Debug.Log("Hit Player 2");
            enemyGameobject.GetComponent<PlayerTwoHP>().fromRight = (attackPoint.transform.position.x >= enemyGameobject.transform.position.x);
            enemyGameobject.GetComponent<PlayerTwoHP>().TakeDamage(10, 30, 30, (float)0.3);
        }
    }


    public void endFTilt()
    {
        anim.SetBool("isFtilt", false);
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


    public void endUpTilt()
    {
        anim.SetBool("isUptilt", false);
        // attackPoint.transform.position.y -= 0.2f;
    }


    // private void OnDrawGizmos()
    // {
    //     Gizmos.DrawWireSphere(attackPoint.transform.position, radius);
    // }

}
