using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthGreen : MonoBehaviour
{

    public float playerPercentage = 0;
    public PlayerMovementGreen playerMovement;
    public bool fromRight;

    // Start is called before the first frame update
    void Start(){
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damage, float kbForcex, float kbForcey, float kbTT)
    {
        playerPercentage += damage;
        // do the knockback stuff, playerMovement is already assigned to the correct script so you'll be all set to just make knockback
        // change the vals in player attacks too
        playerMovement.KnockFromRight = fromRight;
        playerMovement.KBForceX = kbForcex;
        playerMovement.KBForceY = kbForcey;
        playerMovement.KBCounter = kbTT;
    }
}
