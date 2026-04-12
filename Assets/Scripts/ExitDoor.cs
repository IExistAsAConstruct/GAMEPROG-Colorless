using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitDoor : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private string winSceneName = "VictoryScreen";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SceneManager.LoadScene(winSceneName);
        }
    }
}
