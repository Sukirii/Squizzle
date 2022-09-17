using UnityEngine;

public class WallChecker : MonoBehaviour
{
    [HideInInspector]
    public bool isWalled, sideLock;

    Player player;

    AudioManager audioManager;
    GameManager gameManager;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        audioManager = FindObjectOfType<AudioManager>();
        gameManager = FindObjectOfType<GameManager>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Mushroom") && !gameManager.isDead)
        {
            audioManager.PlaySound(0);
            player.Die();
            return;
        }

        if (collision.CompareTag("Grass") || collision.CompareTag("Ice"))
            isWalled = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Grass") || collision.CompareTag("Ice"))
            isWalled = false;
    }
}