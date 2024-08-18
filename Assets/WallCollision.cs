using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCollision : MonoBehaviour
{
 
  

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacles"))
        {
            Debug.Log("Collision with obstacle!");


            // Destroy the obstacle
            Destroy(collision.gameObject);

        }
    }


}
