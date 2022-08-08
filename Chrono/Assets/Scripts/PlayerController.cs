using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Vector2 velocity;
    Vector3 lastPos;
    public bool active;
    int speed = 5;
    int jumpForce = 10;
    float groundDistance = 0.5f;
    public LayerMask jump;

    Rigidbody2D playerRB;
    GameManager gameManager;
    public Animator anim;
    public SpriteRenderer sprite;
    public GameObject groundDetect;
    public GameObject respawn;

    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        respawn = gameManager.timeLocations[gameManager.activeTime];
        if (PlayerPrefs.GetInt("flip", 0) == 1)
        {
            sprite.flipX = true;
        }
        lastPos = transform.position;
    }

    void Update()
    {
        anim.SetFloat("Speed", Mathf.Abs(playerRB.velocity.x));

        //check if moving
        if (transform.position.x != lastPos.x)
        {
            if (lastPos.x - transform.position.x < -0.05)
            {
                sprite.flipX = false;
                PlayerPrefs.SetInt("flip", 0);
            }
            else if (lastPos.x - transform.position.x > 0.05)
            {
                sprite.flipX = true;
                PlayerPrefs.SetInt("flip", 1);
            }
        }
        
        //Move Player
        velocity = playerRB.velocity;
        velocity.x = Input.GetAxisRaw("Horizontal") * speed;
        playerRB.velocity = velocity;

        //Jump only if no interaction
        if (active == false && Input.GetKeyDown(KeyCode.UpArrow) && Physics2D.Raycast(groundDetect.transform.position, Vector2.down, groundDistance, jump) || active == false && Input.GetKeyDown(KeyCode.W) && Physics2D.Raycast(groundDetect.transform.position, Vector2.down, groundDistance, jump))
        {
            anim.SetTrigger("Jump");
            playerRB.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            Debug.Log("Jump");
        }
    }

    void LateUpdate()
    {
        lastPos = transform.position;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Sappling")
        {
            active = true;
            if (gameManager.water == 1 && gameManager.sapplingNum < 3 && Input.GetKey(KeyCode.W) || gameManager.water == 1 && gameManager.sapplingNum < 3 && Input.GetKey(KeyCode.UpArrow))
            {
                gameManager.sapplingNum++;
                gameManager.water = 0;
            }
        }

        if (collision.gameObject.tag == "Key")
        {
            active = true;
            if (gameManager.keyCount == 0 && Input.GetKey(KeyCode.W) || gameManager.keyCount == 0 && Input.GetKey(KeyCode.UpArrow))
            {
                gameManager.keyCount++;
                gameManager.pastKey.SetActive(false);
            }
            else if (gameManager.keyCount == 1 && Input.GetKey(KeyCode.W) || gameManager.keyCount == 1 && Input.GetKey(KeyCode.UpArrow))
            {
                gameManager.keyCount++;
                gameManager.futureKey.SetActive(false);
            }
        }

        if (collision.gameObject.tag == "Water" && gameManager.waterCan == 1 && gameManager.water == 0)
        {
            active = true;
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            {
                gameManager.water++;
            }
        }

        if (collision.gameObject.tag == "Chest" && gameManager.keyCount == 2)
        {
            active = true;
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            {
                gameManager.waterCan = 1;
                gameManager.chest.SetActive(false);
                gameManager.openChest.SetActive(true);
            }
        }

        if (collision.gameObject.tag == "Time")
        {
            gameManager.activeTime = int.Parse(collision.gameObject.name);
            active = true;
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            {
                gameManager.StartCoroutine("TimeTravel");
            }
        }

        if (collision.gameObject.tag == "Pit")
        {
            gameObject.transform.position = respawn.transform.position;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Time")
        {
            active = false;
        }

        if (collision.gameObject.tag == "Key")
        {
            active = false;
        }

        if (collision.gameObject.tag == "Chest")
        {
            active = false;
        }

        if (collision.gameObject.tag == "Water")
        {
            active = false;
        }

        if (collision.gameObject.tag == "Sappling")
        {
            active = false;
        }

        if (collision.gameObject.tag == "Time")
        {
            active = false;
        }
    }
}
