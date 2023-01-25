using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class characterSelection : MonoBehaviour
{
    public int selectedCharacter1 = 0;
    public int selectedCharacter2 = 0;

    public Sprite[] characterSpritesPlayer1;
    public Sprite[] characterSpritesPlayer2;

    public SpriteRenderer spriteRendererPlayer1;
    public SpriteRenderer spriteRendererPlayer2;
    public Controls playerInput;

    public void Start()
    {   
        playerInput = new Controls();
        playerInput.Player.Disable();
        playerInput.SelectScreen.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        // if x hit, change sprite on player one's side
        playerInput.SelectScreen.Player1Switch.performed += ctx => {
            selectedCharacter1 = (selectedCharacter1 + 1) % characterSpritesPlayer1.Length;
            spriteRendererPlayer1.sprite = characterSpritesPlayer1[selectedCharacter1];
            PlayerPrefs.SetInt("selectedCharacter1", selectedCharacter1);
        };

        // if y hit, change sprite on player two's side
        playerInput.SelectScreen.Player2Switch.performed += ctx => {
            selectedCharacter2 = (selectedCharacter2 + 1) % characterSpritesPlayer2.Length;
            spriteRendererPlayer2.sprite = characterSpritesPlayer2[selectedCharacter2];
            PlayerPrefs.SetInt("selectedCharacter2", selectedCharacter2);
        };
    }
}
