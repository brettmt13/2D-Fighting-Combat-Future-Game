using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class characterSelection : MonoBehaviour
{
    public int selectedCharacter1 = 0;
    public int selectedCharacter2 = 0;

    public Sprite[] characterSpritesPlayer1;
    public Sprite[] characterSpritesPlayer2;
    public Sprite[] player1Title;
    public Sprite[] player2Title;

    public SpriteRenderer spriteRendererPlayer1;
    public SpriteRenderer spriteRendererPlayer2;
    public SpriteRenderer titleRendererPlayer1;
    public SpriteRenderer titleRendererPlayer2;
    public Controls playerInput;

    private PlayerInput pi;
    private int index;

    public void Start()
    {   

        pi = GetComponent<PlayerInput>();
        index = pi.playerIndex;


        PlayerPrefs.SetInt("selectedCharacter1", 0);
        PlayerPrefs.SetInt("selectedCharacter2", 0);
        playerInput = new Controls();
        playerInput.Player.Disable();
        playerInput.SelectScreen.Enable();
        spriteRendererPlayer1.sprite = characterSpritesPlayer1[0];
        titleRendererPlayer1.sprite = player1Title[0];
        spriteRendererPlayer2.sprite = characterSpritesPlayer2[0];
        titleRendererPlayer2.sprite = player2Title[0];
    }

    // Update is called once per frame
    void Update() //p1switch goes forward, p2switch goes backward
    {
        // if x (top) hit, change sprite on player one's side
        playerInput.SelectScreen.Player1Switch.performed += ctx => {
            if(index == 0){
                selectedCharacter1 = (selectedCharacter1 + 1) % characterSpritesPlayer1.Length;
                spriteRendererPlayer1.sprite = characterSpritesPlayer1[selectedCharacter1];
                titleRendererPlayer1.sprite = player1Title[selectedCharacter1];
                PlayerPrefs.SetInt("selectedCharacter1", selectedCharacter1);
            }
            else if(index == 0){
                selectedCharacter2 = (selectedCharacter2 + 1) % characterSpritesPlayer2.Length;
                spriteRendererPlayer2.sprite = characterSpritesPlayer2[selectedCharacter2];
                titleRendererPlayer2.sprite = player2Title[selectedCharacter2];
                PlayerPrefs.SetInt("selectedCharacter2", selectedCharacter2);
            }

            // selectedCharacter1 = (selectedCharacter1 + 1) % characterSpritesPlayer1.Length;
            // spriteRendererPlayer1.sprite = characterSpritesPlayer1[selectedCharacter1];
            // titleRendererPlayer1.sprite = player1Title[selectedCharacter1];
            // PlayerPrefs.SetInt("selectedCharacter1", selectedCharacter1);
        };

        // if y (left) hit, change sprite on player two's side
        playerInput.SelectScreen.Player2Switch.performed += ctx => {
            if(index == 0){
                selectedCharacter1 = (selectedCharacter1 - 1) % characterSpritesPlayer1.Length;
                if(selectedCharacter1 < 0){
                    selectedCharacter1 = 2;
                }
                spriteRendererPlayer1.sprite = characterSpritesPlayer1[selectedCharacter1];
                titleRendererPlayer1.sprite = player1Title[selectedCharacter1];
                PlayerPrefs.SetInt("selectedCharacter1", selectedCharacter1);
            }
            else if(index == 1){
                selectedCharacter2 = (selectedCharacter2 - 1) % characterSpritesPlayer2.Length;
                if(selectedCharacter2 < 0){
                    selectedCharacter2 = 2;
                }
                spriteRendererPlayer2.sprite = characterSpritesPlayer2[selectedCharacter2];
                titleRendererPlayer2.sprite = player2Title[selectedCharacter2];
                PlayerPrefs.SetInt("selectedCharacter2", selectedCharacter2);
            }
            
            // selectedCharacter2 = (selectedCharacter2 + 1) % characterSpritesPlayer2.Length;
            // spriteRendererPlayer2.sprite = characterSpritesPlayer2[selectedCharacter2];
            // titleRendererPlayer2.sprite = player2Title[selectedCharacter2];
            // PlayerPrefs.SetInt("selectedCharacter2", selectedCharacter2);
        };
        
        playerInput.SelectScreen.StartGame.performed += ctx => {
            SceneManager.LoadScene("SampleScene");
            playerInput.SelectScreen.Disable();
            playerInput.Player.Enable();
        };
    }
}
