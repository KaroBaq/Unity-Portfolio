using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    private float destroyXLimitNeg = -10.0f;
    private float destroyXLimitPos = 17.0f;

    public ParticleSystem explosion;
    private AudioSource enemAudio;
    public AudioClip crash;

    // Start is called before the first frame update
    void Start()
    {
        enemAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // Delete object once they are off limits
        if (transform.position.x < destroyXLimitNeg || transform.position.x > destroyXLimitPos)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Sword") && gameObject.CompareTag("Enemy"))
        {
            enemAudio.PlayOneShot(crash, 1.0f);
            explosion.Play();
            Destroy(gameObject, 0.2f);
        }
    }
}