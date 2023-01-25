using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class Player1Manager : MonoBehaviour

{
    public GameObject[] playerSelect1;
    public int selectedCharacter1 = 0;

    public void LoadPlayer(){
        // playerSelect1[selectedCharacter1]
        // currentPlayer = Instantiate(playerSelect1[selectedCharacter1], position.transform.position, Quaternion.identity);
        playerSelect1[selectedCharacter1].SetActive(true);
        PlayerPrefs.SetInt("selectedCharacter1", selectedCharacter1);
        Debug.Log("Player1-pre"+PlayerPrefs.GetInt("selectedCharacter1"));
    }

    public void NextCharacter1(){

        playerSelect1[selectedCharacter1].SetActive(false);
        selectedCharacter1 = (selectedCharacter1 + 1) % playerSelect1.Length;
        LoadPlayer();
    }

    

    public void PreviousCharacter1(){

        playerSelect1[selectedCharacter1].SetActive(false);
        selectedCharacter1--;
        if (selectedCharacter1 < 0){
            selectedCharacter1 += playerSelect1.Length;
        }
        LoadPlayer();
    }

}
