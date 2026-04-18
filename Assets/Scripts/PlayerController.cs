using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 5f;

    [Header("Detection")]
    public Transform groundCheck;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator pAni;
    private SpriteRenderer sprite;

    private bool isGrounded;
    private float moveInput;
    private bool isGiant = false;
    private bool isInvincible = false;
    private Vector3 originalScale;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        pAni = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        originalScale = transform.localScale;
    }

    private void Update()
    {
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        // 캐릭터 비율 유지하며 방향 전환 및 거대화
        float scaleMultiplier = isGiant ? 2f : 1f;
        if (moveInput < 0)
            transform.localScale = new Vector3(-originalScale.x * scaleMultiplier, originalScale.y * scaleMultiplier, originalScale.z);
        else if (moveInput > 0)
            transform.localScale = new Vector3(originalScale.x * scaleMultiplier, originalScale.y * scaleMultiplier, originalScale.z);

        // 바닥 체크 (에러 방지: groundCheck가 있을 때만 실행)
        if (groundCheck != null)
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.3f, groundLayer);
    }

    public void OnMove(InputValue value) => moveInput = value.Get<Vector2>().x;

    public void OnJump(InputValue value)
    {
        if (value.isPressed && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            pAni.SetTrigger("Jump");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 1. 가시나 낭떠러지 ("Dead" 태그)
        if (collision.CompareTag("Dead") || collision.CompareTag("Respawn"))
        {
            RestartLevel();
        }

        // 2. 적 ("Enemy" 태그) - 무적일 땐 살고, 아닐 땐 죽음
        if (collision.CompareTag("Enemy"))
        {
            if (!isInvincible) RestartLevel();
        }

        // 3. 무적 아이템 ("Item" 태그)
        if (collision.CompareTag("Item"))
        {
            StopAllCoroutines();
            StartCoroutine(ItemEffectRoutine());
            Destroy(collision.gameObject);
        }
    }

    IEnumerator ItemEffectRoutine()
    {
        isGiant = true;
        isInvincible = true;
        int originalLayer = gameObject.layer;
        gameObject.layer = LayerMask.NameToLayer("Invincible");

        float timer = 0;
        while (timer < 5f)
        {
            sprite.color = new Color(1, 1, 1, 0.5f);
            yield return new WaitForSeconds(0.1f);
            sprite.color = new Color(1, 1, 1, 1f);
            yield return new WaitForSeconds(0.1f);
            timer += 0.2f;
        }

        gameObject.layer = originalLayer;
        isInvincible = false;
        isGiant = false;
    }

    void RestartLevel() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
}