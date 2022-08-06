using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Vector2 velocity;
    bool active;
    int speed = 5;

    Rigidbody2D playerRB;
    GameObject mCam;

    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        //Check For Active Key
        if (Input.GetKey(KeyCode.UpArrow))
        {
            active = true;
            playerRB.velocity = Vector2.zero;
        }
        else
        {
            active = false;
        }

        if (active == false)
        {
            //Move Player
            velocity = playerRB.velocity;
            velocity.x = Input.GetAxisRaw("Horizontal") * speed;
            playerRB.velocity = velocity;
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                Debug.Log("Past");
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                Debug.Log("Future");
            }
        }
    }
}
