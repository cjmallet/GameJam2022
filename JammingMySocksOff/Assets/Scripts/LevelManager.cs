using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class LevelManager : MonoBehaviour
{
    private GameObject normalGround;
    private GameObject inverseGround;
    private GameObject ground;
    private static string levelPrefix="Level_";

    // Start is called before the first frame update
    void Start()
    {
        ground=GameObject.Find("Ground");
        inverseGround = ground.transform.GetChild(1).gameObject;
        normalGround = ground.transform.GetChild(2).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchInversion(bool inversion)
    {
        inverseGround.GetComponent<TilemapCollider2D>().enabled = inversion;
        inverseGround.GetComponent<TilemapRenderer>().enabled = inversion;
        normalGround.GetComponent<TilemapCollider2D>().enabled = !inversion;
        normalGround.GetComponent<TilemapRenderer>().enabled = !inversion;
    }

    public void CompleteLevel()
    {
        LoadNextLevel();
    }

    public void LoadNextLevel()
    {
        string currentName = SceneManager.GetActiveScene().name;
        int currentLevel = Int32.Parse(currentName.Remove(0,6));
        SceneManager.LoadScene(levelPrefix+(currentLevel+1));
    }

    public void LoadSpecificLevel(string sceneName)
    {

    }
}
