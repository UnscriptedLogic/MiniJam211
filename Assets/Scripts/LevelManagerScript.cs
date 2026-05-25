using System;
using System.Collections.Generic;
using System.Linq;
using Components;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LevelManagerScript : MonoBehaviour
{
    //Timer Stuff
    [SerializeField] private float timeRemaining = 60f;
    private int lastReportedSecond;

    public event Action<int> OnTimeRemainingSecondChanged;
    
    public float TimeRemaining => timeRemaining;

    //Player Stuff
    [SerializeField] private Player playerPrefab;
    private Player player;
    
    //Entity Thingies
    private List<Entity> entities = new List<Entity>();
    
    public List<Entity> Entities => entities;

    public int levelIndex;


    private void Start()
    {
        if (SceneManager.GetActiveScene() != null)
        {
            levelIndex = SceneManager.GetActiveScene().buildIndex;
        }

        InitializeTimer();
        
        entities = FindObjectsByType<Entity>().ToList();
        
        player = Instantiate(playerPrefab);
        player.Initialize(this);
    }

    void Update()
    {
        TickTimer();

        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            RestartLevel();
        }

    }

    private void TickTimer()
    {
        if (timeRemaining <= 0f)
        {
            LevelWon();
            return;
        }
        timeRemaining = Mathf.Max(0f, timeRemaining - Time.deltaTime);
        int currentSecond = Mathf.CeilToInt(timeRemaining);

        if (currentSecond != lastReportedSecond)
        {
            lastReportedSecond = currentSecond;
            OnTimeRemainingSecondChanged?.Invoke(currentSecond);
        }
    }

    private void InitializeTimer()
    {
        timeRemaining = Mathf.Max(0f, timeRemaining);
        lastReportedSecond = Mathf.CeilToInt(timeRemaining);
    }
    
    #region Level Loading and Management

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
    
    public void LevelLost()
    {
        SceneManager.LoadScene("Level_Lost"); 
    }

    public void LevelWon()
    {
        SceneManager.LoadScene("Level_Victory");
    }
    #endregion
}
