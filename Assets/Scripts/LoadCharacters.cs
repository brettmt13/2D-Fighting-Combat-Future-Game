using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadCharacters : MonoBehaviour
{

    public GameObject[] characterPrefabs1;
    public GameObject[] characterPrefabs2;

    // Start is called before the first frame update
    void Start()
    {
        int selectedCharacter1 = PlayerPrefs.GetInt("selectedCharacter1");
        int selectedCharacter2 = PlayerPrefs.GetInt("selectedCharacter2");
        GameObject player1 = characterPrefabs1[selectedCharacter1];
        GameObject player2 = characterPrefabs2[selectedCharacter2];
        player1.SetActive(true);
        player2.SetActive(true);
        Instantiate(player1);
        Instantiate(player2);

    }

}
