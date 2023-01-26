using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerHealthGreen : MonoBehaviour
{

    public float playerPercentage = 0;
    public PlayerMovementGreen playerMovement;
    public bool fromRight;
    public GameObject[] healthBars;
    public float healthAmount = 100f;
    private int index;

    private PlayerInput pi;
    // public Canvas canvas;

    // Start is called before the first frame update
    void Start(){
        // canvas.GetComponent<Canvas>();
        pi = GetComponent<PlayerInput>();
        index = pi.playerIndex;
        if(index == 0){
            // Debug.Log("ssioh");
            // healthBars[0].SetActive(true);
            // Instantiate(healthBars[0], new Vector3(0,0,0), Quaternion.identity);
            // healthBars[0].transform.parent = canvas.transform;
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
        Debug.Log(pi.playerIndex);
        playerPercentage += damage;
        // do the knockback stuff, playerMovement is already assigned to the correct script so you'll be all set to just make knockback
        // change the vals in player attacks too
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
