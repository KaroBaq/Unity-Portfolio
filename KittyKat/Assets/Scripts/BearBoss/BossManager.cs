using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossManager : MonoBehaviour
{
    
    public int index { get; private set; }
    private int lives = 3;
    
    public bool gameOver = false;    
    public bool isInitialized { get; private set; }

    public float xRightLimit { get; private set; }
    public float xLeftLimit { get; private set; }

    public GameObject player;
    [SerializeField]
    public GameObject[] character;
    [SerializeField]
    public GameObject[] hearts;

    // Start is called before the first frame update
    void Start()
    {
        Physics.autoSimulation = true;

        // Load character
        if (DataManager.Instance != null)
        {
            index = DataManager.Instance.LoadData();
        }
        else
        {
            index = 0;
        }
        player = Instantiate(character[index], character[index].transform.position, character[index].transform.rotation);
        isInitialized = true;

        //Set Limits for all objects
        xRightLimit = 15.0f;
        xLeftLimit = -6.0f;
    }

    public void UpdateLives()
    {
        lives -= 1;

        if (lives < 0)
        {
          
            StartCoroutine(WaitGameOver());
        }
        if (lives > -1)
        {
            hearts[lives].SetActive(false);
        }
    }

    IEnumerator WaitGameOver()
    {
        Physics.autoSimulation = false;
        gameOver = true;
        player.GetComponent<PlayerController>().Anim("Die1");
        GameObject.Find("Bear_4").GetComponent<MoveBear>().Anim("Eat");
        yield return new WaitForSeconds(5.0f);
        SceneManager.LoadScene(3);
    }
}