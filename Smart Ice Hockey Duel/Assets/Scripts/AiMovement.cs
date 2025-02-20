using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class AiMovement : MonoBehaviour
{
    // ����������ƶ��ٶ�
    public float maxMovementSpeed = 20f;
    // ������Rigidbody2D��������ڿ��Ʊ����������Ϊ
    public Rigidbody2D puck;
    // ������Transform��������ڻ�ȡ�߽��λ��
    public Transform boundaryHolder;

    // ˽�б��������ڴ洢��ұ߽磨����Ӧ���Ǳ����ı߽磩
    // ע�⣺float4 ������Unity��C#�ı�׼���ͣ����������һ����������Զ���Ľṹ��
    private float4 _playerBoundary;
    // ˽�е�Rigidbody2D�������
    private Rigidbody2D _rb;
    // �������ʼλ��
    private Vector2 _startingPos;
    // �����Ŀ��λ��
    private Vector2 _targetPos;

    // ��һ�εķ���
    private Vector2 _oldDirection;
    // �����ڷ���ı�ʱ������ƫ�Ƶ�X��ֵ
    private float _offsetX;

    // ��Ǳ����Ƿ��ڶ������򣨻���Y���������
    private bool _isTop;

    // Ĭ�ϵ��ƶ��ٶ�
    float _movementSpeed = .35f;
    // ����İ뾶�����ڼ���߽�
    private float _playerRadius;

    void Start()
    {
        // ��ȡ���洢Rigidbody2D���������
        _rb = GetComponent<Rigidbody2D>();
        // ��ȡ���洢�������ʼλ��
        _startingPos = _rb.position;
        // ��ȡ���洢CircleCollider2D����İ뾶
        _playerRadius = GetComponent<CircleCollider2D>().radius;

        // ���ݱ����Y��λ��ȷ���Ƿ��ڶ�������
        _isTop = transform.position.y > 0;


        // �����Ƿ��ڶ����������ò�ͬ�ı߽�ֵ
        if (_isTop)
        {
            // ��������ı߽�����
            _playerBoundary = new float4(
                boundaryHolder.GetChild(0).position.x + _playerRadius,
                boundaryHolder.GetChild(1).position.x - _playerRadius,
                boundaryHolder.GetChild(3).position.y + _playerRadius,
                -boundaryHolder.GetChild(2).position.y - _playerRadius
            );
        }
        else
        {
            // �ײ�����ı߽�����
            _playerBoundary = new float4(
                boundaryHolder.GetChild(0).position.x + _playerRadius,
                boundaryHolder.GetChild(1).position.x - _playerRadius,
                boundaryHolder.GetChild(2).position.y + _playerRadius,
                -boundaryHolder.GetChild(3).position.y - _playerRadius
            );
        }
    }

    void FixedUpdate()
    {
        // ��ȡ��ǰλ��
        var position = _rb.position;
        // ����Ŀ�귽��
        var direction = (_targetPos - position).normalized;

        // ��鷽���Ƿ�ı�
        bool hasDirectionChanged = DidDirectionChange(direction);
        // �������ı䣬������ƫ��
        if (hasDirectionChanged)
        {
            _offsetX = _playerRadius * Random.Range(-1f, 1f);
        }

        // ���ݱ����λ�ú��Ƿ��ڶ������򣬵���Ŀ��λ�ú��ƶ��ٶ�
        if ((_isTop && puck.position.y <= 0) || (!_isTop && puck.position.y >= 0))
        {
            // �����߽�ʱ�Ĵ���
            if (hasDirectionChanged)
            {
                _movementSpeed = maxMovementSpeed * Random.Range(.1f, .3f);
            }

            _targetPos = new Vector2(Mathf.Clamp(puck.position.x + _offsetX, _playerBoundary[0], _playerBoundary[1]),
                _startingPos.y);
        }
        else
        {
            // �ڱ߽��ڵĴ���
            if (hasDirectionChanged)
            {
                _movementSpeed = maxMovementSpeed * Random.Range(.4f, 1f);
            }

            _targetPos = new Vector2(Mathf.Clamp(puck.position.x + _offsetX, _playerBoundary[0], _playerBoundary[1]),
                Mathf.Clamp(puck.position.y, _playerBoundary[2], _playerBoundary[3]));
        }

        // �ƶ�����Ŀ��λ��
        _rb.MovePosition(Vector2.MoveTowards(position, _targetPos, _movementSpeed * Time.fixedDeltaTime));

        // ������һ�εķ���
        _oldDirection = direction;
    }

    // ��鷽���Ƿ�ı�ķ���
    bool DidDirectionChange(Vector2 direction)
    {
        // �����ǰ�з�������һ��û�з�������Ϊ����ı�
        if (direction.sqrMagnitude > 0 && _oldDirection.sqrMagnitude == 0f) return true;
        // ���㵱ǰ�������һ�η���֮��ĽǶȲ�
        float angle = Vector2.Angle(direction, _oldDirection);
        // ����ǶȲ����20�ȣ�����Ϊ����ı�
        return angle > 20;
    }
}
