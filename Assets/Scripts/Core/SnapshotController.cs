using UnityEngine;
using System.Collections;

/// <summary>
/// Professional Viewfinder implementation with holding mechanic, 
/// screen flash, and smooth animations.
/// </summary>
public class SnapshotController : MonoBehaviour
{
    [Header("Settings")]
    public KeyCode captureKey = KeyCode.Mouse1;
    public Vector2Int resolution = new Vector2Int(1024, 1024);
    public float holdDistance = 1.0f;
    public float animationSpeed = 10f;

    [Header("References")]
    public Camera snapshotCamera;
    public GameObject photoPrefab; 
    public Transform holdPoint; // Point in front of camera
    public CanvasGroup flashUI; // A simple white Image on a Canvas

    private GameObject _heldPhoto;
    private bool _isHolding = false;

    void Update()
    {
        if (Input.GetKeyDown(captureKey))
        {
            if (!_isHolding)
            {
                StartCoroutine(CaptureRoutine());
            }
            else
            {
                ReleasePhoto();
            }
        }

        if (_isHolding && _heldPhoto != null)
        {
            // Smoothly keep the photo in front of the camera
            _heldPhoto.transform.position = Vector3.Lerp(_heldPhoto.transform.position, holdPoint.position, Time.deltaTime * animationSpeed);
            _heldPhoto.transform.rotation = Quaternion.Slerp(_heldPhoto.transform.rotation, holdPoint.rotation, Time.deltaTime * animationSpeed);
        }
    }

    private IEnumerator CaptureRoutine()
    {
        if (snapshotCamera == null || photoPrefab == null) yield break;

        // 1. Shutter Flash
        if (flashUI != null) StartCoroutine(FlashEffect());

        // 2. Create UNIQUE Texture
        RenderTexture tempRT = new RenderTexture(resolution.x, resolution.y, 24);
        tempRT.Create();
        
        snapshotCamera.targetTexture = tempRT;
        snapshotCamera.enabled = true;
        snapshotCamera.Render();
        snapshotCamera.enabled = false;

        // 3. Instantiate the Photo in 'Holding' state
        _heldPhoto = Instantiate(photoPrefab, holdPoint.position + (holdPoint.forward * -0.5f), holdPoint.rotation);
        
        // Disable physics while holding
        Rigidbody rb = _heldPhoto.GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;

        // 4. Apply Texture
        Renderer rend = _heldPhoto.GetComponent<Renderer>();
        if (rend != null)
        {
            Material photoMat = new Material(Shader.Find("Standard"));
            photoMat.mainTexture = tempRT;
            photoMat.EnableKeyword("_EMISSION");
            photoMat.SetTexture("_EmissionMap", tempRT);
            photoMat.SetColor("_EmissionColor", Color.white * 0.4f);
            rend.material = photoMat;
        }

        _isHolding = true;
        Debug.Log("[Snapshot] Photo captured and held.");
    }

    private void ReleasePhoto()
    {
        _isHolding = false;
        
        // Re-enable physics
        Rigidbody rb = _heldPhoto.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.AddForce(holdPoint.forward * 5f, ForceMode.Impulse);
        }

        _heldPhoto = null;
    }

    private IEnumerator FlashEffect()
    {
        flashUI.alpha = 1.0f;
        while (flashUI.alpha > 0)
        {
            flashUI.alpha -= Time.deltaTime * 3f;
            yield return null;
        }
    }
}
