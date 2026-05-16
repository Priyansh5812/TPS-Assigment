using TMPro;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class MainMenuStateData
{
    public CanvasGroup cgMain;
    public Button btn_start;
}


[System.Serializable]
public class GameplayStateData
{
    public CanvasGroup cgMain;  
    public Image healthFill;
    public TextMeshProUGUI wavePrompt;
    public TextMeshProUGUI remainingEnemies;
    public float wavePromptDuration;
}


[System.Serializable]
public class LevelEndStateData
{
    public CanvasGroup cgMain;
    public Button btn_playAgain;
    public TextMeshProUGUI title;
    public TextMeshProUGUI ScoreText;
    [HideInInspector] public GameOverType gameOverType;
    [HideInInspector] public int score; 
}