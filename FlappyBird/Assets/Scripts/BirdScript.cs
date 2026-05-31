using UnityEngine;

public class BirdScript : MonoBehaviour
{
    public Rigidbody2D myRigidbody;
    public float flapStrength;
    public LogicScript logic;
    public bool birdIsAlive = true;

    private float screenHeight;

    void Start()
    {
        logic = GameObject
            .FindGameObjectWithTag("Logic")
            .GetComponent<LogicScript>();

        screenHeight = Camera.main.orthographicSize;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) == true
         && birdIsAlive
        )
        {
            myRigidbody.linearVelocity =
                Vector2.up * flapStrength;
        }

        if (transform.position.y > screenHeight
         || transform.position.y < -screenHeight)
        {
            logic.GameOver();
            birdIsAlive = false;
        }
    }

  private void OnCollisionEnter2D(Collision2D collision)
  {
    logic.GameOver();
    birdIsAlive = false;
  }
}
