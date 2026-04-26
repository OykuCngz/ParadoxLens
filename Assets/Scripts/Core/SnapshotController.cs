using UnityEngine;

/// <summary>
/// Manages the 'Viewfinder' logic: capturing the screen and projecting it onto a physical object.
/// High-end technical implementation for specialized game dev portfolios.
/// </summary>
public class SnapshotController : MonoBehaviour
{
    [Header("Settings")]
    public KeyCode captureKey = KeyCode.Mouse1;
    public LayerMask captureMask;
    public Vector2Int resolution = new Vector2Int(1024, 1024);

    [Header("References")]
    public Camera snapshotCamera;
    public GameObject photoPrefab; 
    public Transform spawnPoint;   

    void Update()
    {
        if (Input.GetKeyDown(captureKey))
        {
            TakeSnapshot();
        }
    }

    public void TakeSnapshot()
    {
        if (snapshotCamera == null || photoPrefab == null) return;

        // 1. Create a UNIQUE 'Film' for this specific photo
        RenderTexture tempRT = new RenderTexture(resolution.x, resolution.y, 24);
        tempRT.Create();
        
        snapshotCamera.targetTexture = tempRT;
        snapshotCamera.enabled = true;
        snapshotCamera.Render();
        snapshotCamera.enabled = false;

        // 2. Create the Physical Photo
        GameObject photo = Instantiate(photoPrefab, spawnPoint.position, spawnPoint.rotation);
        
        // 3. Apply the 'Captured Reality' to a unique material
        Renderer photoRenderer = photo.GetComponent<Renderer>();
        if (photoRenderer != null)
        {
            Material photoMat = new Material(Shader.Find("Standard"));
            photoMat.mainTexture = tempRT;
            
            // Make it look 'Photo-like' (Self-illuminated)
            photoMat.EnableKeyword("_EMISSION");
            photoMat.SetTexture("_EmissionMap", tempRT);
            photoMat.SetColor("_EmissionColor", Color.white * 0.5f);
            
            photoRenderer.material = photoMat;
        }

        // 4. Add physics to the photo
        Rigidbody photoRb = photo.GetComponent<Rigidbody>();
        if (photoRb != null)
        {
            photoRb.AddForce(spawnPoint.forward * 2f, ForceMode.Impulse);
        }
    }
}
