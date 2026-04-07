using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FirstPersonController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 5f;
    public float sprintSpeed = 8f;
    public float jumpForce = 5f;

    [Header("Look Settings")]
    public float mouseSensitivity = 2f;
    public Transform playerCamera;
    public float maxLookAngle = 89f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    private Rigidbody _rb;
    private float _xRotation = 0f;
    private bool _isGrounded;
    private Vector3 _moveInput;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.freezeRotation = true; // Oyuncunun fiziksel olarak devrilmesini engeller.

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleLook();
        CheckGrounded();
        HandleJump();
        
        // Girişleri Update içinde alıyoruz.
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        _moveInput = (transform.right * x + transform.forward * z).normalized;
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleLook()
    {
        if (playerCamera == null) return;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -maxLookAngle, maxLookAngle);

        playerCamera.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    private void HandleMovement()
    {
        float speed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed;
        
        // Mevcut y hızını (örneğin düşme hızını) koruyarak x ve z düzleminde hareket uyguluyoruz.
        Vector3 moveVelocity = _moveInput * speed;
        _rb.linearVelocity = new Vector3(moveVelocity.x, _rb.linearVelocity.y, moveVelocity.z);
    }

    private void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && _isGrounded)
        {
            // Zıplarken y hızını direkt atamak rigidbody tabanlı controllerlarda daha tutarlıdır.
            _rb.linearVelocity = new Vector3(_rb.linearVelocity.x, jumpForce, _rb.linearVelocity.z);
        }
    }

    private void CheckGrounded()
    {
        if (groundCheck != null)
        {
            _isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        }
        else
        {
            // Eğer groundCheck atanmamışsa alternatif olarak basit bir raycast.
            _isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f, groundMask);
        }
    }
}
