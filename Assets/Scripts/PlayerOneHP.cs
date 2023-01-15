using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerOneHP : MonoBehaviour
{

    public Image healthBar;
    public float healthAmount = 100f;

    public PlayerMovement playerMovement;
    public bool fromRight;

    public Image gameOver;
    public TMP_Text text;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (healthAmount <= 0)
        {
            Debug.Log("Left guy dies PLAYER ONE DIED");
            gameOver.enabled = true;
            text.enabled = true;
            // text.text = "P2 Wins!";
        }
    }

    public void TakeDamage(float damage, float kbForcex, float kbForcey, float kbTT)
    {
        playerMovement.KnockFromRight = fromRight;
        playerMovement.KBForceX = kbForcex;
        playerMovement.KBForceY = kbForcey;
        playerMovement.KBCounter = kbTT;


        healthAmount -= damage;
        healthBar.fillAmount = healthAmount / 100f;
    }
}
