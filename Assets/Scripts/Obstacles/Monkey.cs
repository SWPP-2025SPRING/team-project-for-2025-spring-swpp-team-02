using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monkey : ObstacleBase
{
    public Vector3 centerOfMass = new Vector3(0, -1, 0);
    public float flyForce = 70f;

    private bool hasLanded = false;
    private Rigidbody rb;
    private Transform playerTransform;

    private LineRenderer trackLine; // Ʈ���� �׸� ���� ������
    public float pointReachThreshold = 5f; // ���� �󸶳� ��������� ���� ������ �Ѿ����
    private int currentPointIndex = 0;
    private Vector3[] trackPoints;

    private bool isPlayingParticle = false;

    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.centerOfMass = centerOfMass;

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        trackLine = GameObject.FindGameObjectWithTag("MonkeyLine").GetComponent<LineRenderer>();
        //trackPoints = new Vector3[trackLine.positionCount];
        //trackLine.GetPositions(trackPoints);

        // Ʈ�� ����Ʈ �ε�
        int count = trackLine.positionCount;
        trackPoints = new Vector3[count];
        for (int i = 0; i < count; i++)
        {
            trackPoints[i] = trackLine.GetPosition(i);
        }

        // �����̿� ���� ����� ����Ʈ �ε��� ã��
        float minDist = float.MaxValue;
        int closestIndex = 0;
        for (int i = 0; i < trackPoints.Length; i++)
        {
            float dist = Vector3.Distance(transform.position, trackPoints[i]);
            if (dist < minDist)
            {
                minDist = dist;
                closestIndex = i;
            }
        }

        if (closestIndex < trackPoints.Length - 1)
        {
            Vector3 a = trackPoints[closestIndex];
            Vector3 b = trackPoints[closestIndex + 1];
            Vector3 toMonkey = transform.position - a;
            Vector3 segment = b - a;

            // ���� ���̷� �Ǵ�: �����̰� a-b ������ ���� ���̿� �ִ� ���
            float dot = Vector3.Dot(toMonkey, segment.normalized);

            if (dot > 0)
            {
                // ���� ��ġ�� a-b ���̿� ���� �� ���� ����Ʈ ����
                closestIndex += 1;
            }
        }
        currentPointIndex = closestIndex;

        gameObject.SetActive(false);
    }

    public override void StartMove()
    {
        base.StartMove();
        rb.useGravity = true;
    }

    protected override void Move()
    {
        // �÷��̾� �ݴ�������� ���� ����
        //if (hasLanded && playerTransform != null)
        //{
        //    float distance = Vector3.Distance(transform.position, playerTransform.position);
        //    if (distance > 20f)
        //    {
        //        Off();
        //    }
        //    else if (distance > 10f)
        //    {
        //        return;
        //    }


        //    Vector3 awayFromPlayer = (transform.position - playerTransform.position).normalized;

        //    if (Physics.Raycast(transform.position + Vector3.up * 0.5f, Vector3.down, out RaycastHit hit, 2f))
        //    {
        //        Vector3 slideDir = Vector3.ProjectOnPlane(awayFromPlayer, hit.normal).normalized;
        //        Vector3 targetPosition = rb.position + slideDir * forwardSpeed * Time.fixedDeltaTime;
        //        rb.MovePosition(targetPosition);
        //    }
        //}

        if (!hasLanded || trackPoints == null || trackPoints.Length == 0 || currentPointIndex >= trackPoints.Length)
        {
            return;
        }
            

        Vector3 targetPoint = trackPoints[currentPointIndex];
        Vector3 toTarget = targetPoint - transform.position;
        Vector2 pointVec2 = new Vector2(targetPoint.x, targetPoint.z);
        Vector2 monkeyVec2 = new Vector2(transform.position.x, transform.position.z);
        Vector2 playerVec2 = new Vector2(playerTransform.position.x, playerTransform.position.z);
        float distanceToTarget = (pointVec2 - monkeyVec2).magnitude;

        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        float distanceToTargetPlayer = (pointVec2 - playerVec2).magnitude;

        if (distanceToPlayer > 100f || distanceToTargetPlayer < distanceToTarget - 5f)
        {
            // �÷��̾���� �Ÿ��� �� ��� or �÷��̾� �ڿ� ���� ��� ��Ȱ��ȭ
            Debug.Log("monkey off");
            Off();
            return;
        }

        if (distanceToTarget < pointReachThreshold)
        {
            if (currentPointIndex < trackPoints.Length - 1)
            {
                currentPointIndex++;
                targetPoint = trackPoints[currentPointIndex];
                toTarget = targetPoint - transform.position;
                //Debug.Log(targetPoint);
            }
            else
            {
                // ������ ����Ʈ ���� �� ���� �Ǵ� ��Ȱ��ȭ
                Off();
                return;
            }
        }

        Vector3 slideDir = toTarget.normalized;

        if (Physics.Raycast(transform.position + Vector3.up * 0.5f, Vector3.down, out RaycastHit hit, 2f))
        {
            slideDir = Vector3.ProjectOnPlane(toTarget, hit.normal).normalized;
        }

        Vector3 targetPosition = rb.position + slideDir * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(targetPosition);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!hasLanded)
        {
            hasLanded = true;
        }
        else if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Obstacle"))
        {
            Vector3 hitDirection = (transform.position - collision.transform.position).normalized;
            rb.AddForce((hitDirection + Vector3.up) * flyForce, ForceMode.Impulse);

            if (!isPlayingParticle)
            {
                ParticleSystem particle = ParticleManager.instance.GetParticle("MonkeyCollision");
                particle.transform.position = collision.contacts[0].point + new Vector3(0, 0.3f, 0);
                particle.transform.rotation = Quaternion.LookRotation(collision.contacts[0].normal);
                particle.Play();
                isPlayingParticle = true;
            }

            Invoke(nameof(Off), 1f);
        }
    }

    private void Off()
    {
        gameObject.SetActive(false);
    }

}
