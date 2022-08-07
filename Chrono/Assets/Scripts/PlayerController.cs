using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Vector2 velocity;
    Vector3 lastPos;
    public bool active;
    int speed = 5;
    int jumpForce = 8;
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

        if (active == true && Input.GetKeyDown(KeyCode.UpArrow) || active == true && Input.GetKeyDown(KeyCode.W))
        {
            gameManager.StartCoroutine("TimeTravel");
        }
    }

    void LateUpdate()
    {
        lastPos = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Time")
        {
            gameManager.activeTime = int.Parse(collision.gameObject.name);
            active = true;
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
    }
}
