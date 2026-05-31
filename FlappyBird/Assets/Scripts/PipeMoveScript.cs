using UnityEngine;

public class PipeMoveScript : MonoBehaviour
{
    public float moveSpeed = 5;
    private float despawnPoint;
    
    void Start()
    {
        SpriteRenderer sprtRndrr =
            GetComponentInChildren<SpriteRenderer>();

        float spriteSize =
            sprtRndrr.bounds.size.x;
        
        float halfWidth =
            Camera.main.orthographicSize
          * Camera.main.aspect;

        float leftBorder = 
            Camera.main.transform.position.x
          - halfWidth;

        despawnPoint = leftBorder - spriteSize / 2f;
    }

    void Update()
    {
        transform.position += Vector3.left * moveSpeed * Time.deltaTime;

        if (transform.position.x <= despawnPoint)
        {
            // Debug.Log("Pipe Deleted");
            Destroy(gameObject);
        }
    }
}
