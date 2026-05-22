using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIInteractWidget : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI TMP_text;
    
    public void ShowInteract(string key, IInteractable interactable)
    {
        TMP_text.text = $"Press <b>{key.ToUpper()}</b> to {interactable.GetActionName()}";
        
        SetVisible(true);
    }
    
    public void SetVisible(bool visible)
    {
        gameObject.SetActive(visible);
    }
}
