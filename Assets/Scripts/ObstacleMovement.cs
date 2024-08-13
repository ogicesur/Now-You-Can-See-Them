using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMovement : MonoBehaviour
{
    private Vector3 moveDirection;
    private float moveSpeed;

    public void Initialize(Vector3 direction, float speed)
    {
        moveDirection = direction;
        moveSpeed = speed;

    }


    // Update is called once per frame
    void Update()
    {
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
        // 可以根据需要添加逻辑来销毁障碍物，例如超出一定范围后销毁
        /* if (transform.position.x < -10 || transform.position.x > 10 ||
             transform.position.z < -10 || transform.position.z > 10)
         {
             Destroy(gameObject);
         }*/
    }
}
