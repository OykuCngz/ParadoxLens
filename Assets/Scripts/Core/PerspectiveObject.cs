using UnityEngine;

/// <summary>
/// Core logic for an object that can be manipulated via perspective.
/// Displays the "Hard Math" components requested for the Polish internship portfolio.
/// </summary>
public class PerspectiveObject : MonoBehaviour
{
    private Rigidbody _rb;
    private float _initialDistance;
    private Vector3 _initialScale;
    private bool _isPickedUp;

    public bool IsPickedUp => _isPickedUp;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// Records initial state when picked up by the camera.
    /// </summary>
    public void OnPickup(float distance)
    {
        _isPickedUp = true;
        _initialDistance = distance;
        _initialScale = transform.localScale;
        
        // Disable physics while holding
        if (_rb != null)
        {
            _rb.isKinematic = true;
            _rb.useGravity = false;
        }
    }

    /// <summary>
    /// Updates scale based on new distance to maintain perceived size.
    /// Formula: NewScale = InitialScale * (NewDistance / InitialDistance)
    /// </summary>
    public void UpdatePerspectiveScale(float currentDistance)
    {
        if (!_isPickedUp) return;

        float scaleMultiplier = currentDistance / _initialDistance;
        transform.localScale = _initialScale * scaleMultiplier;
    }

    /// <summary>
    /// Finalizes placement and re-aligns gravity.
    /// </summary>
    public void OnRelease(Vector3 newGravityDirection)
    {
        _isPickedUp = false;

        if (_rb != null)
        {
            _rb.isKinematic = false;
            
            // Integrate with the custom gravity system
            GravityBody gBody = GetComponent<GravityBody>();
            if (gBody != null)
            {
                gBody.GravityDirection = newGravityDirection;
            }
        }
    }
}
