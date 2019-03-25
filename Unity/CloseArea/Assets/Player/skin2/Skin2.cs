using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skin2 : Player {

    // Use this for initialization
    public override void activity()
    {
        curcalldown = valcalldown;
        for (int i = 0; i < players.Length; i++)
        {
            players[i].GetComponent<Rigidbody2D>().AddForce(100*(players[i].transform.position - transform.position));
        }
        
    }
}
