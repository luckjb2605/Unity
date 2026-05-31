using UnityEngine;

public class SetBackgroundSize : MonoBehaviour
{
    void Start()
    {
        SpriteRenderer sr =
            GetComponent<SpriteRenderer>();

        float spriteHeight =
            sr.bounds.size.y;

        float spriteWidth =
            sr.bounds.size.x;

        float worldHeight =
            Camera.main.orthographicSize * 2f;

        float worldWidth =
            worldHeight * Camera.main.aspect;

        transform.localScale = new Vector3(
            worldWidth  / spriteWidth,
            worldHeight / spriteHeight,
            1f
        );
    }
}
