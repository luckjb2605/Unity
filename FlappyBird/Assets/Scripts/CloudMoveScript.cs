using UnityEngine;

public class CloudMoveScript : MonoBehaviour
{
    public float averageSpeed;
    public float deviation;

    private float despawnLocation;

    void SetDespawnLocation()
    {
        SpriteRenderer sprtRndrr =
            GetComponent<SpriteRenderer>();

        float spriteSize =
            sprtRndrr.bounds.size.x;
        
        float halfWidth =
            Camera.main.orthographicSize
          * Camera.main.aspect;

        float leftBorder = 
            Camera.main.transform.position.x
          - halfWidth;

        despawnLocation = leftBorder - spriteSize / 2f;
    }

    void SetAverageSpeed()
    {
        averageSpeed = Random.Range(
            averageSpeed - deviation,
            averageSpeed + deviation
        );
    }

    void Start()
    {
        SetDespawnLocation();
        SetAverageSpeed();
    }
    

    void Update()
    {
        transform.position +=
            Vector3.left * averageSpeed * Time.deltaTime;
        
        if (transform.position.x <= despawnLocation)
        {
            Destroy(gameObject);
        }
    }
}
