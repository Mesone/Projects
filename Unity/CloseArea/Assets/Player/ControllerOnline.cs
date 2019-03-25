using System.Collections;
using System.Collections.Generic;

using UnityEngine.Networking;
using UnityEngine;

public class ControllerOnline : MonoBehaviour {
    NetworkConnection net;
    public float moveSpeed = 4;
    public float jumpForce = 300;
    public bool startFlip = false;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    public data info;
    private Player player;

    public string myname="def";
    private int id = 0;

    private int mytimer=0;

    private bool isJump = false;
    private int timerJump = 0;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        player = GetComponent < Player >();
        //StartCoroutine(Register());
        //Debug.Log(Time.time);
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(SendMove(""));
        controller();
        mytimer++;
        if (mytimer == 30)
        {
            mytimer = 0;
            StartCoroutine(WhatNew());
        }
    }

    private void controller()
    {
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
            sr.flipX = startFlip;
            //StartCoroutine(SendMove("left"));
        }
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
            sr.flipX = !startFlip;
            //StartCoroutine(SendMove("right"));
        }
        if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) && !isJump)
        {
            rb.AddForce(new Vector2(0, jumpForce));
            isJump = true;
            //StartCoroutine(Move("left"));
        }
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
           // StartCoroutine(UpdateD());
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        isJump = false;
    }
    public class data
    {
        public int pid;
        public string name;
        public string move;
        public int countPlayers;
        public float x;
        public float y;

        public static data CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<data>(jsonString);
        }
    }
    IEnumerator Register()
    {
        WWWForm form = new WWWForm();
        form.AddField("name", myname);
        form.AddField("pid", player.id.ToString());
        UnityWebRequest www = UnityWebRequest.Post("http://asrom.ru:5000/register", form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            //Debug.Log("Form upload complete!"+ www.downloadHandler.text);
            info=data.CreateFromJSON(www.downloadHandler.text);
            Debug.Log("id"+info.pid);
            //Debug.Log(Time.time);
        }
    }

    IEnumerator SendMove(string str)
    {
        WWWForm form = new WWWForm();
        form.AddField("pid", info.pid.ToString());
        form.AddField("move", str);
        form.AddField("x", transform.position.x.ToString());
        form.AddField("y", transform.position.y.ToString());
        UnityWebRequest www = UnityWebRequest.Post("http://asrom.ru:5000/move", form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            //Debug.Log(www.error);
        }
        else
        {
            //Debug.Log("Form upload complete!" + www.downloadHandler.text);
        }
    }
    IEnumerator WhatNew()
    {
        WWWForm form = new WWWForm();
        form.AddField("pid", player.id.ToString());
        UnityWebRequest www = UnityWebRequest.Post("http://asrom.ru:5000/get", form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            //Debug.Log(www.error);
        }
        else
        {
            //Debug.Log("Form upload complete!" + www.downloadHandler.text);
            ControllerOnline.data info = ControllerOnline.data.CreateFromJSON(www.downloadHandler.text);
            Player[] parray = FindObjectsOfType<Player>();
            
            if (info.countPlayers != parray.Length)
            {
                for (int i = 0; i < info.countPlayers; i++)
                {
                    if (i != info.pid)
                    {
                        StartCoroutine(GetInfo(i));
                    }
                }
            }
        }
    }
    IEnumerator GetInfo(int id)
    {
        WWWForm form = new WWWForm();
        form.AddField("pid", id.ToString());
        UnityWebRequest www = UnityWebRequest.Post("http://asrom.ru:5000/get", form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            //Debug.Log("Form upload complete!" + www.downloadHandler.text);
            ControllerOnline.data info = ControllerOnline.data.CreateFromJSON(www.downloadHandler.text);
            GameObject GO = new GameObject();
            GO.name = "enemy";
            GO.AddComponent<Skin2>();
            GO.GetComponent<Skin2>().id = info.pid;
            GO.AddComponent<autoController>();
            GO.AddComponent<Rigidbody2D>();
            GO.GetComponent<Rigidbody2D>().freezeRotation = true;
            GO.AddComponent<SpriteRenderer>();
            GO.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("skin1");
            GO.AddComponent<PolygonCollider2D>();
            GO.transform.position = new Vector2(info.x, info.y);
        }
    }
}
