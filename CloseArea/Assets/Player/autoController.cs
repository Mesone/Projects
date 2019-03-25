using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class autoController : MonoBehaviour {
    public bool startFlip;
    public float moveSpeed = 4;

    private Rigidbody2D rb;
    private SpriteRenderer sr;

    private ControllerOnline.data info;
    private Player player;
    private int timeout = 0;
	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        player = GetComponent<Player>();
	}
	
	// Update is called once per frame
	void Update () {
        timeout++;
            StartCoroutine(UpdatePos());

    }
    
    IEnumerator UpdatePos()
    {
        WWWForm form = new WWWForm();
        form.AddField("pid", player.id.ToString());
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
            gameObject.transform.position = new Vector3(info.x, info.y);
            /*Debug.Log("infomove=" + info.move);
            if (info.move == "right")
            {
                rb = FindObjectOfType<Player>().GetComponent<Rigidbody2D>();
                rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
                sr.flipX = !startFlip;
            }

            if (info.move == "left")
            {
                rb = FindObjectOfType<Player>().GetComponent<Rigidbody2D>();
                rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
                sr.flipX = startFlip;
            }*/

        }
    }
}
