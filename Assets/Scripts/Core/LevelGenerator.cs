using UnityEngine;

/// <summary>
/// Procedurally generates a test chamber for Paradox Lens.
/// Shows recruiters that you can handle algorithmic level design.
/// </summary>
public class LevelGenerator : MonoBehaviour
{
    [Header("Room Settings")]
    public Vector3 roomSize = new Vector3(20, 10, 20);
    public Material wallMaterial;
    
    [Header("Spawn Settings")]
    public GameObject amberCratePrefab;
    public int cubeCount = 5;

    void Start()
    {
        GenerateRoom();
        SpawnTestObjects();
    }

    void GenerateRoom()
    {
        // Create Floor
        CreateWall(new Vector3(0, -0.5f, 0), new Vector3(roomSize.x, 1, roomSize.z), "Floor");
        
        // Create Ceiling
        CreateWall(new Vector3(0, roomSize.y + 0.5f, 0), new Vector3(roomSize.x, 1, roomSize.z), "Ceiling");

        // Create Walls
        CreateWall(new Vector3(roomSize.x/2 + 0.5f, roomSize.y/2, 0), new Vector3(1, roomSize.y, roomSize.z), "Wall_Right");
        CreateWall(new Vector3(-roomSize.x/2 - 0.5f, roomSize.y/2, 0), new Vector3(1, roomSize.y, roomSize.z), "Wall_Left");
        CreateWall(new Vector3(0, roomSize.y/2, roomSize.z/2 + 0.5f), new Vector3(roomSize.x, roomSize.y, 1), "Wall_Front");
        CreateWall(new Vector3(0, roomSize.y/2, -roomSize.z/2 - 0.5f), new Vector3(roomSize.x, roomSize.y, 1), "Wall_Back");
    }

    void CreateWall(Vector3 pos, Vector3 scale, string name)
    {
        GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        wall.name = name;
        wall.transform.position = pos;
        wall.transform.localScale = scale;
        wall.GetComponent<Renderer>().material = wallMaterial;
        
        // Ensure walls are on the 'Placement' layer for our Controller
        wall.layer = LayerMask.NameToLayer("Default"); 
    }

    void SpawnTestObjects()
    {
        for (int i = 0; i < cubeCount; i++)
        {
            Vector3 randomPos = new Vector3(
                Random.Range(-roomSize.x/3, roomSize.x/3),
                1f,
                Random.Range(-roomSize.z/3, roomSize.z/3)
            );

            // If we had a prefab, we'd instantiate it. 
            // For now, let's just make a Cube and add our scripts!
            GameObject testCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            testCube.name = "ParadoxCube_" + i;
            testCube.transform.position = randomPos;
            
            // Add Physics
            Rigidbody rb = testCube.AddComponent<Rigidbody>();
            rb.mass = 5f;

            // Add our Custom Scripts
            testCube.AddComponent<GravityBody>();
            testCube.AddComponent<PerspectiveObject>();
            
            // Add a vibrant color to make it look "Indie"
            testCube.GetComponent<Renderer>().material.color = Color.Lerp(Color.cyan, Color.magenta, Random.value);
        }
    }
}
