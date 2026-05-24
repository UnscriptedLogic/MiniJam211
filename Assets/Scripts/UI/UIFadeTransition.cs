using System;
using DG.Tweening;
using UnityEngine;

public class UIFadeTransition : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;

    public Tween fadeInTween;
    public Tween fadeOutTween;
    
    public Tween FadeIn(float duration = 0.25f)
    {
        canvasGroup.alpha = 0;

        fadeInTween = canvasGroup.DOFade(1, duration).SetDelay(1).SetEase(Ease.Linear);

        return fadeInTween;
    }
    
    public Tween FadeOut(float duration = 0.25f)
    {
        canvasGroup.alpha = 1;

        fadeOutTween = canvasGroup.DOFade(0, duration).SetDelay(1).SetEase(Ease.Linear);

        return fadeOutTween;
    }
}
