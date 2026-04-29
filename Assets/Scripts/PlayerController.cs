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

    [Header("Stage Settings")]
    public bool needsKey = false; // 이 스테이지에서 열쇠가 필요한지 여부 (체크박스)
    public bool hasKey = false;   // 열쇠를 먹었는지 여부

    private Rigidbody2D rb;
    private Animator pAni;
    private SpriteRenderer sprite;

    private bool isGrounded;
    private float moveInput;
    private bool isInvincible = false; // 무적 상태 확인용

    private float originalJumpForce; // 원래 점프력을 저장할 변수

    private float originalMoveSpeed;

    float score;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        pAni = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();

        // 캐릭터가 물리 마찰로 넘어지지 않게 회전 고정
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        originalJumpForce = jumpForce;
        originalMoveSpeed = moveSpeed;

        score = 0f;
    }

    private void Update()
    {
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        // 방향 전환
        if (moveInput < 0) transform.localScale = new Vector3(-1, 1, 1);
        else if (moveInput > 0) transform.localScale = new Vector3(1, 1, 1);

        // 바닥 체크 (Raycast)
        if (groundCheck != null)
        {
            // 마지막 인자에 groundLayer가 꼭 들어가 있는지 확인하세요!
            RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, 0.5f, groundLayer);
            isGrounded = hit.collider != null;

            // 시각적으로 레이저가 어디까지 가는지 확인하려면 아래 줄을 추가해보세요 (Scene 뷰에서 보임)
            Debug.DrawRay(groundCheck.position, Vector2.down * 0.5f, Color.red);
        }
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
        // 1. 리스폰 대상 (Respawn 또는 Enemy 태그)
        if (collision.CompareTag("Respawn") || collision.CompareTag("Enemy"))
        {
            if (!isInvincible) // 무적 아닐 때만 죽음
            {
                RestartLevel();
            }
            else
            {
                Debug.Log("무적 상태라 죽지 않습니다.");
            }
        }

        // 2. 열쇠 획득 ("Key" 태그)
        if (collision.CompareTag("Key"))
        {
            hasKey = true;
            Debug.Log("열쇠 획득!");
            Destroy(collision.gameObject);
        }

        // 3. 골인 지점 ("Finish" 태그)
        if (collision.CompareTag("Finish"))
        {
            HighScore.TrySet(SceneManager.GetActiveScene().buildIndex, (int)Time.timeSinceLevelLoad);

            // 열쇠가 필요 없는 스테이지거나, 필요할 때 열쇠가 있으면 통과
            if (!needsKey || (needsKey && hasKey))
            {
                collision.GetComponent<LevelObject>().MoveToNextLevel();
            }
            else
            {
                Debug.Log("열쇠가 필요합니다!");
            }
        }

        // 4. 무적 아이템 ("Item" 태그)
        if (collision.CompareTag("Item"))
        {
            StopAllCoroutines();
            StartCoroutine(InvincibleRoutine());
            Destroy(collision.gameObject);
            score += 10f;
        }

        if (collision.CompareTag("JumpItem"))
        {
            StopCoroutine("JumpBoostRoutine"); // 이미 실행 중이면 멈추고 새로 시작
            StartCoroutine(JumpBoostRoutine());
            Destroy(collision.gameObject);
        }

        if (collision.CompareTag("SpeedItem"))
        {
            StopCoroutine("SpeedBoostRoutine"); // 이미 실행 중이면 멈추고 새로 시작
            StartCoroutine(SpeedBoostRoutine());
            Destroy(collision.gameObject);
        }
    }

    IEnumerator InvincibleRoutine()
    {
        isInvincible = true;
        float timer = 0;
        float duration = 10f; // 피지컬 보완을 위해 10초로 늘려두었습니다.

        while (timer < duration)
        {
            sprite.color = new Color(1, 1, 1, 0.5f);
            yield return new WaitForSeconds(0.1f);
            sprite.color = new Color(1, 1, 1, 1f);
            yield return new WaitForSeconds(0.1f);
            timer += 0.2f;
        }

        isInvincible = false;
        sprite.color = Color.white;
    }

    IEnumerator JumpBoostRoutine()
    {
        Debug.Log("점프력 상승! 스테이지 끝날 때까지 유지됩니다.");

        // 안전장치: 혹시라도 originalJumpForce가 0이면 기본값 설정
        if (originalJumpForce <= 0) originalJumpForce = 5f;

        // 점프력을 2배(혹은 원하는 만큼)로 높입니다.
        jumpForce = originalJumpForce * 2.0f;

        // 아이템 효과 중임을 알리기 위해 색상 변경
        sprite.color = Color.cyan;

        // [중요] 시간을 기다리거나 원래대로 돌리는 코드를 모두 삭제했습니다.
        // 이렇게 하면 스테이지가 새로 시작되거나 다음 씬으로 넘어가기 전까지 효과가 유지됩니다.
        yield break;
    }

    IEnumerator SpeedBoostRoutine()
    {
        Debug.Log("이동속도 상승!");
        moveSpeed = originalMoveSpeed * 1.8f; // 속도를 1.8배 증가 (조절 가능)

        // 효과 중임을 알리기 위해 노란색(번개 색)으로 변경
        sprite.color = Color.yellow;

        yield return new WaitForSeconds(5f); // 5초 동안 유지

        moveSpeed = originalMoveSpeed; // 원래대로 복구
        sprite.color = Color.white;
        Debug.Log("이동속도 원상복구");
    }

    void RestartLevel() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
}