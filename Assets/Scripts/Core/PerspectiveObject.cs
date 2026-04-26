using UnityEngine;

/// <summary>
/// Core logic for an object that can be manipulated via perspective.
/// Enhanced with smoothing for a professional 'AAA' feel.
/// </summary>
public class PerspectiveObject : MonoBehaviour
{
    [SerializeField] private float _smoothingSpeed = 15f;
    
    private Rigidbody _rb;
    private float _initialDistance;
    private Vector3 _initialScale;
    private bool _isPickedUp;
    
    private Vector3 _targetScale;
    private Vector3 _targetPosition;

    public bool IsPickedUp => _isPickedUp;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _targetScale = transform.localScale;
    }

    void Update()
    {
        if (_isPickedUp)
        {
            // Smoothly interpolate to target scale and position
            transform.localScale = Vector3.Lerp(transform.localScale, _targetScale, Time.deltaTime * _smoothingSpeed);
        }
    }

    /// <summary>
    /// Records initial state when picked up by the camera.
    /// </summary>
    public void OnPickup(float distance)
    {
        _isPickedUp = true;
        _initialDistance = distance;
        _initialScale = transform.localScale;
        _targetScale = _initialScale;
        
        if (_rb != null)
        {
            _rb.isKinematic = true;
            _rb.useGravity = false;
        }
    }

    /// <summary>
    /// Updates target scale based on new distance.
    /// </summary>
    public void UpdatePerspectiveScale(float currentDistance)
    {
        if (!_isPickedUp) return;

        float scaleMultiplier = currentDistance / _initialDistance;
        _targetScale = _initialScale * scaleMultiplier;
    }

    /// <summary>
    /// Finalizes placement and re-aligns gravity.
    /// </summary>
    public void OnRelease(Vector3 newGravityDirection)
    {
        _isPickedUp = false;
        transform.localScale = _targetScale; // Snap to final scale on release

        if (_rb != null)
        {
            _rb.isKinematic = false;
            
            GravityBody gBody = GetComponent<GravityBody>();
            if (gBody != null)
            {
                gBody.GravityDirection = newGravityDirection;
            }
        }
    }
}
