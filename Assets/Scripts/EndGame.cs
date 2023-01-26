using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{

    private Animator anim;
    private Controls playerInput;

    // Start is called before the first frame update
    void Start()
    {
        playerInput = new Controls();
        playerInput.EndScreen.Enable();
        Debug.Log(PlayerPrefs.GetInt("PlayerOneWins"));
        anim = GetComponent<Animator>();
        if(PlayerPrefs.GetInt("PlayerOneWins") == 0){
            anim.SetBool("OneWins", false);
            anim.SetBool("TwoWins", true);
        }
        else{
            anim.SetBool("OneWins", true);
            anim.SetBool("TwoWins", false);
        }
        // yield return new WaitForSeconds(3f);

    }

    // Update is called once per frame
    void Update()
    {
        playerInput.EndScreen.Rematch.performed += ctx =>{
            Debug.Log("hello");
            playerInput.EndScreen.Disable();
            playerInput.SelectScreen.Enable();
            SceneManager.LoadScene("characterSelection");
        };
    }
}
