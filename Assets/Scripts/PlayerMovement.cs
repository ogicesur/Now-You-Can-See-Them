using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerMovement : MonoBehaviourPun
{

    public float speed = 5.0f; // 移动速度
    public float rotationSpeed = 700.0f; // 旋转速度

    void Update()
    {
        if (!photonView.IsMine && PhotonNetwork.IsConnected)
            return;
        // 获取水平和垂直输入（WSAD 或 箭头键）
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // 计算移动的方向
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        // 按速度移动物体
        transform.Translate(movement * speed * Time.deltaTime, Space.World);

        // 计算旋转的角度
        if (movement != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }
    }
}