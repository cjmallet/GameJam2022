using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private int scoreLossPerSecond;
    private GameObject normalGround;
    private GameObject inverseGround;
    private GameObject ground;
    private GameObject levelUI;
    private GameObject endLevelUI;
    private static string levelPrefix="Level_";
    private float timer;
    private int score=200;

    // Start is called before the first frame update
    void Start()
    {
        ground=GameObject.Find("Ground");
        levelUI = GameObject.Find("InGameUI");
        endLevelUI = GameObject.Find("EndLevelUI");
        inverseGround = ground.transform.GetChild(1).gameObject;
        normalGround = ground.transform.GetChild(2).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        levelUI.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Score: " + score;
        levelUI.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Time: " + Mathf.RoundToInt(timer);
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
        //(Mathf.RoundToInt(score-(scoreLossPerSecond*timer))).ToString()
        string currentName = SceneManager.GetActiveScene().name;
        int currentLevel = Int32.Parse(currentName.Remove(0,6));
        SceneManager.LoadScene(levelPrefix+(currentLevel+1));
    }

    public void LoadSpecificLevel(string sceneName)
    {

    }

    public void AddScore(int gain)
    {
        score += gain;
    }

    public void LowerScore(int loss)
    {
        score -= loss;
    }
}
