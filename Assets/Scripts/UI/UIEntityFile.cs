using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIEntityFile : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI TMP_text;
    [SerializeField] private Slider slider;
    [SerializeField] private Image icon;
    [SerializeField] private CanvasGroup canvas;
    
    private Action<Action<float>> subscribeToValueChanged;
    private Action<Action<float>> unsubscribeFromValueChanged;

    public CanvasGroup Canvas => canvas;
    
    public void Initialize(string name, float value, Sprite iconSprite)
    {
        TMP_text.text = name;
        slider.value = value;
        icon.sprite = iconSprite;
    }

    public void SetValue(float value)
    {
        slider.value = value;
    }
    
    public void BindValueToEvent(Action<Action<float>> subscribe, Action<Action<float>> unsubscribe)
    {
        subscribeToValueChanged = subscribe;
        unsubscribeFromValueChanged = unsubscribe;
        subscribeToValueChanged?.Invoke(SetValue);
    }

    private void OnDestroy()
    {
        unsubscribeFromValueChanged?.Invoke(SetValue);
    }
}
