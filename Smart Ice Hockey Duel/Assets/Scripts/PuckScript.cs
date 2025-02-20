using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PuckScript : MonoBehaviour
{
    // 引用ScoreScript以更新得分
    public ScoreScript scoreScript;
    // 对象的最大速度
    public float maxSpeed = 20f;
    // 当碰撞发生时播放的声音对象
    public GameObject voice;

    // 对象的Rigidbody2D组件
    private Rigidbody2D _rb;
    // 标记是否已经达到目标
    private bool _wasGoal = false;

    public bool IsStart;
    private float _lastXPosition;
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("PlayerGoal"))
        {
            scoreScript.Increment(false);
            _wasGoal = true;
            StartCoroutine(ResetPuck(false));
        }
        else if (col.CompareTag("AiGoal"))
        {
            scoreScript.Increment(true);
            _wasGoal = true;
            StartCoroutine(ResetPuck(true));
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.name == "PlayerRed" || collision.transform.name == "PlayerBlue" || collision.transform.name == "Barrier")
        {
            GameObject a = Instantiate(voice);
            Destroy(a, 1f);
        }
    }

    IEnumerator ResetPuck(bool didAiScore)
    {
        yield return new WaitForSeconds(1);
        _rb.velocity = Vector2.zero;
        _wasGoal = false;

        if (didAiScore)
        {
            _rb.position = Vector2.down;
        }
        else
        {
            _rb.position = Vector2.up;
        }
    }

    private void FixedUpdate()
    {
        if (_wasGoal)
        {
            _rb.velocity = Vector2.zero;
        }
        else
        {
            _rb.velocity = Vector2.ClampMagnitude(_rb.velocity, maxSpeed);
        }
    }
}
