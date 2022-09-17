using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    public Transform cam;
    public float relativeMove = 0.3f;

    private void Update()
    {
        transform.position = new Vector2(cam.position.x * relativeMove, transform.position.y);
    }
}