using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Vector3 centerOfMass = new Vector3(0, -1, 0);
    public GameObject virtualCamera;

    [Header("Move")]
    public float moveSpeed = 30;
    public float maxSpeed = 25;
    public float maxUpSpeed = 23, maxDownSpeed = 27;
    public float rotateSpeed = 360;
    private Vector3 playerMoveDirection;
    private Rigidbody myRigidbody;
    private KeyCode previousInput = KeyCode.None;
    private float adSpeed = 0;

    [Header("AD")]

    [SerializeField] private float drag = 0.99f;
    [SerializeField] private float xCoeff = 1;
    [SerializeField] private float yCoeff = 10;
    [SerializeField] private float yIntercept = 13;

    private bool isParticleDelay = false;

    [Header("jump")]
    [SerializeField] private bool isJump = false;
    private Coroutine jumpCoroutine;
    [SerializeField] private LayerMask groundLayer;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = gameObject.GetComponent<Rigidbody>();
        myRigidbody.centerOfMass = centerOfMass;
    }

    void Update()
    {
        playerMoveInput();
        PlayerAcceleration();
        JumpCheck();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
        Rotate();
    }

    private void JumpCheck()
    {
        if (Physics.BoxCast(transform.position, (transform.lossyScale - new Vector3(0, 0.2f, 0)) / 2, -transform.up, transform.rotation, 0.2f))
        {
            if (jumpCoroutine != null)
            {
                StopCoroutine(jumpCoroutine);
            }
            isJump = false;
        }
        else
        {
            jumpCoroutine = StartCoroutine(JumpCheckCoroutine());
        }

    }

    IEnumerator JumpCheckCoroutine()
    {
        yield return new WaitForSeconds(0.1f);
        jumpCoroutine = null;
        isJump = true;
    }

    private void PlayerAcceleration()
    {
        if (Input.GetKeyDown(KeyCode.A) & previousInput != KeyCode.A)
        {
            previousInput = KeyCode.A;
            adSpeed += 1;
        }
        if (Input.GetKeyDown(KeyCode.D) & previousInput != KeyCode.D)
        {
            previousInput = KeyCode.D;
            adSpeed += 1;
        }

        adSpeed *= drag;
        moveSpeed = yIntercept + Mathf.Sqrt(xCoeff * adSpeed) * yCoeff;
    }

    private void playerMoveInput()
    {
        // 카메라 방향 확인
        Vector3 cameraForward = virtualCamera.transform.forward;
        cameraForward -= new Vector3(0, cameraForward.y, 0);
        cameraForward = cameraForward.normalized;

        playerMoveDirection = Vector3.zero;
        if (Input.GetKey(KeyCode.UpArrow))
        {
            playerMoveDirection += cameraForward;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            playerMoveDirection -= cameraForward;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            playerMoveDirection -= Vector3.Cross(Vector3.up, cameraForward);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            playerMoveDirection += Vector3.Cross(Vector3.up, cameraForward);
        }
    }
    
    IEnumerator MakeMoveParticle()
    {
        isParticleDelay = true;
        ParticleSystem moveParticle = ParticleManager.instance.GetParticle("PlayerMove");
        moveParticle.transform.position = transform.position - transform.forward * 0.2f - transform.up * 0.1f;

        yield return new WaitForSeconds(0.05f);
        isParticleDelay = false;
    }
    

    // 오르막인지 내리막인지에 따라 최대 속도 변화
    private void CheckGound()
    {
        if (Vector3.Dot(transform.forward, Vector3.up) > 0.05) // 오르막을 오르는 경우
        { 
            maxSpeed = maxUpSpeed;
        }
        else if (Vector3.Dot(transform.forward, Vector3.up) < -0.05) // 내리막을 내려가는 경우
        { 
            maxSpeed = maxDownSpeed;
        }
        else // 평지에서 움직이는 경우
        { 
            maxSpeed = 20;
        }
    }

    private void Move()
    {
        if (!isJump)
        {
            Vector3 groundNormal; // 현재 위치의 바닥 normal 확인
            if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, 1f, groundLayer))
            {
                groundNormal = hit.normal;
            }
            else
            {
                groundNormal = transform.up;
            }

            Vector3 projected_move_dir = Vector3.ProjectOnPlane(playerMoveDirection, groundNormal);
            myRigidbody.AddForce(projected_move_dir.normalized * moveSpeed);

            CheckGound();

            // 최대 속도 적용
            if (myRigidbody.velocity.magnitude > maxSpeed)
            { 
                myRigidbody.velocity += (myRigidbody.velocity.normalized * maxSpeed - myRigidbody.velocity) * 0.1f;
            }

            
            if (!isParticleDelay && myRigidbody.velocity.magnitude > 5) // 움직일 때 생기는 파티클 (현재 임시 제거)
            { 
                StartCoroutine(MakeMoveParticle());
            }
            
        }
        else
        {
            // 점프를 뛴 상태에서는 바닥에 빨리 떨어지도록 아래 방향 힘이 작용
            myRigidbody.AddForce(-Vector3.up * moveSpeed * moveSpeed / 40f);
        }
    }

    // 조작하는 방향으로 회전
    private void Rotate()
    {
        if (playerMoveDirection.magnitude > 0 && !isJump)
        {
            Quaternion toRotation = Quaternion.LookRotation(playerMoveDirection);
            Quaternion playerRotation = Quaternion.Euler(0, myRigidbody.rotation.eulerAngles.y, 0);

            toRotation = Quaternion.RotateTowards(playerRotation, toRotation, rotateSpeed * Time.fixedDeltaTime);
            myRigidbody.rotation = Quaternion.Euler(myRigidbody.rotation.eulerAngles.x, toRotation.eulerAngles.y, myRigidbody.rotation.eulerAngles.z);
        }
    }
}
