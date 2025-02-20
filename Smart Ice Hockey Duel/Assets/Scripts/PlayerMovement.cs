using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Networking.PlayerConnection;

[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    // �߽�����ߵ�Transform�������Ӧ�ð����ĸ��Ӷ��󣬶�������ҵ��ƶ��߽�
    public Transform boundaryHolder;

    // ˽�в���ֵ�����ڸ��ٶ����Ƿ񱻵���Լ��Ƿ�����ƶ�
    private bool _hasBeenClicked, _canMove;
    // �����Rigidbody2D��Collider2D���
    private Rigidbody2D _rb;
    private Collider2D _collider;

    // ��ҵı߽磬ʹ��float4����Unity��ͨ��ʹ��Vector4��ֱ����ĸ�float������
    private float[] _playerBoundary = new float[4];

    // Start�ڵ�һ֡����֮ǰ������
    void Start()
    {
        // ��ȡ�����Rigidbody2D��Collider2D���
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();

        // ���Խ�Collider2Dת��ΪCircleCollider2D��ע�⣺��Ҫ�����ʵ������һ��CircleCollider2D�����
        var circleCol = _collider as CircleCollider2D;
        if (circleCol != null && boundaryHolder != null && boundaryHolder.childCount >= 4)
        {
            // ������ҵı߽磬����boundaryHolder���Ӷ���λ�ú�CircleCollider2D�İ뾶
            _playerBoundary[0] = boundaryHolder.GetChild(0).position.x + circleCol.radius; // �ұ߽�
            _playerBoundary[1] = boundaryHolder.GetChild(1).position.x - circleCol.radius; // ��߽�
            _playerBoundary[2] = boundaryHolder.GetChild(2).position.y + circleCol.radius; // �ϱ߽�
            _playerBoundary[3] = boundaryHolder.GetChild(3).position.y - circleCol.radius; // �±߽�
        }
        else
        {
            Debug.LogError("BoundaryHolder is not set up correctly or CircleCollider2D is missing.");
        }
    }

    // Updateÿ֡������һ��
    void Update()
    {
        // ��갴���¼�
        if (Input.GetMouseButtonDown(0))
        {
            // ��ȡ���λ�ã�������Ƿ���Collider2D�ص�
            var mousePos = GetMousePos();
            _canMove = _collider.OverlapPoint(mousePos);
        }

        // ��ק�¼�
        if (Input.GetMouseButton(0))
        {
            // ��������ƶ�������¶����λ�õ����λ��
            if (_canMove)
            {
                var mousePos = GetMousePos();
                _rb.MovePosition(mousePos);
            }
        }

        // ��������������Ļ����ת��Ϊ�������꣬����������ұ߽���
        Vector2 GetMousePos()
        {
            // �������Ļλ��ת��Ϊ��������
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // �����λ����������ұ߽���
            var clampedMousePos = new Vector2(
                Mathf.Clamp(mousePos.x, _playerBoundary[0], _playerBoundary[1]),
                Mathf.Clamp(mousePos.y, _playerBoundary[2], _playerBoundary[3])
            );

            return clampedMousePos;
        }
    }
}