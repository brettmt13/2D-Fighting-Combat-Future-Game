using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class Player1Manager : MonoBehaviour

{
    public GameObject[] playerSelect1;
    public int selectedCharacter1 = 0;
    public GameObject position;
    private GameObject currentPlayer;
    
    public void Start(){
        LoadPlayer();
    }

    public void LoadPlayer(){
        playerSelect1[selectedCharacter1].SetActive(false);
        currentPlayer = Instantiate(playerSelect1[selectedCharacter1], position.transform.position, Quaternion.identity);
        PlayerPrefs.SetInt("selectedCharacter1", selectedCharacter1);
    }

    public void NextCharacter1(){

        currentPlayer.SetActive(false);
        Destroy(currentPlayer);
        selectedCharacter1 = (selectedCharacter1 + 1) % playerSelect1.Length;
        LoadPlayer();
    }

    

    public void PreviousCharacter1(){

        currentPlayer.SetActive(false);
        Destroy(currentPlayer);
        selectedCharacter1--;
        if (selectedCharacter1 < 0){
            selectedCharacter1 += playerSelect1.Length;
        }
        LoadPlayer();
    }

}
