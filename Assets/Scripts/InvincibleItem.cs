using UnityEngine;

public class InvincibleItem : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 플레이어의 무적 함수 호출
            collision.GetComponent<PlayerHealth>().StartInvincibility();

            // 아이템 제거
            Destroy(gameObject);
        }
    }
}