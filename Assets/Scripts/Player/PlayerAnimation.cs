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

    // Update is called once per frame
    void Update()
    {
        animator.speed = myRigidbody.velocity.magnitude * AnimationSpeed;
    }
}
