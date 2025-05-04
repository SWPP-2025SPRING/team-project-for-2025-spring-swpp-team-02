using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMapPlayerController : MonoBehaviour
{
    public float speed = 5.0f;
    public Transform cameraTransform;
    public LineRenderer trackLine;
    public float smoothSpeed = 5f;
    public float cameraOffsetDistance = 10f;

    private Vector3 player_move_dir;

    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // 카메라 기준 forward, right
        Vector3 cameraForward = cameraTransform.forward;
        cameraForward.y = 0;
        cameraForward.Normalize();

        Vector3 cameraRight = cameraTransform.right;
        cameraRight.y = 0;
        cameraRight.Normalize();

        Vector3 movement = cameraForward * moveVertical + cameraRight * moveHorizontal;
        transform.Translate(movement * speed * Time.deltaTime, Space.World);

        FollowTrackLine();
    }

    private void FollowTrackLine()
    {
        if (trackLine == null || trackLine.positionCount == 0) return;

        Vector3 nearestPoint = trackLine.GetPosition(0);
        int nearestIndex = 0;
        float minDistance = Vector3.Distance(transform.position, nearestPoint);

        // 가장 가까운 포인트 찾기
        for (int i = 1; i < trackLine.positionCount; i++)
        {
            Vector3 point = trackLine.GetPosition(i);
            float distance = Vector3.Distance(transform.position, point);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestPoint = point;
                nearestIndex = i;
            }
        }

        // 진행 방향 계산 (다음 점으로 향하는 벡터)
        Vector3 nextPoint = (nearestIndex < trackLine.positionCount - 1) ? trackLine.GetPosition(nearestIndex + 1) : nearestPoint;
        Vector3 direction = (nextPoint - nearestPoint).normalized;

        // 진행 방향에서 offset만큼 뒤로 카메라 위치 계산
        Vector3 targetPosition = nearestPoint - direction * cameraOffsetDistance;

        // y 높이 유지
        targetPosition.y = cameraTransform.position.y;

        // 부드럽게 이동
        cameraTransform.position = Vector3.Lerp(cameraTransform.position, targetPosition, smoothSpeed * Time.deltaTime);

        // 플레이어 바라보기
        cameraTransform.LookAt(transform.position);
    }


    private Vector3 GetClosestPointOnLine(LineRenderer line, Vector3 point)
    {
        Vector3 closestPoint = line.GetPosition(0);
        float minDist = Vector3.Distance(point, closestPoint);

        for (int i = 1; i < line.positionCount - 1; i++)
        {
            Vector3 segmentStart = line.GetPosition(i);
            Vector3 segmentEnd = line.GetPosition(i + 1);
            Vector3 projectedPoint = ProjectPointOnLineSegment(segmentStart, segmentEnd, point);
            float dist = Vector3.Distance(point, projectedPoint);
            if (dist < minDist)
            {
                minDist = dist;
                closestPoint = projectedPoint;
            }
        }

        return closestPoint;
    }

    private Vector3 ProjectPointOnLineSegment(Vector3 a, Vector3 b, Vector3 p)
    {
        Vector3 ap = p - a;
        Vector3 ab = b - a;
        float magnitudeAB = ab.sqrMagnitude;
        float abapProduct = Vector3.Dot(ap, ab);
        float distance = abapProduct / magnitudeAB;

        if (distance < 0) return a;
        else if (distance > 1) return b;
        else return a + ab * distance;
    }
}
