using UnityEngine;

// GameStart �� GameExit�� ����
public class IntroSceneController : MonoBehaviour
{
    public void GameStartEvent()
    {
        // SceneName�� Lobby�� ���� ȣ��
        UnityNote.SceneLoader.Instance.LoadScene(SceneNames.Lobby);
        // Checekd
        Debug.Log("GameStartEvent Called");
    }

    public void GameExitEvent()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}
