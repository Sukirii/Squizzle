using System.Collections;
using UnityEngine;

public class Mushroom : MonoBehaviour
{
    public MushroomChecker checker;
    Rigidbody2D rb;
    Animator animator;

    AudioManager audioManager;

    public GameObject dieEffect;

    public int startHP = 3;
    int HP;

    bool isLeft;
    [HideInInspector]
    public bool moveLock;

    bool isDead;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioManager = FindObjectOfType<AudioManager>();
    }

    private void Start()
    {
        HP = startHP;

        isLeft = transform.eulerAngles.y == 180f;
    }

    private void FixedUpdate()
    {
        if (!moveLock)
            rb.velocity = new Vector2(transform.right.x * 3.5f, rb.velocity.y);
    }

    public void GetHit()
    {
        if (isDead)
            return;

        HP--;

        animator.Play("Hit");
        audioManager.PlaySound(1);

        if (HP <= 0)
        {
            isDead = true;
            Instantiate(dieEffect, transform.position, dieEffect.transform.rotation);
            Destroy(gameObject);
        }
    }

    public void SwitchDirection()
    {
        if (moveLock)
            return;

        moveLock = true;
        rb.velocity = new Vector2(0f, rb.velocity.y);

        StartCoroutine(TurnAround());
    }

    IEnumerator TurnAround()
    {
        yield return new WaitForSeconds(0.5f);

        isLeft = !isLeft;

        if (isLeft)
            transform.eulerAngles = new Vector2(0f, 180f);
        else
            transform.eulerAngles = new Vector2(0f, 0f);

        yield return new WaitForSeconds(1f);

        moveLock = false;
    }
}