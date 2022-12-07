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
    [SerializeField] float turnSensitivity;

    Rigidbody2D playerRB;
    GameManager gameManager;
    public Animator anim;
    public SpriteRenderer sprite;
    public GameObject groundDetect;

    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        if (PlayerPrefs.GetInt("flip", 0) == 1)
        {
            sprite.flipX = true;
        }
        lastPos = transform.position;
    }

    void Update()
    {
        //animations
        anim.SetFloat("Speed", Mathf.Abs(playerRB.velocity.x));

        if (playerRB.velocity.x > turnSensitivity)
        {
            sprite.flipX = false;
            PlayerPrefs.SetInt("flip", 0);
        }
        else if (playerRB.velocity.x < -turnSensitivity)
        {
            sprite.flipX = true;
            PlayerPrefs.SetInt("flip", 1);
        }

        //Move Player
        velocity = playerRB.velocity;
        velocity.x = Input.GetAxisRaw("Horizontal") * speed;
        playerRB.velocity = velocity;

        //Jump
        if (Input.GetKeyDown(KeyCode.W) && Physics2D.Raycast(groundDetect.transform.position, Vector2.down, groundDistance, jump))
        {
            anim.SetTrigger("Jump");
            playerRB.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            Debug.Log("Jump");
        }

        //Time Travel
        if (Input.GetKeyDown(KeyCode.S))
        {
            gameManager.StartCoroutine("TimeTravel");
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //Interactions
        if (collision.gameObject.tag == "Sappling")
        {
            if (gameManager.water == 1 && gameManager.sapplingNum < 3 && Input.GetKey(KeyCode.E))
            {
                gameManager.sapplingNum++;
                gameManager.water = 0;
                gameManager.waterIcon.SetActive(false);
            }
        }

        if (collision.gameObject.tag == "Key")
        {
            if (gameManager.keyCount == 0 && Input.GetKey(KeyCode.E))
            {
                gameManager.keyCount++;
                gameManager.pastKey.SetActive(false);
            }
            else if (gameManager.keyCount == 1 && Input.GetKey(KeyCode.E))
            {
                gameManager.keyCount++;
                gameManager.futureKey.SetActive(false);
                gameManager.keyIcon.SetActive(true);
            }
        }

        if (collision.gameObject.tag == "Water" && gameManager.waterCan == 1 && gameManager.water == 0)
        {
            if (Input.GetKey(KeyCode.E))
            {
                gameManager.water++;
                gameManager.waterIcon.SetActive(true);
            }
        }

        if (collision.gameObject.tag == "Chest" && gameManager.keyCount == 2)
        {
            if (Input.GetKey(KeyCode.E))
            {
                gameManager.waterCan = 1;
                gameManager.chest.SetActive(false);
                gameManager.openChest.SetActive(true);
                gameManager.keyIcon.SetActive(false);
                gameManager.canIcon.SetActive(true);
            }
        }

        if (collision.gameObject.tag == "Big Tree")
        {
            if (Input.GetKey(KeyCode.E))
            {
                gameManager.GameOver();
            }
        }

        if (collision.gameObject.tag == "Pit")
        {
            gameObject.transform.position = new Vector2(-60, -22.6f);
        }
    }
}
