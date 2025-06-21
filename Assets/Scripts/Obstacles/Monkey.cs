using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monkey : ObstacleBase
{
    public Vector3 centerOfMass = new Vector3(0, -1, 0);
    public float flyForce = 70f;

    private bool hasLanded = false;
    private Rigidbody myRigidbody;
    private Transform playerTransform;

    public LineRenderer trackLine;
    public float pointReachThreshold = 5f;
    private int currentPointIndex = 0;
    private Vector3[] trackPoints;

    private bool isPlayingParticle = false;

    protected override void Start()
    {
        base.Start();
        Init();
        FindClosestIndex();
        gameObject.SetActive(false);
    }

    private void FindClosestIndex()
    {
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

            float dot = Vector3.Dot(toMonkey, segment.normalized);

            if (dot > 0)
            {
                closestIndex += 1;
            }
        }
        currentPointIndex = closestIndex;
    }

    private void Init()
    {
        myRigidbody = GetComponent<Rigidbody>();
        myRigidbody.useGravity = false;
        myRigidbody.centerOfMass = centerOfMass;

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        int count = trackLine.positionCount;
        trackPoints = new Vector3[count];
        for (int i = 0; i < count; i++)
        {
            trackPoints[i] = trackLine.GetPosition(i);
        }
    }

    public override void StartMove()
    {
        base.StartMove();
        myRigidbody.useGravity = true;
    }

    private Vector3 FindToTargetVector()
    {
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
            Debug.Log("monkey off");
            Off();
            return Vector3.zero;
        }

        if (distanceToTarget < pointReachThreshold)
        {
            if (currentPointIndex < trackPoints.Length - 1)
            {
                currentPointIndex++;
                targetPoint = trackPoints[currentPointIndex];
                toTarget = targetPoint - transform.position;
            }
            else
            {
                Off();
                return Vector3.zero;
            }
        }

        return toTarget;
    }

    protected override void Move()
    {
        if (!hasLanded || trackPoints == null || trackPoints.Length == 0 || currentPointIndex >= trackPoints.Length)
        {
            return;
        }

        Vector3 toTarget = FindToTargetVector();
        Vector3 slideDir = toTarget.normalized;

        if (Physics.Raycast(transform.position + Vector3.up * 0.5f, Vector3.down, out RaycastHit hit, 2f))
        {
            slideDir = Vector3.ProjectOnPlane(toTarget, hit.normal).normalized;
        }

        Vector3 targetPosition = myRigidbody.position + slideDir * moveSpeed * Time.fixedDeltaTime;
        myRigidbody.MovePosition(targetPosition);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!hasLanded)
        {
            hasLanded = true;
        }

        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Obstacle"))
        {
            Vector3 hitDirection = (transform.position - collision.transform.position).normalized;
            myRigidbody.AddForce((hitDirection + Vector3.up) * flyForce / 10000, ForceMode.Impulse);

            if (!isPlayingParticle)
            {
                ParticleManager.instance.Play("MonkeyCollision",
                                              collision.contacts[0].point + new Vector3(0, 0.3f, 0),
                                              Quaternion.LookRotation(collision.contacts[0].normal));
                
                isPlayingParticle = true;
            }

            Invoke(nameof(Off), 1f);
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            SoundManager.instance.PlayAudioWithDelay("Effect", "MonkeyCollisionSound", 0.05f);
        }
    }

    private void Off()
    {
        gameObject.SetActive(false);
    }
}
