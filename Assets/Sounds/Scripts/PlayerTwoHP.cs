using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerTwoHP : MonoBehaviour
{

    public Image healthBar;
    public float healthAmount = 100f;

    public PlayerTwoMovement playerTwoMovement;
    public bool fromRight;

    public Image gameOver;
    public TMP_Text text;

    // Start is called before the first frame update
    void Start()
    {
        gameOver.enabled = false;
        text.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (healthAmount <= 0)
        {
            Debug.Log("Right guy dies PLAYER TWO DIED");
            gameOver.enabled = true;
            text.enabled = true;
        }
    }
    // basic ftilt has kbforce 30, kbtt 0.3

    public void TakeDamage(float damage, float kbForcex, float kbForcey, float kbTT)
    {
        playerTwoMovement.KnockFromRight = fromRight;
        playerTwoMovement.KBForceX = kbForcex;
        playerTwoMovement.KBForceY = kbForcey;
        playerTwoMovement.KBCounter = kbTT;

        healthAmount -= damage;
        healthBar.fillAmount = healthAmount / 100f;
    }
}
