using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Vector2 lastPos;
    public int scene;
    public bool box;
    public int keyCount;
    public int waterCan;
    public int water;
    public int sapplingNum;

    MasterManager master;
    public GameObject pastKey;
    public GameObject futureKey;
    public GameObject chest;
    public GameObject openChest;
    public GameObject[] sappling;
    public GameObject keyIcon;
    public GameObject waterIcon;
    public GameObject canIcon;
    public GameObject iconPlate;
    GameObject player;
    public Animator transition;

    void Start()
    {
        master = GameObject.FindGameObjectWithTag("Master").GetComponent<MasterManager>();
        keyCount = master.keyCount;
        sapplingNum = master.sapplingNum;
        water = master.water;
        waterCan = master.waterCan;
        box = master.box;
        lastPos = master.lastPos;
        scene = SceneManager.GetActiveScene().buildIndex;
        if (keyCount != 0 && scene == 1)
        {
            pastKey.SetActive(false);
        }
        else if (keyCount == 1 && scene == 2)
        {
            futureKey.SetActive(true);
        }
        if (waterCan == 1 && scene == 1)
        {
            chest.SetActive(false);
            openChest.SetActive(true);
        }
        if (sapplingNum != 0 && scene == 2)
        {
            sappling[sapplingNum].SetActive(true);
        }
        if (scene != 0 && scene != 3 && scene != 4)
        {
            iconPlate.SetActive(true);

            if (water == 1)
            {
                waterIcon.SetActive(true);
            }

            if (waterCan == 1)
            {
                canIcon.SetActive(true);
            }

            if (keyCount == 2 && waterCan != 1)
            {
                keyIcon.SetActive(true);
            }
        }
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.transform.position = lastPos;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            EndGame();
        }
    }

    IEnumerator TimeTravel()
    {
        if (scene == 2)
        {
            transition.SetTrigger("Start");
            Save();
            yield return new WaitForSeconds(0.5f);
            SceneManager.LoadScene(1);
        }
        else
        {
            transition.SetTrigger("Start");
            Save();
            yield return new WaitForSeconds(0.5f);
            SceneManager.LoadScene(2);
        }
    }

    public void Save()
    {
        master.keyCount = keyCount;
        master.sapplingNum = sapplingNum;
        master.water = water;
        master.waterCan = waterCan;
        master.box = box;
        if (player != null)
        {
            master.lastPos = player.transform.position;
        }
    }

    public void StartGame()
    {
        Save();
        StartCoroutine("Game");
    }

    IEnumerator Game()
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(2);
    }

    public void EndGame()
    {
        Save();
        StartCoroutine("End");
    }

    IEnumerator End()
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(0);
    }

    public void NewGame()
    {
        PlayerPrefs.SetInt("box", 0);
        PlayerPrefs.SetInt("key", 0);
        PlayerPrefs.SetInt("sappling", 0);
        PlayerPrefs.SetInt("water", 0);
        PlayerPrefs.SetInt("waterCan", 0);
        PlayerPrefs.SetFloat("lastPosX", -60);
        PlayerPrefs.SetFloat("lastPosY", -22.6f);
        StartCoroutine("New");
    }

    IEnumerator New()
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(2);
    }

    public void GameOver()
    {
        StartCoroutine("Over");
    }

    IEnumerator Over()
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(3);
    }

    IEnumerator Credits()
    {
        Debug.Log("ending");
        yield return new WaitForSeconds(7);
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(4);
    }
}
