using UnityEngine;

public class CloudSpawnerScript : MonoBehaviour
{
    public GameObject cloudPrefab;
    public float spawnRate;

    private float timer;
    private float spawnX;
    
    void Start()
    {
        SpriteRenderer cloudSpriteRndr =
            cloudPrefab.GetComponent<SpriteRenderer>();
        
        float cloudWidth =
            cloudSpriteRndr.bounds.size.x;

        float screenHalfWidth =
            Camera.main.orthographicSize * Camera.main.aspect;

        spawnX = screenHalfWidth + cloudWidth / 2.0f;
    }

    void Update()
    {
        timer += Time.deltaTime;
        
        if (timer >= spawnRate)
        {
            SpawnCloud();
            timer = 0;
        }
    }

    void SpawnCloud()
    {
        // "Half Height"
        float screenHeight = Camera.main.orthographicSize;
        float randomY = Random.Range(-screenHeight, screenHeight);

        Instantiate(
            cloudPrefab,
            new Vector3(spawnX, randomY, 0),
            Quaternion.identity
        );
    }
}
