using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{

    public int[] selections = new int[] {0, 1};
    public List<GameObject> fighters = new List<GameObject>();
    PlayerInputManager manager;
    // Start is called before the first frame update
    void Start()
    {
        manager = GetComponent<PlayerInputManager>();
        for(int i = 0; i < selections.Length; i++){
            manager.playerPrefab = fighters[selections[i]];
            manager.JoinPlayer(selections[i]);            
        }
        // manager.JoinPlayer(0);

    }
    // Update is called once per frame
    void Update()   
    {
        Debug.Log(manager.playerCount);
    }
}
