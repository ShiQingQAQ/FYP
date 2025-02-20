using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Networking.PlayerConnection;

[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    // 边界持有者的Transform组件，它应该包含四个子对象，定义了玩家的移动边界
    public Transform boundaryHolder;

    // 私有布尔值，用于跟踪对象是否被点击以及是否可以移动
    private bool _hasBeenClicked, _canMove;
    // 对象的Rigidbody2D和Collider2D组件
    private Rigidbody2D _rb;
    private Collider2D _collider;

    // 玩家的边界，使用float4（但Unity中通常使用Vector4或分别定义四个float变量）
    private float[] _playerBoundary = new float[4];

    // Start在第一帧更新之前被调用
    void Start()
    {
        // 获取对象的Rigidbody2D和Collider2D组件
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();

        // 尝试将Collider2D转换为CircleCollider2D（注意：这要求对象实际上有一个CircleCollider2D组件）
        var circleCol = _collider as CircleCollider2D;
        if (circleCol != null && boundaryHolder != null && boundaryHolder.childCount >= 4)
        {
            // 设置玩家的边界，基于boundaryHolder的子对象位置和CircleCollider2D的半径
            _playerBoundary[0] = boundaryHolder.GetChild(0).position.x + circleCol.radius; // 右边界
            _playerBoundary[1] = boundaryHolder.GetChild(1).position.x - circleCol.radius; // 左边界
            _playerBoundary[2] = boundaryHolder.GetChild(2).position.y + circleCol.radius; // 上边界
            _playerBoundary[3] = boundaryHolder.GetChild(3).position.y - circleCol.radius; // 下边界
        }
        else
        {
            Debug.LogError("BoundaryHolder is not set up correctly or CircleCollider2D is missing.");
        }
    }

    // Update每帧被调用一次
    void Update()
    {
        // 鼠标按下事件
        if (Input.GetMouseButtonDown(0))
        {
            // 获取鼠标位置，并检查是否与Collider2D重叠
            var mousePos = GetMousePos();
            _canMove = _collider.OverlapPoint(mousePos);
        }

        // 拖拽事件
        if (Input.GetMouseButton(0))
        {
            // 如果可以移动，则更新对象的位置到鼠标位置
            if (_canMove)
            {
                var mousePos = GetMousePos();
                _rb.MovePosition(mousePos);
            }
        }

        // 辅助方法：将屏幕坐标转换为世界坐标，并限制在玩家边界内
        Vector2 GetMousePos()
        {
            // 将鼠标屏幕位置转换为世界坐标
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // 将鼠标位置限制在玩家边界内
            var clampedMousePos = new Vector2(
                Mathf.Clamp(mousePos.x, _playerBoundary[0], _playerBoundary[1]),
                Mathf.Clamp(mousePos.y, _playerBoundary[2], _playerBoundary[3])
            );

            return clampedMousePos;
        }
    }
}