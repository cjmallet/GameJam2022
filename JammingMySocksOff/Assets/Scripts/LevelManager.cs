using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private int maxModifier;
    [HideInInspector] public bool inverted;

    private GameObject normalGround;
    private GameObject inverseGround;
    private GameObject ground;
    private GameObject levelUI;
    private GameObject endLevelUI;
    private static string levelPrefix="Level_";
    private float timer;
    private int score;

    // Start is called before the first frame update
    void Start()
    {
        ground=GameObject.Find("Ground");
        levelUI = GameObject.Find("InGameUI");
        levelUI.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Score: " + score;
        endLevelUI = GameObject.Find("EndLevelUI");
        endLevelUI.SetActive(false);
        inverseGround = ground.transform.GetChild(1).gameObject;
        normalGround = ground.transform.GetChild(2).gameObject;
        score = 0;
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        levelUI.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Time: " + Mathf.RoundToInt(timer);
    }

    public void SwitchInversion(bool inversion)
    {
        inverted=inversion;
        inverseGround.GetComponent<TilemapCollider2D>().enabled = inversion;
        inverseGround.GetComponent<TilemapRenderer>().enabled = inversion;
        normalGround.GetComponent<TilemapCollider2D>().enabled = !inversion;
        normalGround.GetComponent<TilemapRenderer>().enabled = !inversion;
    }

    public void CompleteLevel()
    {
        Time.timeScale = 0;
        endLevelUI.SetActive(true);
        levelUI.SetActive(false);

        double multiplier = Math.Round((double)maxModifier / Mathf.RoundToInt(timer), 1);
        if (multiplier<1)
        {
            multiplier = 1;
        }
        
        endLevelUI.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Score: " + score;
        endLevelUI.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "Time Multiplier: " +multiplier;
        endLevelUI.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Total Score: " + (Mathf.RoundToInt(score * (float)multiplier)).ToString();
    }

    public void LoadNextLevel()
    {
        string currentName = SceneManager.GetActiveScene().name;
        int currentLevel = Int32.Parse(currentName.Remove(0,6));
        SceneManager.LoadScene(levelPrefix+(currentLevel+1));
        Time.timeScale = 1;
    }

    public void LoadSpecificLevel(string sceneName)
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1;
    }

    public void AddScore(int gain)
    {
        score += gain;
        levelUI.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Score: " + score;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
