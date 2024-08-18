using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
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
    
}*/


using System;
using UnityEngine;

public class ObstacleMovement : MonoBehaviour
{
    private Vector3 moveDirection;
    private float moveSpeed;

    // Delegate for handling the destruction of obstacles
    public event Action OnDestroyCallback;

    public void Initialize(Vector3 direction, float speed)
    {
        moveDirection = direction;
        moveSpeed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

    }

    private void DestroyObstacle()
    {
        // Trigger the OnDestroyCallback event before destroying the object
        OnDestroyCallback?.Invoke();
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        // Ensure the callback is called if the object is destroyed through other means
        OnDestroyCallback?.Invoke();
    }
}
