using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeaveButton : MonoBehaviour
{
    private bool inverted;
    private Color color;

    // Start is called before the first frame update
    void Start()
    {
        color = transform.GetChild(0).GetComponent<TextMeshProUGUI>().color;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!inverted)
            {
                transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color(1-color.r,1-color.g,1-color.b);
                Shader.SetGlobalFloat("_InvertColors", 1);
                inverted = true;
            }
            else
            {
                transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = color;
                Shader.SetGlobalFloat("_InvertColors", 0);
                inverted = false;
            }
        }
    }

    public void Quit()
    {
        Application.Quit();
    }
}
