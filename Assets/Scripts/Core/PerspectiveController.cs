using UnityEngine;

/// <summary>
/// Manages the player's Spatial Camera interaction.
/// Implementation of the 'farthest point' raycast logic for perspective scaling.
/// </summary>
public class PerspectiveController : MonoBehaviour
{
    [Header("Settings")]
    public float maxPickUpDistance = 50f;
    public LayerMask placementMask;
    
    [Header("References")]
    public Transform holdParent; // Child of camera where objects are visually held

    [Header("Physics & Juice")]
    [SerializeField] private float _throwForce = 2.0f;
    [SerializeField] private float _velocitySmoothTime = 0.1f;

    private PerspectiveObject _currentObject;
    private Camera _cam;
    private Vector3 _lastCamPos;
    private Vector3 _camVelocity;

    void Start()
    {
        _cam = GetComponent<Camera>();
        _lastCamPos = transform.position;
    }

    void Update()
    {
        CalculateCameraVelocity();

        if (Input.GetMouseButtonDown(0))
        {
            if (_currentObject == null)
            {
                TryPickUp();
            }
            else
            {
                DropObject();
            }
        }

        if (_currentObject != null)
        {
            UpdateObjectPlacement();
        }
    }

    private void CalculateCameraVelocity()
    {
        // Simple velocity tracking for momentum transfer
        Vector3 instantVelocity = (transform.position - _lastCamPos) / Time.deltaTime;
        _camVelocity = Vector3.Lerp(_camVelocity, instantVelocity, Time.deltaTime / _velocitySmoothTime);
        _lastCamPos = transform.position;
    }

    private void TryPickUp()
    {
        Ray ray = _cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (Physics.Raycast(ray, out RaycastHit hit, maxPickUpDistance))
        {
            PerspectiveObject pObj = hit.collider.GetComponent<PerspectiveObject>();
            if (pObj != null)
            {
                _currentObject = pObj;
                _currentObject.OnPickup(hit.distance);
                _currentObject.transform.SetParent(holdParent);
            }
        }
    }

    private void UpdateObjectPlacement()
    {
        Ray ray = _cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        float targetDistance = maxPickUpDistance;

        if (Physics.Raycast(ray, out RaycastHit hit, maxPickUpDistance, placementMask))
        {
            targetDistance = hit.distance;
        }

        _currentObject.transform.position = ray.GetPoint(targetDistance);
        _currentObject.UpdatePerspectiveScale(targetDistance);
    }

    private void DropObject()
    {
        _currentObject.transform.SetParent(null);
        
        // Calculate the combined release velocity (Camera movement + Forward momentum)
        Vector3 releaseVelocity = _camVelocity + (transform.forward * _throwForce);
        
        _currentObject.OnRelease(transform.forward, releaseVelocity); 
        _currentObject = null;
    }
}
