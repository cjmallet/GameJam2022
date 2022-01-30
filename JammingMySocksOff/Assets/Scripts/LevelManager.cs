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
    [SerializeField] private TextMeshProUGUI inputFieldText;
    [HideInInspector] public bool inverted;
    [HideInInspector] public bool nameUIopen;
    [HideInInspector] public bool paused;

    private GameObject normalGround;
    private GameObject inverseGround;
    private GameObject ground;
    private GameObject levelUI;
    private GameObject endLevelUI;
    private GameObject nameUI;
    private GameObject openInput;
    private GameObject escMenu;
    private Highscores levelHighscores;
    private bool inputMenuOpen;
    private static string levelPrefix = "Level_";
    private double multiplier;
    private string playerName;
    private float timer;
    private int lastModifiedPosition;
    private int score;
    private int finalScore;

    // Start is called before the first frame update
    void Start()
    {
        ground = GameObject.Find("Ground");
        levelUI = GameObject.Find("InGameUI");
        levelUI.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = score.ToString();
        endLevelUI = GameObject.Find("EndLevelUI");
        endLevelUI.SetActive(false);
        nameUI = GameObject.Find("NameInput");
        nameUI.SetActive(false);
        inverseGround = ground.transform.GetChild(1).gameObject;
        normalGround = ground.transform.GetChild(2).gameObject;
        escMenu = GameObject.Find("EscapeMenu");
        escMenu.SetActive(false);
        SwitchInversion(false);

        //System.IO.File.Delete(Application.persistentDataPath + "/" + SceneManager.GetActiveScene().name + ".json");
        if (System.IO.File.Exists(Application.persistentDataPath + "/" + SceneManager.GetActiveScene().name + ".json"))
        {
            levelHighscores = JsonUtility.FromJson<Highscores>(System.IO.File.ReadAllText(Application.persistentDataPath + "/" + SceneManager.GetActiveScene().name + ".json"));
        }
       
        score = 0;
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        levelUI.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Time: " + Mathf.RoundToInt(timer);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseLevel(!paused);
        }
    }

    public void SwitchInversion(bool inversion)
    {
        inverted = inversion;
        inverseGround.GetComponent<TilemapCollider2D>().enabled = inversion;
        inverseGround.GetComponent<TilemapRenderer>().enabled = inversion;
        normalGround.GetComponent<TilemapCollider2D>().enabled = !inversion;
        normalGround.GetComponent<TilemapRenderer>().enabled = !inversion;
    }

    public void CompleteLevel()
    {
        Time.timeScale = 0;
        
        levelUI.SetActive(false);

        multiplier = Math.Round((double)maxModifier / Mathf.RoundToInt(timer), 1);
        if (multiplier < 1)
        {
            multiplier = 1;
        }

        finalScore = Mathf.RoundToInt(score * (float)multiplier);

        if (levelHighscores!=null)
        {
            LoadHighscores();
        }
        else
        {
            UploadHighscore(1);
            endLevelUI.SetActive(false);
            nameUI.SetActive(true);
        }
    }

    public void SaveHighscore()
    {
        string jsonHighscore = JsonUtility.ToJson(levelHighscores);
        if (System.IO.File.Exists(Application.persistentDataPath + "/" + SceneManager.GetActiveScene().name + ".json"))
        {
            System.IO.File.Delete(Application.persistentDataPath + "/" + SceneManager.GetActiveScene().name + ".json");
        }
        System.IO.File.WriteAllText(Application.persistentDataPath + "/" + SceneManager.GetActiveScene().name + ".json", jsonHighscore);

        endLevelUI.SetActive(true);
        nameUI.SetActive(false);

        endLevelUI.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Score: " + score;
        endLevelUI.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = "Time Multiplier: " + multiplier;
        endLevelUI.transform.GetChild(4).GetComponent<TextMeshProUGUI>().text = "Total Score: " + finalScore.ToString();
        List<GameObject> highscoreObjects = new List<GameObject>();

        for (int x = 0; x < endLevelUI.transform.GetChild(5).childCount; x++)
        {
            Transform child = endLevelUI.transform.GetChild(5).GetChild(x);
            child.GetChild(0).GetComponent<TextMeshProUGUI>().text = levelHighscores.allHighScores[x].position.ToString();
            child.GetChild(1).GetComponent<TextMeshProUGUI>().text = levelHighscores.allHighScores[x].name;
            child.GetChild(2).GetComponent<TextMeshProUGUI>().text = levelHighscores.allHighScores[x].highScore.ToString();
        }
    }

    private void LoadHighscores()
    {
        bool highscoreReached = false;

        foreach (Highscore highscore in levelHighscores.allHighScores)
        {
            if (finalScore>highscore.highScore)
            {
                ChangeHighscore(highscore.position);
                highscoreReached = true;
                endLevelUI.SetActive(false);
                nameUI.SetActive(true);
                break;
            }
            else
            {
                highscoreReached = false;
            }
        }

        if (levelHighscores.allHighScores.Length < 10&&!highscoreReached)
        {
            UploadHighscore(levelHighscores.allHighScores.Length - 1);
            endLevelUI.SetActive(false);
            nameUI.SetActive(true);
        }
        if(levelHighscores.allHighScores.Length==10 &&!highscoreReached)
        {
            Debug.Log("NoHighscore");
            SaveHighscore();
        }
    }

    private void UploadHighscore(int position)
    {
        Highscore highscore = CreateHighscore(position);

        if (position==1&& levelHighscores==null)
        {
            levelHighscores = new Highscores();
            levelHighscores.allHighScores = new Highscore[10];
            for (int x = position;x<=10;x++)
            {
                Highscore score = new Highscore();
                score.position =x;

                levelHighscores.allHighScores[x-1] = score;
            }
        }

        levelHighscores.allHighScores[position - 1] = highscore;
    }

    private void ChangeHighscore(int position)
    {
        Highscore highscore = CreateHighscore(position);

        Highscore oldHighscore = levelHighscores.allHighScores[position-1];
        oldHighscore.position++;
        levelHighscores.allHighScores[position-1] = highscore;
        position++;

        for (int x=position;x<=10;x++)
        {
            highscore = levelHighscores.allHighScores[x - 1];
            levelHighscores.allHighScores[x - 1]=oldHighscore;
            oldHighscore = highscore;
            oldHighscore.position++;
        }
    }

    public void GetPlayerName()
    {
        playerName = inputFieldText.text;
        levelHighscores.allHighScores[lastModifiedPosition].name = playerName;
        SaveHighscore();
    }

    private Highscore CreateHighscore(int position)
    {
        Highscore highscore = new Highscore();
        highscore.position = position;
        highscore.name = playerName;
        highscore.highScore = finalScore;
        lastModifiedPosition = position - 1;

        return highscore;
    }

    public void LoadNextLevel()
    {
        string currentName = SceneManager.GetActiveScene().name;
        int currentLevel = Int32.Parse(currentName.Remove(0, 6));
        SceneManager.LoadScene(levelPrefix + (currentLevel + 1));
        Time.timeScale = 1;
    }

    public void LoadSpecificLevel(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        Time.timeScale = 1;
    }

    public void PauseLevel(bool isPaused)
    {
        paused = isPaused;
        if (isPaused)
        {
            Time.timeScale = 0;
            escMenu.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            escMenu.SetActive(false);
        }
    }

    public void AddScore(int gain)
    {
        score += gain;
        levelUI.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = score.ToString();
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    [Serializable]
    public class Highscores
    {
        public Highscore[] allHighScores;
    }

    [Serializable]
    public class Highscore
    {
        public int position;
        public int highScore;
        public string name;
    }
}
