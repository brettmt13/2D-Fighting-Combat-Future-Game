using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealthRed : MonoBehaviour
{
 public float playerPercentage = 0;
    public PlayerMovementRed playerMovement;
    public bool fromRight;
    public GameObject[] healthBars;
    public float healthAmount = 1000f;
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
            transform.position = new Vector2(-13.27f,-6.53f);
        }
        else{
            healthBars[0].SetActive(false);
            healthBars[1].SetActive(false);
            healthBars[2].SetActive(false);
            transform.position = new Vector2(12.17f,-6.53f);
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
            healthBars[2].GetComponent<Image>().fillAmount = healthAmount / 1000f;
        }
        else{
            healthBars[5].GetComponent<Image>().fillAmount = healthAmount / 1000f;
        }

        if(healthAmount <= 0){
            if(index == 0){
                PlayerPrefs.SetInt("PlayerOneWins", 0);
                PlayerPrefs.SetInt("PlayerTwoWins", 1);
            }
            else{
                PlayerPrefs.SetInt("PlayerOneWins", 1);
                PlayerPrefs.SetInt("PlayerTwoWins", 0);
            }
            playerMovement.playerInput.Player.Disable();
            playerMovement.playerInput.EndScreen.Enable();
            SceneManager.LoadScene("gameOver");
        }

    }
}
