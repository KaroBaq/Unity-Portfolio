using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    private float maxSpeed = 3.0f;
    private float xLeftLimit;
    private float xRightLimit;
    private float jumpForce = 300.0f;
    private float attackDuration = 0.6f;
    private float hitForce = 2500.0f;

    private int index;

    private bool jump;
    private bool hit = false;

    private string currentAnim;    

    private Rigidbody playerRb;

    private BoxCollider sword;

    private Animator animPlayer;

    private MainManager mainManager;

    private BossManager bossManager;

    private AudioSource playerSource;

    [SerializeField]    
    private AudioClip jumpSound;

    // Start is called before the first frame update
    void Start()
    {
        animPlayer = GetComponent<Animator>();
        playerRb = GetComponent<Rigidbody>();
        playerSource = GetComponent<AudioSource>();

        // Obtain the character that was chosen, depending if we are in the Main or the Bear scene

        if (GameObject.Find("Main Manager") != null)
        {
            mainManager = GameObject.Find("Main Manager").GetComponent<MainManager>();
            xRightLimit = mainManager.xRightLimit;
            xLeftLimit = mainManager.xLeftLimit;
            index = mainManager.index;
        }
        else
        {
            bossManager = GameObject.Find("Boss Manager").GetComponent<BossManager>();
            xRightLimit = bossManager.xRightLimit;
            xLeftLimit = bossManager.xLeftLimit;
            index = bossManager.index;
        }        

        // Enable the corresponding sword

        if (index == 0)
        {
            sword = GameObject.Find("OHS06").GetComponent<BoxCollider>();
        }
        else
        {
            sword = GameObject.Find("OHS03").GetComponent<BoxCollider>();
        }        
        sword.enabled = false;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // Move and animate the player if the game is not over

        if (mainManager != null)
        {
            if (!mainManager.gameOver)
            {
                MovePlayer();
            }
        }
        else
        {
            if (!bossManager.gameOver)
            {
                MovePlayer();
            }
        }                     
    }

    private void MovePlayer()
    {      
        // Move to the sides
        if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0)
        {
            float sgn = Mathf.Abs(Input.GetAxis("Horizontal")) / Input.GetAxis("Horizontal");
            transform.rotation = Quaternion.Euler(0, 90 * sgn, 0);

            float velocityChange = Input.GetAxis("Horizontal") * (maxSpeed - playerRb.velocity.x);
            playerRb.AddForce(new Vector3(velocityChange, 0, 0), ForceMode.VelocityChange);            
            Anim("Run");           
        }

        // Jump
        if (Input.GetKeyDown(KeyCode.Space) && jump == false)
        {
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            playerSource.PlayOneShot(jumpSound, 1.0f);
            jump = true;
            Anim("Jump");
        }

        // Attack       
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            if (sword.enabled == true)
            {
                StopCoroutine(ActSword());
            }

            StartCoroutine(ActSword());

            if (Input.GetMouseButtonDown(0))
            {
                Anim("Attack1");
            }
            else
            {
                Anim("Attack2");
            }
        }

        // Stay
        if (!Input.anyKey)
        {
            Anim("Idle");
        }

        // Reset position if player goes off limits
        if (transform.position.x > xRightLimit)
        {
            transform.position = new Vector3(xRightLimit, transform.position.y, transform.position.z);
        }

        if (transform.position.x < xLeftLimit)
        {
            transform.position = new Vector3(xLeftLimit, transform.position.y, transform.position.z);
        }
    }

    public void Anim(string nextAnim)
    {    
        if (currentAnim != null)
        {
            animPlayer.SetBool(currentAnim, false);
        }

        // If we are in the air prioritize jumping animation
        if (jump == true)
        {
            animPlayer.SetBool("Jump", true);
        }
        else
        {
            animPlayer.SetBool(nextAnim, true);
            animPlayer.SetBool("Jump", false);
            currentAnim = nextAnim;
        }        
    }

    IEnumerator ActSword()
    {
        // Enable sword when player is attacking
        sword.enabled = true;
        yield return new WaitForSeconds(attackDuration);
        sword.enabled = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Path") || collision.gameObject.CompareTag("Obstacle"))
        {
            jump = false;
        }

        if (hit == false)
        {            
            if (collision.gameObject.CompareTag("Enemy"))
            {
                // Throw the character to the opposite direction
                if (collision.transform.position.x > transform.position.x)
                {
                    playerRb.AddForce(Vector3.left * hitForce, ForceMode.Impulse);
                }
                else
                {
                    playerRb.AddForce(Vector3.right * hitForce, ForceMode.Impulse);
                }
                // Animate the hit
                Anim("GetHit");

                // Uodate Lives
                if (mainManager != null)
                {
                    mainManager.GetComponent<MainManager>().UpdateLives();
                }
                else
                {
                    bossManager.GetComponent<BossManager>().UpdateLives();
                }
                
            }

            // Add a damage delay between enemy/ player contact hits
            StartCoroutine(WaitAfterHit());
        }               
    }

    IEnumerator WaitAfterHit()
    {
        hit = true;
        yield return new WaitForSeconds(1.0f);
        hit = false;
    }
}