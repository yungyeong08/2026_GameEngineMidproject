using UnityEngine;
using System.Linq;
using TMPro;

public class RankPage : MonoBehaviour
{
    [SerializeField] Transform contentRoot;
    [SerializeField] GameObject rowPrefab;

    StageResultList allDate;

    void Awake()
    {
        allDate = StageResultSaver.LoaRank();
        OnClickStage1(); // 처음에 켜졌을 때는 1스테이지 보여주기
    }

    // [버튼 연결용 함수들을 아예 분리하기]
    // 유니티 STAGE 1 버튼에는 이 함수를 연결 (숫자 입력 필요 없음!)
    public void OnClickStage1() => RefreshRankList(1);

    // 유니티 STAGE 2 버튼에는 이 함수를 연결
    public void OnClickStage2() => RefreshRankList(2);

    // 유니티 STAGE 3 버튼에는 이 함수를 연결
    public void OnClickStage3() => RefreshRankList(3);


    // 실제 로직은 여기서 처리 (내부 함수로 변경)
    private void RefreshRankList(int stageIndex)
    {
        foreach (Transform child in contentRoot)
        {
            Destroy(child.gameObject);
        }

        var sortedDate = allDate.results
            .Where(r => r.stage == stageIndex)
            .OrderByDescending(x => x.score)
            .ToList();

        for (int i = 0; i < sortedDate.Count; i++)
        {
            GameObject row = Instantiate(rowPrefab, contentRoot);
            TMP_Text rankText = row.GetComponentInChildren<TMP_Text>();
            rankText.text = $"{i + 1}. {sortedDate[i].playerName} - {sortedDate[i].score}";
        }
    }
}