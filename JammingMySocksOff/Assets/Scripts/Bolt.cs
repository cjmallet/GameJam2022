using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bolt : Pickups
{
    [SerializeField] private int scoreBonus;
    [SerializeField] private AudioClip[] audioClips;
    private AudioSource collectionSound;

    // Start is called before the first frame update
    void Start()
    {
        collectionSound = GetComponent<AudioSource>();
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
            StartCoroutine("StartCollection");
        }
    }

    private IEnumerator StartCollection()
    {
        if (GetComponent<ParticleSystem>()!=null)
        {
            GetComponent<ParticleSystem>().Stop();
        }
        GetComponent<BoxCollider2D>().enabled=false;
        GetComponent<SpriteRenderer>().enabled = false;
        GameManager.Instance.levelManager.AddScore(scoreBonus);
        collectionSound.clip = audioClips[Random.Range(0,3)];
        collectionSound.Play();
        yield return new WaitForSeconds(collectionSound.clip.length);
        Destroy(transform.gameObject);
    }
}
