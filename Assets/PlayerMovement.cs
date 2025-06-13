using UnityEngine;
using Photon.Pun;

public class PlayerMovement : MonoBehaviourPunCallbacks
{
    public float speed = 5f;
    public float mouseSensitivity = 2f;
    public float jumpForce = 5f;
    public Transform cam;

    private Rigidbody rb;
    private float xRotation = 0f;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // Cursor.lockState = CursorLockMode.Locked;
        if (!photonView.IsMine)
        {
            
            GetComponentInChildren<Camera>().enabled = false;
            GetComponentInChildren<AudioListener>().enabled = false;
            // Unlock the cursor for other players
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            // Mouse Look
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
    
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
    
            cam.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            transform.Rotate(Vector3.up * mouseX);
    
            // Jump
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }
    }

    void FixedUpdate()
    {
        // Movement
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        rb.MovePosition(rb.position + move * speed * Time.fixedDeltaTime);
    }

    // Ground Check
    void OnCollisionStay(Collision collision)
    {
        isGrounded = true;
        Debug.Log("Grounded: " + isGrounded);
    }

    void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
        Debug.Log("Grounded: " + isGrounded);
    }
}
