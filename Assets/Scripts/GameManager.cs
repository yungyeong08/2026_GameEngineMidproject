using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public TMP_InputField inputField;
    public Button gameStartButton;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameStartButton.onClick.AddListener(OnGameStartButtonClicked);
    }

    private void OnGameStartButtonClicked()
    {
        string playerName = inputField.text.Trim();
        if (string.IsNullOrEmpty(playerName))
        {
            Debug.LogWarning("플레이어 이름을 입력해주세요.");
            return;
        }
        // 플레이어 이름을 PlayerPrefs에 저장
        PlayerPrefs.SetString("PlayerName", playerName);
        PlayerPrefs.Save();
        Debug.Log("플레이어 이름이 저장됨: " + playerName);

        // 게임 씬으로 이동
        SceneManager.LoadScene("Level_1");
    }
}
