using UnityEngine;
using System.Collections;

/// <summary>
/// A high-fidelity First Person Controller designed for technical portfolios.
/// Includes smooth acceleration, head bobbing, and dynamic FOV.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class FirstPersonController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float _walkSpeed = 5f;
    [SerializeField] private float _sprintSpeed = 8f;
    [SerializeField] private float _acceleration = 10f;
    [SerializeField] private float _jumpForce = 6f;
    [SerializeField] private float _airControl = 0.5f;

    [Header("Look Settings")]
    [SerializeField] private float _mouseSensitivity = 1.5f;
    [SerializeField] private float _smoothLookTime = 0.05f;
    [SerializeField] private Transform _playerCamera;
    [SerializeField] private float _maxLookAngle = 89f;

    [Header("Head Bob Settings")]
    [SerializeField] private bool _useHeadBob = true;
    [SerializeField] private float _bobFrequency = 1.5f;
    [SerializeField] private float _bobHorizontalAmplitude = 0.05f;
    [SerializeField] private float _bobVerticalAmplitude = 0.05f;
    [SerializeField] private Transform _cameraHolder; // Child of camera for bobbing

    [Header("Ground Check")]
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private float _groundDistance = 0.3f;
    [SerializeField] private LayerMask _groundMask;

    // Internal State
    private Rigidbody _rb;
    private float _xRotation = 0f;
    private bool _isGrounded;
    private Vector3 _currentVelocity;
    private float _bobTimer;
    private float _defaultFov;
    private Camera _cam;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.freezeRotation = true;
        _rb.interpolation = RigidbodyInterpolation.Interpolate;

        _cam = _playerCamera.GetComponent<Camera>();
        _defaultFov = _cam.fieldOfView;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleLook();
        CheckGrounded();
        HandleJump();
        HandleHeadBob();
        HandleFov();
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleLook()
    {
        if (_playerCamera == null) return;

        float mouseX = Input.GetAxisRaw("Mouse X") * _mouseSensitivity;
        float mouseY = Input.GetAxisRaw("Mouse Y") * _mouseSensitivity;

        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -_maxLookAngle, _maxLookAngle);

        _playerCamera.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    private void HandleMovement()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        
        Vector3 targetDir = (transform.right * x + transform.forward * z).normalized;
        float speed = Input.GetKey(KeyCode.LeftShift) ? _sprintSpeed : _walkSpeed;

        if (!_isGrounded) speed *= _airControl;

        Vector3 targetVelocity = targetDir * speed;
        targetVelocity.y = _rb.linearVelocity.y;

        // Smooth acceleration
        _rb.linearVelocity = Vector3.Lerp(_rb.linearVelocity, targetVelocity, _acceleration * Time.fixedDeltaTime);
    }

    private void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && _isGrounded)
        {
            _rb.linearVelocity = new Vector3(_rb.linearVelocity.x, _jumpForce, _rb.linearVelocity.z);
        }
    }

    private void CheckGrounded()
    {
        _isGrounded = Physics.CheckSphere(_groundCheck.position, _groundDistance, _groundMask);
    }

    private void HandleHeadBob()
    {
        if (!_useHeadBob || !_isGrounded || _cameraHolder == null) return;

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        if (Mathf.Abs(horizontalInput) > 0.1f || Mathf.Abs(verticalInput) > 0.1f)
        {
            _bobTimer += Time.deltaTime * (_rb.linearVelocity.magnitude * _bobFrequency);
            
            float hBob = Mathf.Sin(_bobTimer) * _bobHorizontalAmplitude;
            float vBob = Mathf.Cos(_bobTimer * 2) * _bobVerticalAmplitude;

            _cameraHolder.localPosition = new Vector3(hBob, vBob, 0);
        }
        else
        {
            _bobTimer = 0;
            _cameraHolder.localPosition = Vector3.Lerp(_cameraHolder.localPosition, Vector3.zero, Time.deltaTime * 5f);
        }
    }

    private void HandleFov()
    {
        float targetFov = (Input.GetKey(KeyCode.LeftShift) && _rb.linearVelocity.magnitude > 1f) ? _defaultFov + 10f : _defaultFov;
        _cam.fieldOfView = Mathf.Lerp(_cam.fieldOfView, targetFov, Time.deltaTime * 8f);
    }
}
