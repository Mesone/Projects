using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class skin1 : Player {

    // Use this for initialization
    /*void Start () {
        
    }*/
	
	// Update is called once per frame
    public override void activity()
    {
        curcalldown = valcalldown;
        
        for (int i = 0; i < players.Length; i++)
        {
            Debug.Log("id="+players[i].id+" name="+players[i].name);
            //Debug.Log(i);
            //Debug.Log("size"+players.Length);
            if (players[i].id != id)
            {
                Debug.Log(players[i].name);
                players[i].GetComponent<Rigidbody2D>().AddForce(new Vector2(0,1000));
            }
        }
    }
}
