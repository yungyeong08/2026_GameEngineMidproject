using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform posA, posB; // 왕복할 두 지점
    public float speed = 2f;      // 이동 속도
    private Vector3 targetPos;

    void Start()
    {
        // 시작할 때 B지점을 향해 출발하게 설정
        targetPos = posB.position;
    }

    void Update()
    {
        // 현재 위치에서 타겟 위치로 이동
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

        // 타겟 지점에 거의 도착(0.1f 이내)하면 타겟을 반대쪽으로 변경
        if (Vector3.Distance(transform.position, targetPos) < 0.1f)
        {
            targetPos = (targetPos == posA.position) ? posB.position : posA.position;
        }
    }
}