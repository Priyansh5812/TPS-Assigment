using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuCallbacksHandler : MonoBehaviour
{
    [SerializeField] GameObject gameplayScreen, gameOverScreen,MenuGameScreen;
    [SerializeField] TextMeshProUGUI gameOverMessage;
    [SerializeField] Button btn_playAgain;

    private void OnEnable()
    {
        EventManager.OnGameOver.AddListener(HandleGameOver);
        btn_playAgain.onClick.AddListener(HandlePlayAgain);
    }

    private void Start()
    {
        PrepareStartup();
    }

    void PrepareStartup()
    {
        MenuGameScreen.gameObject.SetActive(true);
        gameplayScreen.SetActive(false);
        gameOverScreen.SetActive(false);
        UnlockMouse();
    }

    void HandleGameOver(GameOverType type , int enemiesRemaining)
    {
        gameplayScreen.SetActive(false);
        gameOverScreen.SetActive(true);
        string str = type == 0 ? "Good Job! you shot down all enemies" : $"You Died \n Remaining Enemies : {enemiesRemaining}";
        gameOverMessage?.SetText(str);
        UnlockMouse();
    }

    void HandlePlayAgain()
    {
        gameplayScreen.SetActive(true);
        gameOverScreen.SetActive(false);
        EventManager.OnGameRestarted.Invoke();
        LockMouse();
    }

    public void LockMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void UnlockMouse()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    private void OnDisable()
    {
        EventManager.OnGameOver.RemoveListener(HandleGameOver);

        btn_playAgain.onClick.RemoveListener(HandlePlayAgain);
    }

}
