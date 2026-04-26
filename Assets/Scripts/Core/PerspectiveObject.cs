using UnityEngine;

/// <summary>
/// Core logic for an object that can be manipulated via perspective.
/// Combined with 'Paradox Effect' shader support and smooth interpolation.
/// </summary>
public class PerspectiveObject : MonoBehaviour
{
    [Header("Visual Settings")]
    [SerializeField] private float _smoothingSpeed = 15f;
    [SerializeField] private Material _paradoxMaterial;
    
    // Physics & Rendering
    private Rigidbody _rb;
    private MeshRenderer _renderer;
    private Material _originalMaterial;

    // Perspective State
    private float _initialDistance;
    private Vector3 _initialScale;
    private Vector3 _targetScale;
    private bool _isPickedUp;

    public bool IsPickedUp => _isPickedUp;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _renderer = GetComponent<MeshRenderer>();
        
        if (_renderer != null)
        {
            _originalMaterial = _renderer.material;
        }

        _targetScale = transform.localScale;
    }

    void Update()
    {
        if (_isPickedUp)
        {
            // Smoothly interpolate to target scale
            transform.localScale = Vector3.Lerp(transform.localScale, _targetScale, Time.deltaTime * _smoothingSpeed);
        }
    }

    /// <summary>
    /// Records initial state when picked up.
    /// Toggles the Paradox Shader for visual feedback.
    /// </summary>
    public void OnPickup(float distance)
    {
        _isPickedUp = true;
        _initialDistance = distance;
        _initialScale = transform.localScale;
        _targetScale = _initialScale;

        // Apply visual effect
        if (_renderer != null && _paradoxMaterial != null)
        {
            _renderer.material = _paradoxMaterial;
        }
        
        if (_rb != null)
        {
            _rb.isKinematic = true;
            _rb.useGravity = false;
        }
    }

    /// <summary>
    /// Updates target scale based on new distance to maintain perceived size.
    /// </summary>
    public void UpdatePerspectiveScale(float currentDistance)
    {
        if (!_isPickedUp) return;

        // The Paradox Formula: Scale = Distance Ratio
        float scaleMultiplier = currentDistance / _initialDistance;
        _targetScale = _initialScale * scaleMultiplier;
    }

    /// <summary>
    /// Finalizes placement, restores visuals, and re-aligns gravity.
    /// Now accepts initial velocity for momentum transfer.
    /// </summary>
    public void OnRelease(Vector3 newGravityDirection, Vector3 initialVelocity)
    {
        _isPickedUp = false;
        transform.localScale = _targetScale; 

        if (_renderer != null && _originalMaterial != null)
        {
            _renderer.material = _originalMaterial;
        }

        if (_rb != null)
        {
            _rb.isKinematic = false;
            
            // APPLY MOMENTUM
            _rb.linearVelocity = initialVelocity;

            GravityBody gBody = GetComponent<GravityBody>();
            if (gBody != null)
            {
                gBody.GravityDirection = newGravityDirection;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Only trigger impact feedback if we were dropped or moving fast
        if (_rb != null && _rb.linearVelocity.magnitude > 2.0f)
        {
            PlayImpactEffect(collision);
        }
    }

    private void PlayImpactEffect(Collision collision)
    {
        // Professional portfolios should have hooks for VFX/SFX
        Debug.Log($"[Impact] {gameObject.name} hit {collision.gameObject.name} with velocity {_rb.linearVelocity.magnitude}");
        
        // FUTURE: Add ScreenShake or Particle spawn here
    }
}
