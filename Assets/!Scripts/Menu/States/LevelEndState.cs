using System;
using UnityEngine;

public class LevelEndState : IMonoState
{
    // state that shows game over or win screen and allows restart
    MenuStateController controller;
    LevelEndStateData data;

    public LevelEndState(MenuStateController controller, LevelEndStateData data)
    {
        this.controller = controller;
        this.data = data;
    }

    public bool IsAlreadyTriggered { get; private set; }

    public void OnEnable(Action OnEnableCompleted = null)
    {   
        InitListeners();
        PrepareStartup();
        OnEnableCompleted?.Invoke();
    }

    void InitListeners()
    {
        data.btn_playAgain.onClick.AddListener(OnGameRestart);
    }

    void PrepareStartup()
    {
        data.cgMain.alpha = 1.0f;
        data.cgMain.interactable = data.cgMain.blocksRaycasts = true;
        Cursor.lockState = CursorLockMode.Confined;

        UpdateStats();
    }

    void UpdateStats()
    {
        data.title?.SetText(data.gameOverType == GameOverType.WIN ? "Well Played!!!" : "Game Over");
        data.ScoreText?.SetText($"Score : {data.score}");
    }


    public void Start(Action OnStartCompleted = null)
    {
        IsAlreadyTriggered = true;
        OnStartCompleted?.Invoke();
    }

    void OnGameRestart()
    {
        controller.InitiateStateChange(typeof(GameplayState));
    }

    void OnExit()
    {
        data.cgMain.alpha = 0.0f;
        data.cgMain.interactable = data.cgMain.blocksRaycasts = false;
    }

    void DeInitListeners()
    {
        data.btn_playAgain.onClick.RemoveListener(OnGameRestart);
    }

    public void OnDisable(Action OnDisableCompleted = null)
    {
        DeInitListeners();
        OnExit();
        OnDisableCompleted?.Invoke();
    }
}
