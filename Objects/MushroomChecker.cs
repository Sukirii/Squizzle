using UnityEngine;

public class MushroomChecker : MonoBehaviour
{
    public bool isTop;
    public bool isHP;

    public Mushroom mushroom;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isHP && collision.CompareTag("Player"))
        {
            mushroom.GetHit();

            if (collision.transform.position.x < transform.position.x)
                collision.GetComponent<Rigidbody2D>().velocity = new Vector2(collision.GetComponent<Rigidbody2D>().velocity.x, 10f);
            else
                collision.GetComponent<Rigidbody2D>().velocity = new Vector2(collision.GetComponent<Rigidbody2D>().velocity.x, 10f);

            return;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isTop && !mushroom.moveLock)
            mushroom.SwitchDirection();
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!isTop && !mushroom.moveLock)
            mushroom.SwitchDirection();
    }
}