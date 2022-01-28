using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    [SerializeField] private bool opened;

    // Start is called before the first frame update
    void Start()
    {
        if (opened)
        {
            GetComponent<Animator>().SetBool("IsOpen",opened);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenExit()
    {
        opened = true;
        GetComponent<Animator>().SetBool("IsOpen", opened);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (opened)
        {
            GameManager.Instance.levelManager.CompleteLevel();
        }
    }
}
