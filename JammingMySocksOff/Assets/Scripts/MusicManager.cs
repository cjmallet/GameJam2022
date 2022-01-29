using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    private static MusicManager instance;
    public static MusicManager Instance
    {
        get { return instance; }
    }

    [SerializeField] private AudioClip[] mainMenuMusic;
    [SerializeField] private AudioClip[] levelMusic;
    private string sceneName;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else { Destroy(this); }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name=="MainMenu")
        {
            GetComponent<AudioSource>().clip= mainMenuMusic[Random.Range(0,2)];
            GetComponent<AudioSource>().Play();
            sceneName = SceneManager.GetActiveScene().name;
        }
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name!=sceneName &&!SceneManager.GetActiveScene().name.Contains("MainMenu"))
        {
            GetComponent<AudioSource>().Stop();
            GetComponent<AudioSource>().clip= levelMusic[Random.Range(0,2)];
            GetComponent<AudioSource>().Play();
            sceneName = SceneManager.GetActiveScene().name;
        }

        if (SceneManager.GetActiveScene().name != sceneName && SceneManager.GetActiveScene().name.Contains("MainMenu"))
        {
            GetComponent<AudioSource>().Stop();
            GetComponent<AudioSource>().clip = mainMenuMusic[Random.Range(0, 2)];
            GetComponent<AudioSource>().Play();
            sceneName = SceneManager.GetActiveScene().name;
        }
    }
}
