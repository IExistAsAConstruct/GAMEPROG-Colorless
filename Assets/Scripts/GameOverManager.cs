using UnityEngine;
using UnityEngine.SceneManagement;
public class GameOverManager : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private string gameOverSceneName = "GameOver";

    private void Start()
    {
        if (playerHealth == null)
            playerHealth = FindFirstObjectByType<PlayerHealth>();

        playerHealth.OnDeath += OnPlayerDeath;
    }

    private void OnDestroy()
    {
        if (playerHealth != null)
            playerHealth.OnDeath -= OnPlayerDeath;
    }

    private void OnPlayerDeath()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(gameOverSceneName);
    }
}
