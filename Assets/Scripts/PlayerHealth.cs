using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public bool isInvincible = false;
    public float duration = 3f; // 무적 시간
    private SpriteRenderer sprite;

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    // 아이템이 이 함수를 호출하게 됩니다
    public void StartInvincibility()
    {
        // 이미 무적 상태라면 중복 실행 방지
        StopCoroutine("InvincibilityRoutine");
        StartCoroutine("InvincibilityRoutine");
    }

    IEnumerator InvincibilityRoutine()
    {
        isInvincible = true;

        // 1. 레이어 변경 (물리적 충돌 무시)
        int originalLayer = gameObject.layer;
        gameObject.layer = LayerMask.NameToLayer("Invincible");

        // 2. 무적 지속 시간 동안 깜빡거리는 효과
        float timer = 0;
        while (timer < duration)
        {
            // 알파값(투명도)을 0.5와 1 사이로 왔다갔다
            sprite.color = new Color(1, 1, 1, 0.5f);
            yield return new WaitForSeconds(0.1f);
            sprite.color = new Color(1, 1, 1, 1f);
            yield return new WaitForSeconds(0.1f);

            timer += 0.2f;
        }

        // 3. 원래 상태로 복구
        sprite.color = new Color(1, 1, 1, 1f);
        gameObject.layer = originalLayer;
        isInvincible = false;
    }

    // 적과 부딪혔을 때 호출할 데미지 함수 예시
    public void TakeDamage()
    {
        if (isInvincible) return; // 무적이면 데미지 로직 실행 안 함

        Debug.Log("데미지 입음!");
    }
}