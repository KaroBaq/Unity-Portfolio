using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEnemyLeft : MonoBehaviour
{
    private float speed = -3.0f;

    private Rigidbody enemyRb;

    // Start is called before the first frame update
    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();      
    }

    // Update is called once per frame
    void LateUpdate()
    {
        enemyRb.velocity = new Vector3 (speed, enemyRb.velocity.y, enemyRb.velocity.z);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Obstacle"))
        {
            // If there is a collision turn around
            speed *= -1;
            float sgn = Mathf.Abs(speed) / speed;
            
            transform.rotation = Quaternion.Euler(0, sgn * 90, 0);            
        }
    }
}