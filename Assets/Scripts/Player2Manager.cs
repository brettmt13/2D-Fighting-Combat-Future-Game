using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2Manager : MonoBehaviour
{
    public GameObject[] playerSelect2;
    public int selectedCharacter2 = 0;
    public GameObject position;
    private GameObject currentPlayer;

    public void Start(){
        LoadPlayer();
    }

    public void LoadPlayer(){
        playerSelect2[selectedCharacter2].SetActive(true);
        currentPlayer = Instantiate(playerSelect2[selectedCharacter2], position.transform.position, Quaternion.identity);
        PlayerPrefs.SetInt("selectedCharacter2", selectedCharacter2);
    }


    public void NextCharacter2(){

        currentPlayer.SetActive(false);
        Destroy(currentPlayer);
        selectedCharacter2 = (selectedCharacter2 + 1) % playerSelect2.Length;
        LoadPlayer();

    }
    public void PreviousCharacter2(){

        currentPlayer.SetActive(false);
        Destroy(currentPlayer);
        selectedCharacter2--;
        if (selectedCharacter2 < 0){
            selectedCharacter2 += playerSelect2.Length;
        }
       LoadPlayer();
    }
}
