using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Vector3 centerOfMass = new Vector3(0, -1, 0);
    public float moveSpeed = 30;
    public float maxSpeed = 25;
    public float rotateSpeed = 360;
    public GameObject virtualCamera;

    private Vector3 playerMoveDirection;
    private Rigidbody myRigidbody;
    private KeyCode previousInput = KeyCode.None;
    private float adSpeed = 0;

    [SerializeField] private float drag = 0.99f, xCoeff = 1, yCoeff = 10, yIntercept = 13;



    [SerializeField] private bool isJump = false;
    private Coroutine jumpCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = gameObject.GetComponent<Rigidbody>();
        myRigidbody.centerOfMass = centerOfMass;
    }

    void Update()
    {
        playerMoveInput();
        playerAcceleration();
        jumpCheck();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        move();
        rotate();
    }

    private void jumpCheck() {
        if (Physics.BoxCast(transform.position, (transform.lossyScale - new Vector3(0, 0.1f, 0)) / 2, -transform.up, transform.rotation, 0.1f)) {
            if (jumpCoroutine != null) {
                StopCoroutine(jumpCoroutine);
            }
            isJump = false;
            //myRigidbody.useGravity = false;
        }
        else {
            jumpCoroutine = StartCoroutine(jumpCheckCoroutine());
        }

    }

    IEnumerator jumpCheckCoroutine() {
        yield return new WaitForSeconds(0.1f);
        isJump = true;
        //myRigidbody.useGravity = true;
    }

    private void playerAcceleration() {
        if (Input.GetKeyDown(KeyCode.A) & previousInput != KeyCode.A) {
            previousInput = KeyCode.A;
            adSpeed += 1;
        }
        if (Input.GetKeyDown(KeyCode.D) & previousInput != KeyCode.D) {
            previousInput = KeyCode.D;
            adSpeed += 1;
        }

        adSpeed *= drag;
        moveSpeed = yIntercept + Mathf.Sqrt(xCoeff * adSpeed) * yCoeff;
    }

    private void playerMoveInput() {
        Vector3 cameraForward = virtualCamera.transform.forward;
        cameraForward -= new Vector3(0, cameraForward.y, 0);
        cameraForward = cameraForward.normalized;

        playerMoveDirection = Vector3.zero;
        if (Input.GetKey(KeyCode.UpArrow)) {
            playerMoveDirection += cameraForward;
        }
        if (Input.GetKey(KeyCode.DownArrow)) {
            playerMoveDirection -= cameraForward;
        }
        if (Input.GetKey(KeyCode.LeftArrow)) {
            playerMoveDirection -= Vector3.Cross(Vector3.up, cameraForward);
        }
        if (Input.GetKey(KeyCode.RightArrow)) {
            playerMoveDirection += Vector3.Cross(Vector3.up, cameraForward);
        }
    }

    private void move() {
        if (!isJump) {
            Vector3 groundNormal;
            if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, 1f)) groundNormal = hit.normal;
            else groundNormal = transform.up;

            Vector3 projected_move_dir = Vector3.ProjectOnPlane(playerMoveDirection, groundNormal);
            myRigidbody.AddForce(projected_move_dir.normalized * moveSpeed);
        }
        else {
            myRigidbody.AddForce(-Vector3.up * moveSpeed * moveSpeed / 40f);
        }

        if (myRigidbody.velocity.magnitude > maxSpeed) {
            myRigidbody.velocity = myRigidbody.velocity.normalized * maxSpeed;
        }
    }

    private void rotate() {
        if (playerMoveDirection.magnitude > 0 && !isJump) {
            Quaternion toRotation = Quaternion.LookRotation(playerMoveDirection);
            Quaternion playerRotation = Quaternion.Euler(0, myRigidbody.rotation.eulerAngles.y, 0);

            toRotation = Quaternion.RotateTowards(playerRotation, toRotation, rotateSpeed * Time.fixedDeltaTime);
            myRigidbody.rotation = Quaternion.Euler(myRigidbody.rotation.eulerAngles.x, toRotation.eulerAngles.y, myRigidbody.rotation.eulerAngles.z);
        }
    }
}
