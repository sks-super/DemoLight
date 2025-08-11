using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal; // 需要Light2D命名空间
using UnityEngine.Rendering.Universal;

public class LightMove : MonoBehaviour
{
    [Header("移动控制")]
    public bool enableMovement = true; // 是否启用移动
    public Transform startPoint;       // 移动起点
    public Transform endPoint;         // 移动终点
    public float moveSpeed = 2f;       // 移动速度
    public bool pingPongMovement = true; // 是否来回移动

    [Header("旋转控制")]
    public bool enableRotation = true; // 是否启用旋转
    public float startAngle = 0f;      // 起始角度
    public float endAngle = 90f;       // 结束角度
    public float rotationSpeed = 60f; // 旋转速度(度/秒)
    public bool pingPongRotation = true; // 是否来回旋转

    private bool movingForward = true;   // 当前移动方向
    private bool rotatingForward = true;  // 当前旋转方向

    [Header("聚光灯设置")]
    public Light2D spotlight; // 2D聚光灯组件
    public int rayCount = 5;   // 发射的射线数量
    public float rayLength = 10f; // 射线最大长度
    public LayerMask detectionMask; // 检测的层级

    [Header("调试设置")]
    public bool drawRays = true; // 是否绘制射线
    public Color rayColor = Color.yellow; // 射线颜色

    private void Start()
    {

        if (enableMovement) transform.position = startPoint.position;

        spotlight=GetComponent<Light2D>();
    }
    void Update()
    {
        if (enableMovement)
        {
            HandleMovement();
        }

        if (enableRotation)
        {
            HandleRotation();
        }

        // 在聚光灯范围内发射探测射线
        CastSpotlightRays();
    }

    // 处理移动逻辑
    void HandleMovement()
    {
        // 确保起点和终点已设置
        if (startPoint == null || endPoint == null)
        {
            Debug.LogWarning("移动起点或终点未设置");
            return;
        }

        // 计算移动方向
        Vector3 targetPos = movingForward ? endPoint.position : startPoint.position;

        // 计算目标方向向量
        Vector3 direction = (targetPos - transform.position).normalized;

        // 移动物体
        transform.position += direction * moveSpeed * Time.deltaTime;

        // 检测是否到达目标点
        float distanceToTarget = Vector3.Distance(transform.position, targetPos);
        if (distanceToTarget < 0.1f)
        {
            // 处理是否来回移动
            if (pingPongMovement)
            {
                movingForward = !movingForward;
            }
            else
            {
                // 单向循环，返回起点
                transform.position = movingForward ? startPoint.position : endPoint.position;
            }
        }
    }

    // 处理旋转逻辑
    void HandleRotation()
    {
        // 计算目标角度
        float targetAngle = rotatingForward ? endAngle : startAngle;

        // 确定当前旋转方向
        float currentAngle = transform.eulerAngles.z;
        float angleDifference = targetAngle - currentAngle;

        // 处理角度差值，确保在[-180, 180]范围内
        if (angleDifference > 180f) angleDifference -= 360f;
        if (angleDifference < -180f) angleDifference += 360f;

        // 计算旋转步长
        float rotationStep = rotationSpeed * Time.deltaTime;

        // 限制旋转步长不超过实际角度差
        if (Mathf.Abs(rotationStep) > Mathf.Abs(angleDifference))
        {
            rotationStep = angleDifference;

            // 到达目标角度后处理方向变化
            if (pingPongRotation)
            {
                rotatingForward = !rotatingForward;
            }
        }
        else
        {
            // 应用旋转方向
            rotationStep *= Mathf.Sign(angleDifference);
        }

        // 应用旋转
        transform.Rotate(0, 0, rotationStep);
    }


    void CastSpotlightRays()
    {
        // 获取聚光灯参数
        float spotAngle = spotlight.pointLightOuterAngle;
        float spotRadius = spotlight.pointLightOuterRadius;

        // 使用实际聚光灯半径
        float actualRayLength = spotRadius;

        // 计算射线方向
        Vector2[] rayDirections = CalculateRayDirections(spotAngle);

        // 发射所有射线
        for (int i = 0; i < rayCount; i++)
        {
            Vector2 rayOrigin = spotlight.transform.position;
            Vector2 rayDir = rayDirections[i];

            // 发射射线
            RaycastHit2D hit = Physics2D.Raycast(
                rayOrigin,
                rayDir,
                actualRayLength,
                detectionMask
            );

            // 处理碰撞结果
            if (hit.collider != null)
            {
                HandleRayHit(hit, i);
            }

            // 调试绘制
            if (drawRays)
            {
                Debug.DrawRay(
                    rayOrigin,
                    rayDir * (hit.collider ? hit.distance : actualRayLength),
                    rayColor
                );
            }
        }
    }

    Vector2[] CalculateRayDirections(float spotAngle)
    {
        Vector2[] directions = new Vector2[rayCount];

        // 计算角度范围（从聚光灯中心向两侧扩展）
        float startAngle = -spotAngle / 2f;
        float angleStep = spotAngle / (rayCount - 1);

        // 获取聚光灯的当前方向
        Vector2 spotlightForward = spotlight.transform.up;

        for (int i = 0; i < rayCount; i++)
        {
            // 计算当前角度
            float currentAngle = startAngle + (angleStep * i);

            // 旋转方向向量
            Quaternion rotation = Quaternion.AngleAxis(currentAngle, Vector3.forward);
            directions[i] = rotation * spotlightForward;
        }

        return directions;
    }

    void HandleRayHit(RaycastHit2D hit, int rayIndex)
    {
        // 在这里处理射线碰撞结果
        //Debug.Log($"Ray {rayIndex} hit: {hit.collider.name} at distance {hit.distance}");

        // 根据碰撞物体类型处理
        if (hit.collider.CompareTag("Player"))
        {
            Debug.Log("Player detected in spotlight!");
            // 触发警报或其他逻辑
            //dead
            hit.transform.GetComponent<PlayerLife>().Die();
            //var p = hit.collider.gameObject.GetComponent<PlayerController>();
            //if (p != null)
            //{
            //    var ev = Schedule<PlayerEnteredLightZone>();
            //    ev.lightMove = this;
            //}
            //return

        }
        //else if (hit.collider.CompareTag("Enemy"))
        //{
        //    Debug.Log("Enemy detected in spotlight");
        //    // 触发战斗或其他逻辑
        //}
    }


}