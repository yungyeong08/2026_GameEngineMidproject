using UnityEngine;

public class EnemyTraceController : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float raycastDistance = 0.5f;
    public float traceDistance = 5f;

    private Transform player;

    private void Start()
    {
        moveSpeed = 2f;

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

        // 레이캐스트로 장애물 확인
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, directionNormalized, raycastDistance);
        Debug.DrawRay(transform.position, directionNormalized * raycastDistance, Color.red);

        bool isBlocked = false;
        foreach (RaycastHit2D rHit in hits)
        {
            // 오타 수정: Obsracle -> Obstacle (유니티 태그도 Obstacle로 만드세요!)
            if (rHit.collider != null && rHit.collider.CompareTag("Obstacle"))
            {
                isBlocked = true;
                break;
            }
        }

        if (isBlocked)
        {
            // 장애물이 있으면 우회 시도
            Vector3 alternativeDirection = Quaternion.Euler(0f, 0f, -90f) * directionNormalized;
            transform.Translate(alternativeDirection * moveSpeed * Time.deltaTime, Space.World);
        }
        else
        {
            // 장애물 없으면 플레이어 추적
            transform.Translate(directionNormalized * moveSpeed * Time.deltaTime, Space.World);
        }
    }
}