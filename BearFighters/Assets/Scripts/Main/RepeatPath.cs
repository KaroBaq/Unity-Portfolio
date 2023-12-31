using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatPath : MonoBehaviour
{
    private Vector3 startPos;
    private float widthRepetition;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        widthRepetition = GetComponent<BoxCollider>().size.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < startPos.x - widthRepetition/2)
        {
            transform.position = startPos;
        }
    }
}