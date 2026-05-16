using System.Collections.Generic;
using UnityEngine;
using System;
public class MenuStateController : MonoBehaviour
{   
    // menu controller that switches between menu states and orchestrates ui flow
    [SerializeField] MainMenuStateData mainMenuStateData;
    [SerializeField] GameplayStateData gameplayStateData;
    [SerializeField] LevelEndStateData levelEndStateData;


    readonly Dictionary<Type, IMonoState> stateReg = new();
    IMonoState currentState;
    bool isChangingState;
    // registry of state instances used by the menu controller
    // current active state instance
    // flag used to prevent overlapping state transitions

   
    private void OnEnable()
    {
        EventManager.OnGameOver.AddListener(HandleGameOver);
    }

    void Start()
    {
        ConstructMenuStates();
        InitiateStateChange(typeof(MainMenuState));
    }

    void ConstructMenuStates()
    {
        stateReg.Add(typeof(MainMenuState), new MainMenuState(this, mainMenuStateData));
        stateReg.Add(typeof(GameplayState), new GameplayState(this, gameplayStateData));
        stateReg.Add(typeof(LevelEndState), new LevelEndState(this, levelEndStateData));
    }

    public void InitiateStateChange(Type newStateType)
    {
        if (isChangingState)
        {
            return;
        }

        IMonoState newState;
        if (stateReg.ContainsKey(newStateType))
            newState = stateReg[newStateType];
        else
        {
            Debug.LogError($"Unknown {newStateType} state type");
            return;
        }

        isChangingState = true;
        if (currentState != null)
            currentState.OnDisable(OnInitialCompleted);
        else
            OnInitialCompleted();


        void OnInitialCompleted()
        {
            currentState = newState;

            if (currentState.IsAlreadyTriggered)
                currentState.OnEnable(OnEnableCompleted);
            else
                currentState.OnEnable(OnEnableThenStart);
        }

        void OnEnableThenStart()
        {
            currentState.Start(OnStartCompleted);
        }

        void OnEnableCompleted()
        {
            isChangingState = false;
        }

        void OnStartCompleted()
        {
            isChangingState = false;
        }
    }

    void HandleGameOver(GameOverType type , int score)
    {
        levelEndStateData.gameOverType = type;
        levelEndStateData.score = score;    
        InitiateStateChange(typeof(LevelEndState));
    }


    void OnDisable()
    {
        EventManager.OnGameOver.RemoveListener(HandleGameOver);
        currentState?.OnDisable();
    }

}
