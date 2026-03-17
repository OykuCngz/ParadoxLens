using UnityEngine;

/// <summary>
/// Allows an object to have its own unique gravity vector.
/// Essential for the "Gravity Projector" mechanic.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class GravityBody : MonoBehaviour
{
    [SerializeField] private Vector3 _gravityDirection = Vector3.down;
    [SerializeField] private float _gravityMagnitude = 9.81f;
    
    private Rigidbody _rb;

    public Vector3 GravityDirection
    {
        get => _gravityDirection;
        set => _gravityDirection = value.normalized;
    }

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        // Disable Unity's default global gravity for this object
        _rb.useGravity = false;
    }

    void FixedUpdate()
    {
        ApplyCustomGravity();
    }

    private void ApplyCustomGravity()
    {
        // Calculate gravitational force: F = m * g
        Vector3 gravityForce = _gravityDirection * _gravityMagnitude * _rb.mass;
        _rb.AddForce(gravityForce, ForceMode.Force);

        // Optional: Re-align object's "down" to match the gravity direction
        // (Great for things like crates or characters that should stand 'up' on the new surface)
        if (_gravityDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.FromToRotation(transform.up, -_gravityDirection) * transform.rotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * 5f);
        }
    }

    /// <summary>
    /// Sets the gravity to align with the provided forward vector.
    /// Used when the Perspective Lens "drops" an object onto a surface.
    /// </summary>
    public void SetGravityFromForward(Vector3 forward)
    {
        // We set the gravity to be the opposite of the surface normal, 
        // or simply follow the direction the player was looking.
        _gravityDirection = forward.normalized;
    }
}
