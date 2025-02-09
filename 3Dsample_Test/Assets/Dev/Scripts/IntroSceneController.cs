using UnityEngine;

// GameStart 및 GameExit를 위함
public class IntroSceneController : MonoBehaviour
{
    public void GameStartEvent()
    {
        // SceneName이 Lobby인 것을 호출
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
