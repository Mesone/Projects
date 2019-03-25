using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

using UnityEditor;
public class Platform : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine(Register());
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    IEnumerator Register()
    {
        WWWForm form = new WWWForm();
        form.AddField("name", "def");
        form.AddField("pid", "1");
        UnityWebRequest www = UnityWebRequest.Post("http://asrom.ru:5000/register", form);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            //Debug.Log("Form upload complete!"+ www.downloadHandler.text);
            ControllerOnline.data info = ControllerOnline.data.CreateFromJSON(www.downloadHandler.text);
            Debug.Log("pid" + info.pid);
            Debug.Log(info.name);
            Debug.Log(info.countPlayers);
            GameObject GO = new GameObject();
            GO.name = "player";
            GO.AddComponent<skin1>();
            GO.GetComponent<skin1>().id = info.pid;
            GO.AddComponent<ControllerOnline>();
            GO.GetComponent<ControllerOnline>().info=info;
            GO.AddComponent<Rigidbody2D>();
            GO.GetComponent<Rigidbody2D>().freezeRotation = true;
            GO.AddComponent<SpriteRenderer>();
            GO.GetComponent<SpriteRenderer>().sprite=Resources.Load<Sprite>("skin1");
            GO.AddComponent<PolygonCollider2D>();
            GO.transform.position = new Vector2(0, 10);
            if (info.countPlayers > 1)
            {
                for (int i = 0; i < info.countPlayers; i++)
                {
                    if (i != info.pid)
                    {
                        StartCoroutine(GetInfo(i));
                    }
                }
            }
            //newSkin.gameObject.AddComponent<ControllerOnline>();
            //Instantiate(GO, transform.position+new Vector3(0,2),transform.rotation);
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
            GO.GetComponent<Rigidbody2D>().freezeRotation=true;
            GO.AddComponent<SpriteRenderer>();
            GO.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("skin1");
            GO.AddComponent<PolygonCollider2D>();
            GO.transform.position = new Vector2(info.x, info.y);
        }
    }
}
