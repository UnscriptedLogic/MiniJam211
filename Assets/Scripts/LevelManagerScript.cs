using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LevelManagerScript : MonoBehaviour
{
    
    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            RestartLevel();
        }
    }

    void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
