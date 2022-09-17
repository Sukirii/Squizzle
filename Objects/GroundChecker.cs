using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    [HideInInspector]
    public bool isGrounded, isIce;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Grass") || collision.CompareTag("Ice") || collision.CompareTag("Platform"))
            isGrounded = true;

        if (collision.CompareTag("Ice"))
            isIce = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Grass") || collision.CompareTag("Ice") || collision.CompareTag("Platform"))
            isGrounded = false;

        if (collision.CompareTag("Ice"))
            isIce = false;
    }
}