using UnityEngine;
using System.Collections;

public class DisappearingPlatform : MonoBehaviour
{
    [Header("설정")]
    public float destroyDelay = 0.5f;
    public float respawnDelay = 3.0f;

    private SpriteRenderer sprite;
    private Collider2D col;
    private Color originalColor;
    private bool isStepped = false;

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        originalColor = sprite.color;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 1. 플레이어 태그 확인
        // 2. 위에서 아래로 떨어지는 힘이 있는지 확인 (y값이 음수면 아래로 누르는 중)
        if (collision.gameObject.CompareTag("Player") && !isStepped)
        {
            if (collision.relativeVelocity.y < 0.1f)
            {
                isStepped = true;
                StartCoroutine(FadeAndDisable());
            }
        }
    }

    IEnumerator FadeAndDisable()
    {
        float elapsed = 0f;
        while (elapsed < destroyDelay)
        {
            sprite.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.3f);
            yield return new WaitForSeconds(0.05f);
            sprite.color = originalColor;
            yield return new WaitForSeconds(0.05f);
            elapsed += 0.1f;
        }

        sprite.enabled = false;
        col.enabled = false;

        yield return new WaitForSeconds(respawnDelay);

        sprite.enabled = true;
        col.enabled = true;
        sprite.color = originalColor;
        isStepped = false;
    }
}