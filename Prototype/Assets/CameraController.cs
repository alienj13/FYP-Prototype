using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float movementSpeed = 5.0f;
    public float mouseSensitivity = 100.0f;
    public float clampAngle = 85.0f;

    private float verticalRotation = 0.0f;
    private float horizontalRotation = 0.0f;

    void Start()
    {
        Vector3 rot = transform.localRotation.eulerAngles;
        horizontalRotation = rot.y;
        verticalRotation = rot.x;
    }

    void Update()
    {
        // WASD movement
        Vector3 movement = new Vector3();
        if (Input.GetKey(KeyCode.W))
            movement += Vector3.forward;
        if (Input.GetKey(KeyCode.S))
            movement += Vector3.back;
        if (Input.GetKey(KeyCode.A))
            movement += Vector3.left;
        if (Input.GetKey(KeyCode.D))
            movement += Vector3.right;

        transform.Translate(movement * movementSpeed * Time.deltaTime, Space.Self);

        // Mouse rotation
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = -Input.GetAxis("Mouse Y");

        horizontalRotation += mouseX * mouseSensitivity * Time.deltaTime;
        verticalRotation += mouseY * mouseSensitivity * Time.deltaTime;

        verticalRotation = Mathf.Clamp(verticalRotation, -clampAngle, clampAngle);

        Quaternion localRotation = Quaternion.Euler(verticalRotation, horizontalRotation, 0.0f);
        transform.rotation = localRotation;
    }
}
