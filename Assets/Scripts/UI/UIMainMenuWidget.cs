using System;
using UnityEngine;

public class UIMainMenuWidget : MonoBehaviour
{
    [SerializeField] private UIButton playBtn;
    [SerializeField] private UIButton quitBtn;
    
    public event EventHandler OnPlayPressed;
    public event EventHandler OnQuitPressed;

    private void Start()
    {
        playBtn.Button.onClick.AddListener(() => OnPlayPressed?.Invoke(this, EventArgs.Empty));
        quitBtn.Button.onClick.AddListener(() => OnQuitPressed?.Invoke(this, EventArgs.Empty));
    }
}
