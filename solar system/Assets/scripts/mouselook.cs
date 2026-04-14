using UnityEngine;

public class mouseLook : MonoBehaviour
{
    public float mouseSensitivity = 200f;
    public Transform playerBody;
    float xRotation = 0f;
    bool debuging;
    bool isPaused;
    [SerializeField] private menu menu;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }


    void Update()
    {
        if(menu.menuCanvas.alpha == 1)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else if (Input.GetKeyDown(KeyCode.V) && !debuging)
        {
            Cursor.lockState = CursorLockMode.None;
            debuging = true;
        }
        else if(Input.GetKeyDown(KeyCode.V) && debuging)
        {
            debuging = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else if (!debuging)
        {
            Cursor.lockState = CursorLockMode.Locked;
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            playerBody.Rotate(Vector3.up * mouseX);
        }  
    }
}

