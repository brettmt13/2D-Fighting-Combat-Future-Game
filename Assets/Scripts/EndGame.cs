using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour
{

    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
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
        
    }
}
