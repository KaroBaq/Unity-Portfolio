using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatBackground : MonoBehaviour
{
    private Vector3 startPos;
    private Vector3 resetPos;

    [SerializeField] 
    private GameObject firstEnvironment;
    [SerializeField] 
    private GameObject secondEnvironment;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        resetPos = firstEnvironment.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (secondEnvironment.transform.position.x < resetPos.x)
        {
            transform.position = startPos;
        }
        
    }
}