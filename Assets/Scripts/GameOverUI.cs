using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private string gameplaySceneName = "Gameplay";
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    public void Retry()
    {
        SceneManager.LoadScene(gameplaySceneName);
    }

    //public void QuitToMenu()
    //{
    //    SceneManager.LoadScene(mainMenuSceneName);
    //}
}