using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GenericUIChoices : MonoBehaviour
{
    [SerializeField] private Sprite[] previews;
    private Color textStartColor;
    private bool inverted;

    private void Start()
    {
        if (SceneManager.GetActiveScene().name=="MainMenu")
        {
            textStartColor = transform.GetChild(0).GetComponent<TextMeshProUGUI>().color;
        }
        Shader.SetGlobalFloat("_InvertColors", 0);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)&& SceneManager.GetActiveScene().name == "MainMenu")
        {
            if (!inverted)
            {
                inverted = true;
                GetComponent<Image>().color = new Color(0,0,0);
                transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color(1-textStartColor.r,1-textStartColor.g,1-textStartColor.b);
                transform.GetChild(1).GetComponent<Image>().sprite = previews[1];
                Shader.SetGlobalFloat("_InvertColors", 1);
            }
            else
            {
                inverted = false;
                GetComponent<Image>().color = new Color(1, 1, 1);
                transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = textStartColor;
                transform.GetChild(1).GetComponent<Image>().sprite = previews[0];
                Shader.SetGlobalFloat("_InvertColors", 0);
            }
        }
    }

    public void ChangeScene(string sceneName)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(sceneName);
    }
}
