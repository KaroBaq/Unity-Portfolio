using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private float minTimeSpawn = 1.0f;
    private float maxTimeSpawn = 2.0f;

    [SerializeField]
    private GameObject[] objects;   

    void Start()
    {
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        while (true)
        {
            float time = Random.Range(minTimeSpawn, maxTimeSpawn);
            int randomIndex;

            if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0)
            {
                randomIndex = Random.Range(0, objects.Length);
            }
            else
            {
                // If the player isn't moving forward send enemies that walk
                randomIndex = Random.Range(2, 4);                
            }
            Instantiate(objects[randomIndex]);
            yield return new WaitForSeconds(time);            
        }        
    }
}
