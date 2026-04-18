using UnityEngine;

public class EnemyTraceController : MonoBehaviour
{
    public float defaultSpeed = 2f; // 기본 속도 설정값
    public float moveSpeed;         // 현재 속도
    public float raycastDistance = 0.5f;
    public float traceDistance = 5f;

    private Transform player;

    private void Start()
    {
        // [추가] 스테이지 시작 시 속도를 기본값으로 초기화
        moveSpeed = defaultSpeed;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;
    }

    private void Update()
    {
        if (player == null) return;

        Vector2 direction = player.position - transform.position;

        if (direction.magnitude > traceDistance)
            return;

        Vector2 directionNormalized = direction.normalized;

        // 레이캐스트 장애물 체크 (태그는 Obstacle 사용)
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, directionNormalized, raycastDistance);

        bool isBlocked = false;
        foreach (RaycastHit2D rHit in hits)
        {
            if (rHit.collider != null && rHit.collider.CompareTag("Obstacle"))
            {
                isBlocked = true;
                break;
            }
        }

        if (isBlocked)
        {
            Vector3 alternativeDirection = Quaternion.Euler(0f, 0f, -90f) * directionNormalized;
            transform.Translate(alternativeDirection * moveSpeed * Time.deltaTime, Space.World);
        }
        else
        {
            transform.Translate(directionNormalized * moveSpeed * Time.deltaTime, Space.World);
        }
    }
}