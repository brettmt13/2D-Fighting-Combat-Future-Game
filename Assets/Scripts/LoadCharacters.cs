using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LoadCharacters : MonoBehaviour
{

    public GameObject[] characterPrefabs1;
    public GameObject[] characterPrefabs2;

    public GameObject player2spawnPoint;
    public GameObject player1spawnPoint;
    public PlayerInputManager manager;

    
    // public int selectedCharacter1 = PlayerPrefs.GetInt("selectedCharacter1");
    // public int selectedCharacter2 = PlayerPrefs.GetInt("selectedCharacter2");

    // Start is called before the first frame update
    void Start()
    {

        manager = GetComponent<PlayerInputManager>(); 
        int selectedCharacter1 = PlayerPrefs.GetInt("selectedCharacter1");
        int selectedCharacter2 = PlayerPrefs.GetInt("selectedCharacter2");
        Debug.Log("player1: " + selectedCharacter1);
        Debug.Log("player2: " + selectedCharacter2);
        GameObject player1 = characterPrefabs1[selectedCharacter1];
        GameObject player2 = characterPrefabs2[selectedCharacter2];
        Debug.Log(player2);
        manager.playerPrefab = player1;
        manager.JoinPlayer(PlayerPrefs.GetInt("selectedCharacter1"), -1, null, Gamepad.all[0]);
        manager.playerPrefab = player2;
        manager.JoinPlayer(PlayerPrefs.GetInt("selectedCharacter2"), -1, null, Gamepad.all[1]);
        // player1.SetActive(true);
        // player2.SetActive(true);
        // Instantiate(player1, player1spawnPoint.transform.position, Quaternion.identity);
        // Instantiate(player2, player2spawnPoint.transform.position, Quaternion.identity);
    }
}
