using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2Manager : MonoBehaviour
{
    public GameObject[] playerSelect2;
    public int selectedCharacter2 = 0;
    public void NextCharacter2(){

        playerSelect2[selectedCharacter2].SetActive(false);
        selectedCharacter2 = (selectedCharacter2 + 1) % playerSelect2.Length;
        playerSelect2[selectedCharacter2].SetActive(true);
        PlayerPrefs.SetInt("selectedCharacter2", selectedCharacter2);

    }
    public void PreviousCharacter2(){

        playerSelect2[selectedCharacter2].SetActive(false);
        selectedCharacter2--;
        if (selectedCharacter2 < 0){
            selectedCharacter2 += playerSelect2.Length;
        }
        playerSelect2[selectedCharacter2].SetActive(true);
        PlayerPrefs.SetInt("selectedCharacter2", selectedCharacter2);
    }
}
