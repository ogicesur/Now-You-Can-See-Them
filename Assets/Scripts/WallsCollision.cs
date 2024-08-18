using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallsCollision : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacles"))
        {
            Debug.Log("Collision with obstacle!");
        }
        // Destroy the obstacle
        Destroy(collision.gameObject);
    }
}
