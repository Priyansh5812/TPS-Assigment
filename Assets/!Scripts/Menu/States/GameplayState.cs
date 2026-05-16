using System;
using System.Collections;
using UnityEngine;

public class GameplayState : IMonoState
{
    // state that displays gameplay ui and updates hud elements
    MenuStateController controller;
    GameplayStateData data;
    WaitForSeconds delay;
    public GameplayState(MenuStateController controller, GameplayStateData data)
    {
        this.controller = controller;
        this.data = data;
        delay = new(data.wavePromptDuration);
    }

    public bool IsAlreadyTriggered { get; private set; }

    public void OnEnable(Action OnEnableCompleted = null)
    {
        InitListeners();
        PrepareStartup();
        OnEnableCompleted?.Invoke();
    }

    public void Start(Action OnStartCompleted = null)
    {
        IsAlreadyTriggered = true;
        data.healthFill.material = new Material(data.healthFill.material);
        OnStartCompleted?.Invoke();
    }

    void InitListeners()
    {
        EventManager.OnPreWaveStarted.AddListener(PromptWave);
        EventManager.OnPlayerDamaged.AddListener(UpdatePlayerHealthView);
        EventManager.OnUpdateEnemyCount.AddListener(UpdateRemainingEnemiesCount);
    }

    void PrepareStartup()
    {
        data.cgMain.alpha = 1.0f;
        data.cgMain.interactable = data.cgMain.blocksRaycasts = true;
        data.wavePrompt.gameObject.SetActive(false);
        UpdatePlayerHealthView(1.0f);
        UpdateRemainingEnemiesCount(0);
        Cursor.lockState = CursorLockMode.Locked;
        EventManager.OnPrepareGame.Invoke();
    }

    void UpdatePlayerHealthView(float value)
    {
        data.healthFill.material.SetFloat("_Factor", value);
    }

    void PromptWave(int wave)
    {
        controller.StartCoroutine(PromptWaveRoutine(wave));
    }

    void UpdateRemainingEnemiesCount(int count)
    {
        data.remainingEnemies?.SetText($"Remaining Enemies : {count}");
    }

    void ProceedToGameOver()
    { 
        
    }


    IEnumerator PromptWaveRoutine(int wave)
    {
        data.wavePrompt.gameObject.SetActive(true);
        yield return null;
        data.wavePrompt?.SetText($"Wave {wave+1}");
        yield return delay;
        data.wavePrompt.gameObject.SetActive(false);
        EventManager.OnWaveStarted.Invoke();
    }

    void OnExit()
    {
        data.cgMain.alpha = 0f;
        data.cgMain.interactable = data.cgMain.blocksRaycasts = false;
    }

    void DeInitListeners()
    {
        EventManager.OnPreWaveStarted.RemoveListener(PromptWave);
        EventManager.OnPlayerDamaged.RemoveListener(UpdatePlayerHealthView);
        EventManager.OnUpdateEnemyCount.RemoveListener(UpdateRemainingEnemiesCount);
    }

    public void OnDisable(Action OnDisableCompleted = null)
    {
        DeInitListeners();
        OnExit();
        OnDisableCompleted?.Invoke();
    }
}
