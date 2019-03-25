using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour {
    public float moveSpeed = 4;
    public float jumpForce = 300;
    public bool startFlip = false;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    

    private bool isJump=false;
    private int timerJump = 0;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }
	
	// Update is called once per frame
	void Update () {
        controller();
	}

    private void controller()
    {
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
            sr.flipX = startFlip;
        }
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
            sr.flipX = !startFlip;
        }
        if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) && !isJump)
        {
            rb.AddForce(new Vector2(0, jumpForce));
            isJump = true;
        }
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {

        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        isJump = false;
    }
}

