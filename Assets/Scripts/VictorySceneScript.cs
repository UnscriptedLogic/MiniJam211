using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictorySceneScript : MonoBehaviour
{
    [SerializeField] private LevelManagerScript levelManager;
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(levelManager.levelIndex + 1);
    }
}
