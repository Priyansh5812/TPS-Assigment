using System;
using UnityEngine;

public class MainMenuState : IMonoState
{
    // state that shows the main menu ui and handles start button
    MenuStateController controller;
    MainMenuStateData data;
    public MainMenuState(MenuStateController controller, MainMenuStateData data)
    { 
        this.controller = controller;
        this.data = data;
    }

    public bool IsAlreadyTriggered { get; private set; }

    public void OnEnable(Action OnEnableCompleted = null)
    {   
        InitListeners();
        OnEnableCompleted?.Invoke();
    }

    public void Start(Action OnStartCompleted = null)
    {
        IsAlreadyTriggered = true;
        PrepareStartup();
        OnStartCompleted?.Invoke();
    }


    void InitListeners()
    {
        data.btn_start.onClick.AddListener(InitiatePlay);
    }

    void PrepareStartup()
    {
        data.cgMain.alpha = 1.0f;
        data.cgMain.interactable = data.cgMain.blocksRaycasts = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    void InitiatePlay()
    {
        controller.InitiateStateChange(typeof(GameplayState));    
    }

    void OnExit()
    {
        data.cgMain.alpha = 0f;
        data.cgMain.interactable = data.cgMain.blocksRaycasts = false;
    }

    void DeInitListeners()
    {
        data.btn_start.onClick.RemoveListener(InitiatePlay);
    }

    public void OnDisable(Action OnDisableCompleted = null)
    {
        OnExit();
        DeInitListeners();
        OnDisableCompleted?.Invoke();
    }
}
