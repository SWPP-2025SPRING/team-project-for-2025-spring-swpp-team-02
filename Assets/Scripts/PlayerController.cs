using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Vector3 center_of_mass;
    public float move_speed;
    public float rotate_speed;
    public GameObject _camera;

    private Vector3 player_move_dir;
    private Rigidbody my_rigidbody;
    private KeyCode previous_input = KeyCode.None;
    private float ad_speed = 0;

    [SerializeField] private float drag, x_coeff, y_coeff, y_intercept;

    // Start is called before the first frame update
    void Start()
    {
        my_rigidbody = gameObject.GetComponent<Rigidbody>();
        my_rigidbody.centerOfMass = center_of_mass;
    }

    void Update()
    {
        player_move_input();
        player_acceleration();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        move();
    }

    private void player_acceleration() {
        if (Input.GetKeyDown(KeyCode.A) & previous_input != KeyCode.A) {
            previous_input = KeyCode.A;
            ad_speed += 1;
        }
        if (Input.GetKeyDown(KeyCode.D) & previous_input != KeyCode.D) {
            previous_input = KeyCode.D;
            ad_speed += 1;
        }

        ad_speed *= drag;
        move_speed = y_intercept + Mathf.Sqrt(x_coeff * ad_speed) * y_coeff;
    }

    private void player_move_input() {
        Vector3 camera_forward = _camera.transform.forward;
        camera_forward -= new Vector3(0, camera_forward.y, 0);
        camera_forward = camera_forward.normalized;

        player_move_dir = Vector3.zero;
        if (Input.GetKey(KeyCode.UpArrow)) {
            player_move_dir += camera_forward;
        }
        if (Input.GetKey(KeyCode.DownArrow)) {
            player_move_dir -= camera_forward;
        }
        if (Input.GetKey(KeyCode.LeftArrow)) {
            player_move_dir -= Vector3.Cross(Vector3.up, camera_forward);
        }
        if (Input.GetKey(KeyCode.RightArrow)) {
            player_move_dir += Vector3.Cross(Vector3.up, camera_forward);
        }
    }

    private void move() {
        my_rigidbody.AddForce(player_move_dir.normalized * move_speed);

        if (player_move_dir.magnitude > 0) {
            Quaternion to_rotation = Quaternion.LookRotation(player_move_dir);
            Quaternion player_rotation = Quaternion.Euler(0, my_rigidbody.rotation.eulerAngles.y, 0);

            to_rotation = Quaternion.RotateTowards(player_rotation, to_rotation, rotate_speed * Time.fixedDeltaTime);

            my_rigidbody.rotation = Quaternion.Euler(my_rigidbody.rotation.eulerAngles.x, to_rotation.eulerAngles.y, my_rigidbody.rotation.eulerAngles.z);

        }
    }
}
