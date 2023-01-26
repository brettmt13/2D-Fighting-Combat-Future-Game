using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerHealthRed : MonoBehaviour
{
 public float playerPercentage = 0;
    public PlayerMovementRed playerMovement;
    public bool fromRight;
    public GameObject[] healthBars;
    public float healthAmount = 100f;
    private int index;

    private PlayerInput pi;

    // Start is called before the first frame update
    void Start(){
        pi = GetComponent<PlayerInput>();
        index = pi.playerIndex;
        if(index == 0){

            healthBars[3].SetActive(false);
            healthBars[4].SetActive(false);
            healthBars[5].SetActive(false);
        }
        else{
            healthBars[0].SetActive(false);
            healthBars[1].SetActive(false);
            healthBars[2].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(float damage, float kbForcex, float kbForcey, float kbTT)
    {   
        playerPercentage += damage;
        playerMovement.KnockFromRight = fromRight;
        playerMovement.KBForceX = kbForcex;
        playerMovement.KBForceY = kbForcey;
        playerMovement.KBCounter = kbTT;

        healthAmount -= damage;
        if(index == 0){
            healthBars[2].GetComponent<Image>().fillAmount = healthAmount / 100f;
        }
        else{
            healthBars[5].GetComponent<Image>().fillAmount = healthAmount / 100f;
        }

    }
}
