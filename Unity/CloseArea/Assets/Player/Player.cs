using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public int valcalldown = 200;

    protected int curcalldown = 0;
    protected int health = 100;
    protected Player[] players;
    public int id=0;
    // Use this for initialization
    void Start () {
        
	}

    // Update is called once per frame
    void Update()
    {
        players = FindObjectsOfType<Player>();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (curcalldown == 0)
                activity();
        }
        if (curcalldown > 0)
            curcalldown--;
        else
            curcalldown = 0;
    }
    public virtual void activity()
    {
    }
}
