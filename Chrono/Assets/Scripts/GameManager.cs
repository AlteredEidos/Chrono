using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    int scene;
    public int activeTime;
    public bool box;
    public int keyCount;
    public int waterCan;
    public int water;
    public int sapplingNum;

    public GameObject[] timeLocations;
    public GameObject pastKey;
    public GameObject futureKey;
    public GameObject chest;
    public GameObject openChest;
    public GameObject[] sappling;
    GameObject player;
    public Animator transition;

    void Start()
    {
        Load();
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
        if (scene != 0)
        {

        }
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.transform.position = timeLocations[activeTime].transform.position;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S) || (Input.GetKeyDown(KeyCode.DownArrow)))
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
            yield return new WaitForSeconds(1);
            SceneManager.LoadScene(1);
        }
        else
        {
            transition.SetTrigger("Start");
            Save();
            yield return new WaitForSeconds(1);
            SceneManager.LoadScene(2);
        }
    }

    void Load()
    {
        activeTime = PlayerPrefs.GetInt("active", 0);
        keyCount = PlayerPrefs.GetInt("key", 0);
        sapplingNum = PlayerPrefs.GetInt("sappling", 0);
        water = PlayerPrefs.GetInt("water", 0);
        waterCan = PlayerPrefs.GetInt("waterCan", 0);
        if (PlayerPrefs.GetInt("box", 0) == 1)
        {
            box = true;
        }
    }

    void Save()
    {
        PlayerPrefs.SetInt("active", activeTime);
        PlayerPrefs.SetInt("key", keyCount);
        PlayerPrefs.SetInt("sappling", sapplingNum);
        PlayerPrefs.SetInt("water", water);
        PlayerPrefs.SetInt("waterCan", waterCan);
        if (box == true)
        {
            PlayerPrefs.SetInt("box", 1);
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
        PlayerPrefs.SetInt("active", 0);
        PlayerPrefs.SetInt("box", 0);
        PlayerPrefs.SetInt("key", 0);
        PlayerPrefs.SetInt("sappling", 0);
        PlayerPrefs.SetInt("water", 0);
        PlayerPrefs.SetInt("waterCan", 0);
        StartCoroutine("New");
    }

    IEnumerator New()
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(2);
    }
}
