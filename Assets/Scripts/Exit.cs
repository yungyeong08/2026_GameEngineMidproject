using UnityEngine;

public class QuitHandler : MonoBehaviour
{
    public void ExitGame()
    {
        // 실제 빌드된 게임을 종료함
        Application.Quit();

        // 유니티 에디터 상에서 테스트할 때 꺼지게 함 (빌드 후에는 상관없음)
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}