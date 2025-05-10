using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallObstacle : ObstacleBase
{
    [SerializeField] private float moveRange = 3f;
    [SerializeField] private Vector3 moveDirection = Vector3.forward;

    private Vector3 startPosition;
    private Vector3 targetPosition;
    private float moveOffset;

    protected override void Start()
    {
        base.Start();
        startPosition = transform.position;
        targetPosition = startPosition + moveDirection.normalized * moveRange;
    }

    protected override void Move()
    {
        moveOffset += Time.deltaTime * moveSpeed;
        if (once)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
        else
        {
            float offset = Mathf.PingPong(moveOffset, moveRange);
            transform.position = startPosition + moveDirection.normalized * offset;
        }
        
    }
}
