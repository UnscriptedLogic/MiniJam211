using TMPro;
using UnityEngine;

public class HUDScript : MonoBehaviour
{
    [SerializeField] private float gameTimer = 60f;

    [SerializeField] private GameObject HUDWidget;
    [SerializeField] private TextMeshProUGUI timerText;
    // Update is called once per frame

    
    void Start()
    {
        
    }

    void Update()
    {
        gameTimer -= Time.deltaTime;
        UpdateTimerText();

    }

    private void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(gameTimer / 60);
        int seconds = Mathf.FloorToInt(gameTimer % 60);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }
}
