using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    int scene;
    public int activeTime;
    public bool box;

    public GameObject[] timeLocations;
    GameObject player;
    public Animator transition;

    void Start()
    {
        Load();
        scene = SceneManager.GetActiveScene().buildIndex;
        player = GameObject.FindGameObjectWithTag("Player");
        if(player != null)
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
        if (PlayerPrefs.GetInt("box", 0) == 1)
        {
            box = true;
        }
    }

    void Save()
    {
        PlayerPrefs.SetInt("active", activeTime);
        if (box == true)
        {
            PlayerPrefs.SetInt("box", 1);
        }
    }

    void StartGame()
    {
        transition.SetTrigger("Start");
        Save();
        SceneManager.LoadScene(1);
    }

    void EndGame()
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

    void NewGame()
    {
        transition.SetTrigger("Start");
        PlayerPrefs.SetInt("active", 0);
        PlayerPrefs.SetInt("box", 0);
        SceneManager.LoadScene(1);
    }
}
