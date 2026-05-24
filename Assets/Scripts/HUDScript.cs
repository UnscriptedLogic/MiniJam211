using Pathfinding;
using TMPro;
using UnityEngine;

public class HUDScript : MonoBehaviour
{
    [SerializeField] private float gameTimer = 60f;

    [SerializeField] private GameObject HUDWidget;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI npcCountText;
    // Update is called once per frame


    void Start()
    {
        gameTimer = 60f;
    }

    void Update()
    {
        gameTimer -= Time.deltaTime;
        UpdateTimerText();

        if (gameTimer <= 0f)
        {
            gameTimer = 0f;
            // Trigger
            HUDWidget.SetActive(false);
        }
    }

    private void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(gameTimer / 60);
        int seconds = Mathf.FloorToInt(gameTimer % 60);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    public void UpdateNPCCountUI(int count)
    {
        npcCountText.text = $"Dino's Alive: {count}";
    }
}
