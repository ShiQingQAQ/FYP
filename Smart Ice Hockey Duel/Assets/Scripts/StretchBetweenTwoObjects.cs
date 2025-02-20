using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StretchBetweenTwoObjects : MonoBehaviour
{
    public Transform object1;
    public Transform object2;
    public float R = 0f;
    void Update()
    {
        Vector3 direction = object2.position - object1.position;

        // 设置 LineObject 的位置为两个物体的中点
        transform.position = object1.position + 0.5f * direction;

        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, direction);
        rotation *= Quaternion.Euler(0, 0, R);  // 使用 R 来调整 Z 轴旋转
        transform.rotation = rotation;
    }
}
