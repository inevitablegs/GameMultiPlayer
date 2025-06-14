using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PhotonView))]
public class PlayerMovement : MonoBehaviourPunCallbacks
{
    public float speed = 5f;
    public float mouseSensitivity = 2f;
    public float jumpForce = 5f;
    public Transform cam; // Assign in Inspector!

    private Rigidbody rb;
    private float xRotation = 0f;
    private bool isGrounded;

    void Awake()
    {
        // Automatically get required components
        rb = GetComponent<Rigidbody>();
        
        // Safety checks
        if (cam == null)
        {
            Debug.LogError("Camera reference not set in PlayerMovement!");
            cam = GetComponentInChildren<Camera>()?.transform;
        }
    }

    void Start()
    {
        if (!photonView.IsMine)
        {
            // Disable components for remote players
            if (cam != null)
            {
                Camera camComponent = cam.GetComponent<Camera>();
                AudioListener audioListener = cam.GetComponent<AudioListener>();
                
                if (camComponent != null) camComponent.enabled = false;
                if (audioListener != null) audioListener.enabled = false;
            }
        }
        else
        {
            // Setup for local player
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            
            // Ensure we have a camera
            if (cam == null)
            {
                Debug.LogWarning("No camera assigned - creating one");
                GameObject cameraGO = new GameObject("PlayerCamera");
                cam = cameraGO.transform;
                cameraGO.AddComponent<Camera>();
                cameraGO.AddComponent<AudioListener>();
                cam.SetParent(transform);
                cam.localPosition = new Vector3(0, 0.7f, 0); // Typical FPS camera height
            }
        }
    }

    void Update()
    {
        if (!photonView.IsMine) return;

        HandleRotation();
        HandleJump();
    }

    void FixedUpdate()
    {
        if (!photonView.IsMine) return;

        HandleMovement();
    }

    void HandleRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        if (cam != null)
        {
            cam.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            transform.Rotate(Vector3.up * mouseX);
        }
    }

    void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        rb.MovePosition(rb.position + move * speed * Time.fixedDeltaTime);
    }

    void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void OnCollisionStay(Collision collision)
    {
        isGrounded = true;
    }

    void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(isGrounded);
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            // Network player, receive data
            this.isGrounded = (bool)stream.ReceiveNext();
            this.transform.position = (Vector3)stream.ReceiveNext();
            this.transform.rotation = (Quaternion)stream.ReceiveNext();
        }
    }
}