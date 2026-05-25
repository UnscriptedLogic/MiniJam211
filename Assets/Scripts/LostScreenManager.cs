using UnityEngine;
using UnityEngine.SceneManagement;

public class LostScreenManager : MonoBehaviour
{
    public int levelDied;
    [SerializeField] private LevelManagerScript levelManager;
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(levelDied);
    }
}
