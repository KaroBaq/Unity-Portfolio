using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainManager : MonoBehaviour
{    
    public int index { get; private set; }
    private int lives = 3;
    private int gameTime = 30; // Time until the scene change
    
    public bool gameOver = false;    
    public bool isInitialized { get; private set; }

    public float xRightLimit { get; private set; }
    public float xLeftLimit { get; private set; }

    [SerializeField]
    public TMP_Text time;

    public GameObject player;
    [SerializeField]
    private GameObject[] character;
    [SerializeField]
    private GameObject[] hearts;

    // Start is called before the first frame update
    void Start()
    {
        Physics.autoSimulation = true;

        // Load selection of character
        if (DataManager.Instance != null)
        {
            index = DataManager.Instance.LoadData();
        }
        else
        {
            index = 0;
        }
        player = Instantiate(character[index], character[index].transform.position, character[index].transform.rotation);
        // Indicate that the character is ready
        isInitialized = true;

        //Set limits
        xRightLimit = 0.0f;
        xLeftLimit = -6.0f;

        // Mark time until the scene change 
        StartCoroutine(GameTime());       
    }

    public void UpdateLives()
    {
        lives -= 1;
        
        if (lives < 0)
        {
            gameOver = true;
            player.GetComponent<PlayerController>().Anim("Die1");
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
        yield return new WaitForSeconds(4.0f);
        SceneManager.LoadScene(3);
    }

    public void ReturnMenu()
    {
        SceneManager.LoadScene(0);
    }

    IEnumerator GameTime()
    {
        for (int i = 0; i < gameTime; i++)
        {
            int j = gameTime - i;
            time.text = "00:" + j;
            yield return new WaitForSeconds(1.0f);
        }

        SceneManager.LoadScene(4);
    }
}