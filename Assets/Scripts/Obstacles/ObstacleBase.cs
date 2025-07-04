using UnityEngine;

public class ObstacleBase : MonoBehaviour
{
    [Header("Obstacle Settings")]
    [SerializeField] protected float moveSpeed = 2f;
    [SerializeField] protected bool once = false;

    protected bool isMoving = false;

    protected virtual void Start()
    {
        
    }

    protected virtual void FixedUpdate()
    {
        if (isMoving)
        {
            Move();
        }
    }

    public virtual void StartMove()
    {
        isMoving = true;
        gameObject.SetActive(true);
    }

    public virtual void StopMove()
    {
        isMoving = false;
    }

    protected virtual void Move()
    {

    }
}
