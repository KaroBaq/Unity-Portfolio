using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    private GameObject player;

    private float xRightLimit;
    private float epsilon = 0.1f;    
    private float maxSpeed = 10.0f;

    private bool inUp;

    IEnumerator Start()
    {
        // Wait until Main loads the player instance
        yield return new WaitUntil(() => GameObject.Find("Main Manager").GetComponent<MainManager>().isInitialized);
        player = GameObject.Find("Main Manager").GetComponent<MainManager>().player;        
        xRightLimit = GameObject.Find("Main Manager").GetComponent<MainManager>().xRightLimit;
        inUp = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (inUp == true)
        {
            float speed = Input.GetAxis("Horizontal") * maxSpeed;
            if (player.transform.position.x > xRightLimit - epsilon && speed > 0)
            {
                transform.Translate(Vector3.left * speed * Time.deltaTime, Space.World);
            }            
        }    
    }
}