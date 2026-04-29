using UnityEngine;
using TMPro;

public class LeaderBoard : MonoBehaviour
{
    public TextMeshProUGUI stage1;
    public TextMeshProUGUI stage2;
    public TextMeshProUGUI stage3;
    public TextMeshProUGUI stage4;
    public TextMeshProUGUI stage5;
    void Start()
    {
        stage1.text = "Stage 1 : " + HighScore.Load(1).ToString();
        stage2.text = "Stage 2 : " + HighScore.Load(2).ToString();
        stage3.text = "Stage 3 : " + HighScore.Load(3).ToString();
        stage4.text = "Stage 4 : " + HighScore.Load(4).ToString();
        stage5.text = "Stage 5 : " + HighScore.Load(5).ToString();

    }

}
