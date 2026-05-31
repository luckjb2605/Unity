using UnityEngine;

public class PipeSpawnerScript : MonoBehaviour
{
    public GameObject pipePrefab;
    public float spawnRate;
    public float heightOffSet;
    
    private float timer = 0f;
    private float spawnX;
    
    void Start()
    {
        // Get renderer to access sprite size.
        // Has to get from children
        // What would return an array, returns the first child's
        SpriteRenderer sprtRendrr =
            pipePrefab.GetComponentInChildren<SpriteRenderer>();

        float pipeHalfWidth =
            sprtRendrr.bounds.size.x / 2f;

        // ortographic size is similar to what a radius is
        // to a circle. It measures height only.
        float screenHalfHeight =
            Camera.main.orthographicSize;

        // since ortographicSize returns relation to height,
        // if you want width, you need to use the aspect ratio,
        // knowing that aspect = width / height.
        // so width = aspect * height.
        float screenHalfWidth = 
            screenHalfHeight * Camera.main.aspect;

        float rightEdge = 
            Camera.main.transform.position.x + screenHalfWidth;
        
        spawnX = rightEdge + pipeHalfWidth;
    }

    void SpawnPipe()
    {
        float lowestPoint = transform.position.y - heightOffSet;
        float highestPoint = transform.position.y + heightOffSet;
        float randomY = Random.Range(lowestPoint, highestPoint);
        
        Instantiate(
            pipePrefab,
            new Vector3(spawnX, randomY, 0),
            transform.rotation // same rotation as this object
            // could also use Quaternion.identity for no rotation.
        );
    }

    void Update()
    {
        timer += Time.deltaTime;
        
        if (timer >= spawnRate)
        {
            SpawnPipe();
            timer = 0;
        }
    }
}
