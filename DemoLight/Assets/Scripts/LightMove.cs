using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal; // ��ҪLight2D�����ռ�
using UnityEngine.Rendering.Universal;

public class LightMove : MonoBehaviour
{
    [Header("�ƶ�����")]
    public bool enableMovement = true; // �Ƿ������ƶ�
    public Transform startPoint;       // �ƶ����
    public Transform endPoint;         // �ƶ��յ�
    public float moveSpeed = 2f;       // �ƶ��ٶ�
    public bool pingPongMovement = true; // �Ƿ������ƶ�

    [Header("��ת����")]
    public bool enableRotation = true; // �Ƿ�������ת
    public float startAngle = 0f;      // ��ʼ�Ƕ�
    public float endAngle = 90f;       // �����Ƕ�
    public float rotationSpeed = 60f; // ��ת�ٶ�(��/��)
    public bool pingPongRotation = true; // �Ƿ�������ת

    private bool movingForward = true;   // ��ǰ�ƶ�����
    private bool rotatingForward = true;  // ��ǰ��ת����

    [Header("�۹������")]
    public Light2D spotlight; // 2D�۹�����
    public int rayCount = 5;   // �������������
    public float rayLength = 10f; // ������󳤶�
    public LayerMask detectionMask; // ���Ĳ㼶

    [Header("��������")]
    public bool drawRays = true; // �Ƿ��������
    public Color rayColor = Color.yellow; // ������ɫ

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

        // �ھ۹�Ʒ�Χ�ڷ���̽������
        CastSpotlightRays();
    }

    // �����ƶ��߼�
    void HandleMovement()
    {
        // ȷ�������յ�������
        if (startPoint == null || endPoint == null)
        {
            Debug.LogWarning("�ƶ������յ�δ����");
            return;
        }

        // �����ƶ�����
        Vector3 targetPos = movingForward ? endPoint.position : startPoint.position;

        // ����Ŀ�귽������
        Vector3 direction = (targetPos - transform.position).normalized;

        // �ƶ�����
        transform.position += direction * moveSpeed * Time.deltaTime;

        // ����Ƿ񵽴�Ŀ���
        float distanceToTarget = Vector3.Distance(transform.position, targetPos);
        if (distanceToTarget < 0.1f)
        {
            // �����Ƿ������ƶ�
            if (pingPongMovement)
            {
                movingForward = !movingForward;
            }
            else
            {
                // ����ѭ�����������
                transform.position = movingForward ? startPoint.position : endPoint.position;
            }
        }
    }

    // ������ת�߼�
    void HandleRotation()
    {
        // ����Ŀ��Ƕ�
        float targetAngle = rotatingForward ? endAngle : startAngle;

        // ȷ����ǰ��ת����
        float currentAngle = transform.eulerAngles.z;
        float angleDifference = targetAngle - currentAngle;

        // ����ǶȲ�ֵ��ȷ����[-180, 180]��Χ��
        if (angleDifference > 180f) angleDifference -= 360f;
        if (angleDifference < -180f) angleDifference += 360f;

        // ������ת����
        float rotationStep = rotationSpeed * Time.deltaTime;

        // ������ת����������ʵ�ʽǶȲ�
        if (Mathf.Abs(rotationStep) > Mathf.Abs(angleDifference))
        {
            rotationStep = angleDifference;

            // ����Ŀ��ǶȺ�����仯
            if (pingPongRotation)
            {
                rotatingForward = !rotatingForward;
            }
        }
        else
        {
            // Ӧ����ת����
            rotationStep *= Mathf.Sign(angleDifference);
        }

        // Ӧ����ת
        transform.Rotate(0, 0, rotationStep);
    }


    void CastSpotlightRays()
    {
        // ��ȡ�۹�Ʋ���
        float spotAngle = spotlight.pointLightOuterAngle;
        float spotRadius = spotlight.pointLightOuterRadius;

        // ʹ��ʵ�ʾ۹�ư뾶
        float actualRayLength = spotRadius;

        // �������߷���
        Vector2[] rayDirections = CalculateRayDirections(spotAngle);

        // ������������
        for (int i = 0; i < rayCount; i++)
        {
            Vector2 rayOrigin = spotlight.transform.position;
            Vector2 rayDir = rayDirections[i];

            // ��������
            RaycastHit2D hit = Physics2D.Raycast(
                rayOrigin,
                rayDir,
                actualRayLength,
                detectionMask
            );

            // ������ײ���
            if (hit.collider != null)
            {
                HandleRayHit(hit, i);
            }

            // ���Ի���
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

        // ����Ƕȷ�Χ���Ӿ۹��������������չ��
        float startAngle = -spotAngle / 2f;
        float angleStep = spotAngle / (rayCount - 1);

        // ��ȡ�۹�Ƶĵ�ǰ����
        Vector2 spotlightForward = spotlight.transform.up;

        for (int i = 0; i < rayCount; i++)
        {
            // ���㵱ǰ�Ƕ�
            float currentAngle = startAngle + (angleStep * i);

            // ��ת��������
            Quaternion rotation = Quaternion.AngleAxis(currentAngle, Vector3.forward);
            directions[i] = rotation * spotlightForward;
        }

        return directions;
    }

    void HandleRayHit(RaycastHit2D hit, int rayIndex)
    {
        // �����ﴦ��������ײ���
        //Debug.Log($"Ray {rayIndex} hit: {hit.collider.name} at distance {hit.distance}");

        // ������ײ�������ʹ���
        if (hit.collider.CompareTag("Player"))
        {
            Debug.Log("Player detected in spotlight!");
            // ���������������߼�
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
        //    // ����ս���������߼�
        //}
    }


}