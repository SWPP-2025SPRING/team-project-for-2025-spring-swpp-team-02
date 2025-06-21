using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Vector3 centerOfMass = new Vector3(0, -1, 0);
    public GameObject virtualCamera;

    [Header("Move")]
    public float moveSpeed = 20;
    public float maxSpeed = 25;
    public float maxUpSpeed = 23, maxDownSpeed = 27;
    public float rotateSpeed = 360;
    private Vector3 playerMoveDirection;
    private Rigidbody myRigidbody;

    [Header("jump")]
    public bool isJump = false;
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
        PlayerMoveInput();
        PlayerInput();
        JumpCheck();
    }

    void FixedUpdate()
    {
        if (GameManager.instance.isRun)
        {
            Move();
            Rotate();
        }
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

    private void PlayerMoveInput()
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

    private void PlayerInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TransitionEffect.instance.MoveScene("MenuScene");
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.position = virtualCamera.transform.position - Vector3.down * 0.5f;
            myRigidbody.velocity = Vector3.zero;
        }
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

    private Vector3 FindGroundNormal()
    {
        if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, 1f, groundLayer))
        {
            return hit.normal;
        }
        else
        {
            return transform.up;
        }
            
    }

    private void Move()
    {
        if (!isJump)
        {
            Vector3 groundNormal = FindGroundNormal(); // 현재 위치의 바닥 normal 확인
            Vector3 projected_move_dir = Vector3.ProjectOnPlane(playerMoveDirection, groundNormal);
            myRigidbody.AddForce(projected_move_dir.normalized * moveSpeed);

            CheckGound();

            // 최대 속도 적용
            if (myRigidbody.velocity.magnitude > maxSpeed)
            {
                myRigidbody.velocity += (myRigidbody.velocity.normalized * maxSpeed - myRigidbody.velocity) * 0.1f;
            }


            if (myRigidbody.velocity.magnitude > 5) // 움직일 때 생기는 파티클
            {
                ParticleManager.instance.PlayWithDelay("PlayerMove",
                                                       transform.position - transform.forward * 0.2f - transform.up * 0.1f,
                                                       Quaternion.identity,
                                                       0.05f);
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
