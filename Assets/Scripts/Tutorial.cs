using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour
{

    public Controls playerInput;
    public void Start(){
        playerInput = new Controls();
        playerInput.TutorialScreen.Enable();
    }
    public void Update(){
        playerInput.TutorialScreen.Continue.performed += ctx => {
            Debug.Log("he");    
            playerInput.TutorialScreen.Disable();
            playerInput.SelectScreen.Enable();
            SceneManager.LoadScene("characterSelection");
            // Debug.Log('w');
        };
    }
}