using DefaultNamespace;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIInteractWidget : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI TMP_text;
    [SerializeField] private float animationDuration = 0.25f;
    
    [Header("Components")]
    [SerializeField] private CanvasGroup canvas;
    [SerializeField] private RectTransform root;
    
    public void ShowInteract(string key, IInteractable interactable)
    {
        TMP_text.text = $"Press <b>{key.ToUpper()}</b> to {interactable.GetActionName()}";
        
        SetVisible(true);
    }
    
    public void SetVisible(bool visible)
    {
        gameObject.SetActive(visible);
        
        if (visible)
        {
            AnimateIn();
        }
        else
        {
            AnimateOut();
        }
    }

    #region Animations

    private void AnimateIn()
    {
        canvas.alpha = 0;
        root.localPosition = new Vector3(0, -5, 0);
        
        canvas.DOFade(1, animationDuration).SetEase(Ease.Linear);
        root.DOLocalMoveY(0, animationDuration).SetEase(Ease.OutExpo);
        
    }
    
    private void AnimateOut()
    {
        canvas.alpha = 1;
        
        canvas.DOFade(0, animationDuration).SetEase(Ease.Linear);
        root.DOLocalMoveY(-5, animationDuration).SetEase(Ease.InExpo);
        
    }

    #endregion
}
