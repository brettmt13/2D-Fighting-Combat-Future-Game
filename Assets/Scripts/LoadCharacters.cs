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
    public int character1;
    public int character2;
    // public characterSelection cs;
    // public int selectedCharacter1 = PlayerPrefs.GetInt("selectedCharacter1");
    // public int selectedCharacter2 = PlayerPrefs.GetInt("selectedCharacter2");

    // Start is called before the first frame update
    void Awake()
    {

        // Debug.Log("cs test player1: " + cs.selectedCharacter1);
        manager = GetComponent<PlayerInputManager>(); 
        character1 = PlayerPrefs.GetInt("selectedCharacter1");
        character2 = PlayerPrefs.GetInt("selectedCharacter2");
        Debug.Log("player1: " + character1);
        Debug.Log("player2: " + character2);
        GameObject player1 = characterPrefabs1[character1];
        GameObject player2 = characterPrefabs2[character2];
        // Debug.Log(player1);
        // Debug.Log(player2);
        // Debug.Log(Gamepad.all[0].ToString());

        manager.playerPrefab = player1;
        manager.JoinPlayer(-1, -1, null, Gamepad.all[0]);
        manager.playerPrefab = player2;
        manager.JoinPlayer(-1, -1, null, Gamepad.all[1]);
        // player1.SetActive(true);
        // player2.SetActive(true);
        // Instantiate(player1, player1spawnPoint.transform.position, Quaternion.identity);
        // Instantiate(player2, player2spawnPoint.transform.position, Quaternion.identity);
    }
}
