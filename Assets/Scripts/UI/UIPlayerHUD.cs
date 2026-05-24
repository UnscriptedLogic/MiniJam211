using System.Collections.Generic;
using Components;
using DG.Tweening;
using Pathfinding;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerHUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private Image timerRadial;

    [SerializeField] private RectTransform entityContainer;
    [SerializeField] private UIEntityFile entityFilePrefab;
    
    private List<UIEntityFile> entityFiles = new List<UIEntityFile>();
    
    public void Initialize(Player player, LevelManagerScript levelManager)
    {
        levelManager.OnTimeRemainingSecondChanged += UpdateTimer;
        UpdateTimer((int)levelManager.TimeRemaining);

        for (int i = 0; i < levelManager.Entities.Count; i++)
        {
            UIEntityFile entityFile = Instantiate(entityFilePrefab, entityContainer);
            AttentionComponent attentionComponent = levelManager.Entities[i].GetComponent<AttentionComponent>();

            if (attentionComponent == null) continue;
            
            entityFile.Initialize(levelManager.Entities[i].EntityDetails.Name, attentionComponent.CurrentAttentionValue,levelManager.Entities[i].EntityDetails.Icon);
            entityFile.BindValueToEvent(
                handler => attentionComponent.OnAttentionChanged += handler,
                handler => attentionComponent.OnAttentionChanged -= handler);
            
            entityFiles.Add(entityFile);
        }
    }

    private void UpdateTimer(int value)
    {
        int minutes = Mathf.FloorToInt(value / 60);
        int seconds = Mathf.FloorToInt(value % 60);
        timerText.text = $"{minutes:00}:{seconds:00}";
        
        timerRadial.DOFillAmount(value / 60f, 0.5f).SetEase(Ease.OutExpo);
        
        float threshold = 20f;
        if (value <= threshold)
        {
            timerRadial.color = Color.Lerp(Color.red, Color.white,value / 20f);
        }
    }
}
