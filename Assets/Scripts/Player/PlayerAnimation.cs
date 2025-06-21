using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    Rigidbody myRigidbody;
    public Animator animator;
    public float AnimationSpeed = 0.1f;
    void Start()
    {
        myRigidbody = gameObject.GetComponent<Rigidbody>();
    }
    void Update()
    {
        animator.speed = myRigidbody.velocity.magnitude * AnimationSpeed;
    }
}
