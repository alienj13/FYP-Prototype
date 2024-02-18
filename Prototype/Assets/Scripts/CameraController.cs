using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float movementSpeed = 5.0f;
    public float rotationSpeed = 1.0f;
   

    void Start()
    {
        
    }

    void Update()
    {
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

        if (Input.GetMouseButtonDown(2)) 
        {

            Cursor.lockState = CursorLockMode.Locked;
        }

        if (Input.GetMouseButton(2))
        {

            float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
            float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;

            transform.eulerAngles += new Vector3(-mouseY, mouseX, 0);
        }

        if (Input.GetMouseButtonUp(2))
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
