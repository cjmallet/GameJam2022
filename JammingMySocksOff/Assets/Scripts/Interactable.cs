using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] private Sprite activatedSprite;
    private GameObject exit;
    private GameObject buttonPrompt;
    private bool activated,inRange;

    // Start is called before the first frame update
    void Start()
    {
        exit = GameObject.Find("Exit");
        buttonPrompt = transform.GetChild(0).gameObject;
        buttonPrompt.SetActive(false);
    }

    private void Update()
    {
        if (inRange&&Input.GetKeyDown(KeyCode.F)&&!activated)
        {
            activated = true;
            buttonPrompt.SetActive(false);
            GetComponent<SpriteRenderer>().sprite = activatedSprite;
            exit.GetComponent<Exit>().OpenExit();
            GetComponent<AudioSource>().Play();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!activated)
        {
            buttonPrompt.SetActive(true);
            inRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")&&buttonPrompt.activeSelf)
        {
            buttonPrompt.SetActive(false);
            inRange = false;
        }
    }
}
