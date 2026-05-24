using System;
using DefaultNamespace.FunctionLibraries;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuPlayer : MonoBehaviour
{
    [SerializeField] private UIMainMenuWidget mainMenuWidgetPrefab;
    [SerializeField] private UIFadeTransition fadeTransitionPrefab;
    
    private UIMainMenuWidget mainMenuWidget;
    private UIFadeTransition fadeTransitionWidget;
    
    private void Start()
    {
        FunctionLib.ResetDoOnce(this, "PlayPressed");
        FunctionLib.ResetDoOnce(this, "QuitPressed");
        
        mainMenuWidget = Instantiate(mainMenuWidgetPrefab);
        fadeTransitionWidget = Instantiate(fadeTransitionPrefab);
        fadeTransitionWidget.FadeOut();
        
        mainMenuWidget.OnPlayPressed += OnPlayPressed;
        mainMenuWidget.OnQuitPressed += OnQuitPressed;
    }

    private void OnPlayPressed(object sender, EventArgs e)
    {
        if (!FunctionLib.DoOnce(this, "PlayPressed")) return;
        
        fadeTransitionWidget.FadeIn().onComplete += () => SceneManager.LoadScene("Level_1");
    }
    
    private void OnQuitPressed(object sender, EventArgs e)
    {
        if (!FunctionLib.DoOnce(this, "QuitPressed")) return;
        
        fadeTransitionWidget.FadeIn().onComplete += () => Application.Quit();
    }
}
