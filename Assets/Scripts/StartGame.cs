using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{

    public Controls playerInput;
    public void Start(){
        playerInput = new Controls();
        playerInput.StartScreen.Enable();
    }
    public void Update(){
        playerInput.StartScreen.StartGame.performed += ctx => {
            // Debug.Log("he");    
            playerInput.StartScreen.Disable();
            playerInput.TutorialScreen.Enable();
            SceneManager.LoadScene("Tutorial");
            // Debug.Log('w');
        };
    }
}