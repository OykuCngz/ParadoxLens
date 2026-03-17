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

    private PerspectiveObject _currentObject;
    private Camera _cam;

    void Start()
    {
        _cam = GetComponent<Camera>();
    }

    void Update()
    {
        // Simple Input Logic (LMB to toggle pick/drop)
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
        // 1. Calculate how far we can move the object before hitting something
        Ray ray = _cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        float targetDistance = maxPickUpDistance;

        if (Physics.Raycast(ray, out RaycastHit hit, maxPickUpDistance, placementMask))
        {
            // Subtract a small buffer based on object bounds (simplified here)
            targetDistance = hit.distance;
        }

        // 2. Adjust object position to that distance
        _currentObject.transform.position = ray.GetPoint(targetDistance);

        // 3. Trigger the "Paradox Math" scaling
        _currentObject.UpdatePerspectiveScale(targetDistance);
    }

    private void DropObject()
    {
        _currentObject.transform.SetParent(null);
        _currentObject.OnRelease(transform.forward); // Re-orient gravity to where we were looking
        _currentObject = null;
    }
}
