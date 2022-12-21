using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterManager : MonoBehaviour
{
    public Vector2 lastPos;
    public int scene;
    public bool box;
    public int keyCount;
    public int waterCan;
    public int water;
    public int sapplingNum;

    GameManager gm;
    AudioSource source;
    GameObject[] other;
    void Awake()
    {
        other = GameObject.FindGameObjectsWithTag("Master");

        foreach (GameObject oneOther in other)
        {
            if (oneOther.scene.buildIndex == -1)
            {
                Destroy(gameObject);
            }
        }
        DontDestroyOnLoad(transform.gameObject);
        source = GetComponent<AudioSource>();

        Load();
    }

    public void PlayMusic()
    {
        if (GetComponent<AudioSource>().isPlaying) return;
        source.Play();
    }

    public void StopMusic()
    {
        source.Stop();
    }

    void Load()
    {
        keyCount = PlayerPrefs.GetInt("key", 0);
        sapplingNum = PlayerPrefs.GetInt("sappling", 0);
        water = PlayerPrefs.GetInt("water", 0);
        waterCan = PlayerPrefs.GetInt("waterCan", 0);
        if (PlayerPrefs.GetInt("box", 0) == 1)
        {
            box = true;
        }
        lastPos = new Vector2(PlayerPrefs.GetFloat("lastPosX", -60), PlayerPrefs.GetFloat("lastPosY", -22.6f));
    }

    void Save()
    {
        gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        gm.Save();
        PlayerPrefs.SetInt("key", keyCount);
        PlayerPrefs.SetInt("sappling", sapplingNum);
        PlayerPrefs.SetInt("water", water);
        PlayerPrefs.SetInt("waterCan", waterCan);
        if (box)
        {
            PlayerPrefs.SetInt("box", 1);
        }
        PlayerPrefs.SetFloat("lastPosX", lastPos.x);
        PlayerPrefs.SetFloat("lastPosY", lastPos.y);
    }

    private void OnApplicationQuit()
    {
        Save();
    }
}
