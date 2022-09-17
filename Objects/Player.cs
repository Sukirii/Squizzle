using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [Header("Setup")]
    public GroundChecker groundChecker;
    public WallChecker wallChecker_Left, wallChecker_Right;
    Rigidbody2D rb;

    public GameObject DieEffect;

    SpriteRenderer spriteRenderer;
    Animator animator;

    public ParticleSystem dustEffect;

    InputManager inputManager;
    AudioManager audioManager;
    SceneManagement sceneManagement;
    GameManager gameManager;

    [Header("Stats")]
    const float moveSpeed = 10f;

    const float acceleration = 5f, iceAcceleration = 1f;
    const float decceleration = 5f, iceDecceleration = 1f;

    const float jumpHeight = 15f;
    const float jumpCutMultiplier = 0.5f;

    const float frictionAmount = 1f, iceFriction = 0f;

    const float velPower = 1f;

    bool isJumping;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        inputManager = FindObjectOfType<InputManager>();
        audioManager = FindObjectOfType<AudioManager>();
        sceneManagement = FindObjectOfType<SceneManagement>();
        gameManager = FindObjectOfType<GameManager>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        #region Sprite
        if (inputManager.movement != 0f)
        {
            if (rb.velocity.x < -0.01f)
            {
                if (!spriteRenderer.flipX)
                    dustEffect.Play();

                spriteRenderer.flipX = true;
            }
            else if (rb.velocity.x > 0.01f)
            {
                if (spriteRenderer.flipX)
                    dustEffect.Play();

                spriteRenderer.flipX = false;
            }
            else if (rb.velocity.y > 0.01f)
                dustEffect.Play();
        }
        #endregion

        #region WallChecker
        if (wallChecker_Left.isWalled && inputManager.movement < -0.01f && rb.velocity.y < 0.01f)
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -2f, Mathf.Infinity));
        else if (wallChecker_Right.isWalled && inputManager.movement > 0.01f && rb.velocity.y < 0.01f)
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -2f, Mathf.Infinity));

        if (wallChecker_Left.isWalled && inputManager.movement < -0.01f && !groundChecker.isGrounded && rb.velocity.y < -1.9f)
            animator.SetBool("IsWallSliding", true);
        else if (wallChecker_Right.isWalled && inputManager.movement > 0.01f && !groundChecker.isGrounded && rb.velocity.y < -1.9f)
            animator.SetBool("IsWallSliding", true);
        else
            animator.SetBool("IsWallSliding", false);
        #endregion

        if (transform.position.y < -10f)
            Die();
    }

    public void Die()
    {
        if (gameManager.isDead || gameManager.isFinished)
            return;

        gameManager.isDead = true;

        animator.Play("Die");
        rb.velocity = new Vector2(0f, rb.velocity.y);

        StartCoroutine(WaitForDeath());
    }

    IEnumerator WaitForDeath()
    {
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Death"));

        gameManager.Restart();

        Instantiate(DieEffect, transform.position, DieEffect.transform.rotation);
        Destroy(gameObject);
    }

    public void StartJump()
    {
        if (groundChecker.isGrounded || gameManager.isDead)
            return;

        if (wallChecker_Left.isWalled && !wallChecker_Left.sideLock)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            rb.AddForce(transform.up * jumpHeight + Vector3.right * 5f, ForceMode2D.Impulse);

            isJumping = true;
            wallChecker_Left.sideLock = true;
            wallChecker_Right.sideLock = false;
            StopAllCoroutines();
            StartCoroutine(WaitForJumpEnd());
        }
        else if (wallChecker_Right.isWalled && !wallChecker_Right.sideLock)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            rb.AddForce(transform.up * jumpHeight + Vector3.left * 5f, ForceMode2D.Impulse);

            isJumping = true;
            wallChecker_Right.sideLock = true;
            wallChecker_Left.sideLock = false;
            StopAllCoroutines();
            StartCoroutine(WaitForJumpEnd());
        }
    }

    public void Jump()
    {
        if(groundChecker.isGrounded && !isJumping && !gameManager.isDead)
        {
            rb.AddForce(transform.up * jumpHeight, ForceMode2D.Impulse);
            dustEffect.Play();

            isJumping = true;
            StopAllCoroutines();
            StartCoroutine(WaitForJumpEnd());
        }
    }

    IEnumerator WaitForJumpEnd()
    {
        yield return new WaitUntil(() => groundChecker.isGrounded);
        yield return new WaitUntil(() => rb.velocity.y < 0.01f);

        isJumping = false;
        wallChecker_Left.sideLock = false;
        wallChecker_Right.sideLock = false;
    }

    public void StopJump()
    {
        if (rb.velocity.y > 0 && !groundChecker.isGrounded)
            rb.AddForce((1 - jumpCutMultiplier) * rb.velocity.y * Vector2.down, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Spikes") && !gameManager.isDead)
        {
            audioManager.PlaySound(0);
            Die();
        }
        else if (collision.CompareTag("Finish") && !gameManager.isDead)
        {
            gameManager.FinishedLevel();
            sceneManagement.FadeTo(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else if (collision.CompareTag("Level Entry"))
            sceneManagement.FadeToString(collision.GetComponent<LevelEntry>().level);
        else if (collision.CompareTag("Collectable"))
        {
            gameManager.GotCollectable();
            Destroy(collision.gameObject);
        }
    }

    private void FixedUpdate()
    {
        float targetSpeed = inputManager.movement * moveSpeed;

        float speedDif = targetSpeed - rb.velocity.x;
        float accelRate;

        if (groundChecker.isIce)
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? iceAcceleration : iceDecceleration;
        else
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : decceleration;

        float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif);

        if (!gameManager.isDead)
            rb.AddForce(movement * Vector2.right);

        #region Friction
        if(groundChecker.isGrounded && Mathf.Abs(inputManager.movement) < 0.01f)
        {
            float amount;

            if (groundChecker.isIce)
                amount = Mathf.Min(Mathf.Abs(rb.velocity.x), Mathf.Abs(iceFriction));
            else
                amount = Mathf.Min(Mathf.Abs(rb.velocity.x), Mathf.Abs(frictionAmount));

            amount *= Mathf.Sign(rb.velocity.x);

            rb.AddForce(Vector2.right * -amount, ForceMode2D.Impulse);
        }
        #endregion
    }
}