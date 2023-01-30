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
    public float healthAmount;
    private float totalHealth;
    private int index;
    public GameObject BlackScreen;
    private bool faded = false;
    public AudioSource source;
    public AudioClip ending;

    private PlayerInput pi;

    // Start is called before the first frame update
    void Start(){
        totalHealth = healthAmount;
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
        if(faded){
                faded = false;
                playerMovement.playerInput.EndScreen.Enable();
                SceneManager.LoadScene("gameOver");
            }
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
            healthBars[2].GetComponent<Image>().fillAmount = healthAmount / totalHealth;
        }
        else{
            healthBars[5].GetComponent<Image>().fillAmount = healthAmount / totalHealth;
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
            playerMovement.playerInput.Player.Disable(); //bad
            source.PlayOneShot(ending);
            StartCoroutine(FadeBlack());
        }

    }

    public IEnumerator FadeBlack(){
        Color objectColor = BlackScreen.GetComponent<Image>().color;
        float fadeAmount;

        while(BlackScreen.GetComponent<Image>().color.a < 1){
            fadeAmount = objectColor.a + ((float)5 * Time.deltaTime);
            objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
            BlackScreen.GetComponent<Image>().color = objectColor;
            yield return new WaitForSeconds(0.05f);
            // yield return null;
        }
        faded = true;
    }

}
