using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class AiMovement : MonoBehaviour
{
    // 公开的最大移动速度
    public float maxMovementSpeed = 20f;
    // 公开的Rigidbody2D组件，用于控制冰球的物理行为
    public Rigidbody2D puck;
    // 公开的Transform组件，用于获取边界的位置
    public Transform boundaryHolder;

    // 私有变量，用于存储玩家边界（这里应该是冰球活动的边界）
    // 注意：float4 并不是Unity或C#的标准类型，这里可能是一个错误或者自定义的结构体
    private float4 _playerBoundary;
    // 私有的Rigidbody2D组件引用
    private Rigidbody2D _rb;
    // 冰球的起始位置
    private Vector2 _startingPos;
    // 冰球的目标位置
    private Vector2 _targetPos;

    // 上一次的方向
    private Vector2 _oldDirection;
    // 用于在方向改变时添加随机偏移的X轴值
    private float _offsetX;

    // 标记冰球是否在顶部区域（基于Y轴的正负）
    private bool _isTop;

    // 默认的移动速度
    float _movementSpeed = .35f;
    // 冰球的半径，用于计算边界
    private float _playerRadius;

    void Start()
    {
        // 获取并存储Rigidbody2D组件的引用
        _rb = GetComponent<Rigidbody2D>();
        // 获取并存储冰球的起始位置
        _startingPos = _rb.position;
        // 获取并存储CircleCollider2D组件的半径
        _playerRadius = GetComponent<CircleCollider2D>().radius;

        // 根据冰球的Y轴位置确定是否在顶部区域
        _isTop = transform.position.y > 0;


        // 根据是否在顶部区域，设置不同的边界值
        if (_isTop)
        {
            // 顶部区域的边界设置
            _playerBoundary = new float4(
                boundaryHolder.GetChild(0).position.x + _playerRadius,
                boundaryHolder.GetChild(1).position.x - _playerRadius,
                boundaryHolder.GetChild(3).position.y + _playerRadius,
                -boundaryHolder.GetChild(2).position.y - _playerRadius
            );
        }
        else
        {
            // 底部区域的边界设置
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
        // 获取当前位置
        var position = _rb.position;
        // 计算目标方向
        var direction = (_targetPos - position).normalized;

        // 检查方向是否改变
        bool hasDirectionChanged = DidDirectionChange(direction);
        // 如果方向改变，添加随机偏移
        if (hasDirectionChanged)
        {
            _offsetX = _playerRadius * Random.Range(-1f, 1f);
        }

        // 根据冰球的位置和是否在顶部区域，调整目标位置和移动速度
        if ((_isTop && puck.position.y <= 0) || (!_isTop && puck.position.y >= 0))
        {
            // 触碰边界时的处理
            if (hasDirectionChanged)
            {
                _movementSpeed = maxMovementSpeed * Random.Range(.1f, .3f);
            }

            _targetPos = new Vector2(Mathf.Clamp(puck.position.x + _offsetX, _playerBoundary[0], _playerBoundary[1]),
                _startingPos.y);
        }
        else
        {
            // 在边界内的处理
            if (hasDirectionChanged)
            {
                _movementSpeed = maxMovementSpeed * Random.Range(.4f, 1f);
            }

            _targetPos = new Vector2(Mathf.Clamp(puck.position.x + _offsetX, _playerBoundary[0], _playerBoundary[1]),
                Mathf.Clamp(puck.position.y, _playerBoundary[2], _playerBoundary[3]));
        }

        // 移动冰球到目标位置
        _rb.MovePosition(Vector2.MoveTowards(position, _targetPos, _movementSpeed * Time.fixedDeltaTime));

        // 更新上一次的方向
        _oldDirection = direction;
    }

    // 检查方向是否改变的方法
    bool DidDirectionChange(Vector2 direction)
    {
        // 如果当前有方向且上一次没有方向，则视为方向改变
        if (direction.sqrMagnitude > 0 && _oldDirection.sqrMagnitude == 0f) return true;
        // 计算当前方向和上一次方向之间的角度差
        float angle = Vector2.Angle(direction, _oldDirection);
        // 如果角度差大于20度，则视为方向改变
        return angle > 20;
    }
}
