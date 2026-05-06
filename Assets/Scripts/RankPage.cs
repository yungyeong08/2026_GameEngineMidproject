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
        RefreshRankList();
    }

    void RefreshRankList()
    {
        foreach (Transform child in contentRoot)
        {
            Destroy(child.gameObject);
        }

        var sortedDate = allDate.results.Where(r => r.stage == 5).OrderByDescending(x => x.score).ToList();

        for (int i = 0; i < sortedDate.Count; i++)
        {
            
            GameObject row = Instantiate(rowPrefab, contentRoot);
            TMP_Text rankText = row.GetComponentInChildren<TMP_Text>();
            rankText.text = $"{i + 1}. {sortedDate[i].playerName} - {sortedDate[i].score}";
        }
    }

}
