using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketJump : Pickups
{
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        if (collision.transform.CompareTag("Player"))
        {
            collision.GetComponent<playerController>().rocketJumpUnlocked = true;
            Destroy(this.gameObject);
        }
    }
}
