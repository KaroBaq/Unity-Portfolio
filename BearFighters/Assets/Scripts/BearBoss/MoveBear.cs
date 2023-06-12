using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveBear : MonoBehaviour
{    
    private float speed = 5.0f; 
    private float tolPos = 6.0f;
    private float minDistBearPlayer = 3.0f;
    private float hitForce = 20000.0f;

    private int lives = 3;

    private string currentAnim;

    private bool hit = false;

    private GameObject player;
    [SerializeField]
    public GameObject[] hearts;

    private Rigidbody bearRb;
    private Animator bearAnim;
    private BossManager bossManager;
    private AudioSource bearAudio;
    [SerializeField]
    public AudioClip crash;
    
    // Start is called before the first frame update
    IEnumerator Start()
    {
        // Wait for the character to be ready
        yield return new WaitUntil(() => GameObject.Find("Boss Manager").GetComponent<BossManager>().isInitialized);
        bossManager = GameObject.Find("Boss Manager").GetComponent<BossManager>();
        player = bossManager.player;

        bearRb = GetComponent<Rigidbody>();
        bearAnim = GetComponent<Animator>();
        bearAudio = GetComponent<AudioSource>();        
    }

    // Update is called once per frame
    void LateUpdate()
    {    
        if (player != null && bossManager.gameOver == false)
        {
            // If the player is near, random attack
            if (Mathf.Abs(transform.position.x - player.transform.position.x) < minDistBearPlayer)
            {
                int randomIndex = Random.Range(1, 5);
                Anim("Attack" + randomIndex);
            }
            else
            {
                Anim("Run Forward");

                if (transform.position.x + tolPos <= player.transform.position.x)
                {
                    speed = Mathf.Abs(speed);
                    transform.rotation = Quaternion.Euler(0, 90, 0);
                }
                if (transform.position.x - tolPos > player.transform.position.x)
                {
                    speed = -Mathf.Abs(speed);
                    transform.rotation = Quaternion.Euler(0, -90, 0);
                }

                bearRb.velocity = new Vector3(speed, bearRb.velocity.y, bearRb.velocity.z);
            }            
        }             
    }

    public void Anim(string nextAnim)
    {
        if (currentAnim != null)
        {
            bearAnim.SetBool(currentAnim, false);
        }

        bearAnim.SetBool(nextAnim, true);
        currentAnim = nextAnim;
    }
    

    private void OnTriggerEnter(Collider other)
    {
        if (hit == false)
        {
            // Check if bear was hit by sword
            if (other.gameObject.CompareTag("Sword"))
            {
                // Throw the bear to the opposite direction
                if (other.transform.position.x > transform.position.x)
                {
                    bearRb.AddForce(Vector3.left * hitForce, ForceMode.Impulse);
                }
                else
                {
                    bearRb.AddForce(Vector3.right * hitForce, ForceMode.Impulse);
                }
                // Animate the hit
                Anim("GetHit");
            }
            // Update lives 
            StartCoroutine(UpdateLivesBear());
            bearAudio.PlayOneShot(crash, 1.0f);
        }
        
    }

    IEnumerator UpdateLivesBear()
    {
        hit = true;
        lives -= 1;

        if (lives < 0)
        {
            Anim("Death");
            bossManager.player.GetComponent<PlayerController>().Anim("Idle");
            StartCoroutine(WaitLoadScene());            
        }
        if (lives > -1)
        {
            hearts[lives].SetActive(false);
        }
        // Add a damage delay between enemy/ player contact hits
        yield return new WaitForSeconds(0.5f);
        hit = false;
    }

    IEnumerator WaitLoadScene()
    {
        bossManager.gameOver = true;
        Physics.autoSimulation = false;
        yield return new WaitForSeconds(5.0f);
        SceneManager.LoadScene(2);
    }
}