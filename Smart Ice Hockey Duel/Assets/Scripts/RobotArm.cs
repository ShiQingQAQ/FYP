using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotArm : MonoBehaviour
{
    public Transform A0, A1, A2, Player;
    public float radius = 2.55f;

    void Update()
    {
        // A2��λ��ʼ����Playerλ�����
        A2.position = Player.position;

        // A1��λ��ֻ������A0ΪԲ�İ뾶Ϊ2.55����A2ΪԲ�İ뾶Ϊ2.55������Բ�Ľ�����
        Vector2 center1 = A0.position;
        Vector2 center2 = A2.position;

        Vector2 d = center2 - center1;
        float dist = d.magnitude;

        if (dist > 2 * radius)
        {
            Debug.Log("The circles are too far apart, no solution.");
            return;
        }
        if (dist < Mathf.Abs(radius - radius))
        {
            Debug.Log("One circle is contained in the other, no solution.");
            return;
        }

        float a = (radius * radius - radius * radius + dist * dist) / (2 * dist);
        float h = Mathf.Sqrt(radius * radius - a * a);

        Vector2 direction = d.normalized;
        Vector2 pa = center1 + a * direction;
        Vector2 s1 = pa + h * new Vector2(-direction.y, direction.x);
        Vector2 s2 = pa - h * new Vector2(-direction.y, direction.x);


        A1.position = s1;
    }
}
