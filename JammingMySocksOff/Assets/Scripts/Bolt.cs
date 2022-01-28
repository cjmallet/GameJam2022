using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bolt : Pickups
{
    [SerializeField] private int scoreBonus;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        if (collision.transform.tag == "Player")
        {
            GameManager.Instance.levelManager.AddScore(scoreBonus);
            Destroy(transform.gameObject);
        }
    }
}
