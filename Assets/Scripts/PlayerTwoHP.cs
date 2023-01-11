using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerTwoHP : MonoBehaviour
{

    public Image healthBar;
    public float healthAmount = 100f;

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

    public void TakeDamage(float damage)
    {
        healthAmount -= damage;
        healthBar.fillAmount = healthAmount / 100f;
    }
}
