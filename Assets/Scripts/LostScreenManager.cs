using UnityEngine;
using UnityEngine.SceneManagement;

public class LostScreenManager : MonoBehaviour
{
    [SerializeField] private LevelManagerScript levelManager;
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(levelManager.levelIndex);
    }
}
