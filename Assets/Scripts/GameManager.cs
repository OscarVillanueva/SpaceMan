using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameState currentGameState;

    public static GameManager sharedInstance;

    private PlayerController playerController;

    public int collectedObjects = 0;

    private void Awake()
    {
        if (sharedInstance == null) sharedInstance = this;
    }

    private void Start()
    {
        StartGame();
        playerController = GameObject.FindObjectOfType<PlayerController>();
    }

    public void StartGame()
    {
        SetGameState(GameState.inGame);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void GameOver()
    {
        SetGameState(GameState.gameOver);
    }

    public void BackToMenu()
    {
        SetGameState(GameState.menu);
    }

    private void SetGameState(GameState newGameState)
    {

        switch (newGameState)
        {
            case GameState.menu:
                currentGameState = GameState.menu;
                Time.timeScale = 0.0f;
                break;

            case GameState.inGame:
                Time.timeScale = 1.0f;
                currentGameState = GameState.inGame;
                LevelManager.sharedInstance.ClearLevelBlocks();
                MenuManager.sharedInstance.ShowGameOverMenu(false);
                Invoke(nameof(ReloadLevel), 0.1f);
                break;

            case GameState.gameOver:
                currentGameState = GameState.gameOver;
                Invoke(nameof(StopTime), 1.0f);
                break;
        }

        currentGameState = newGameState;

    }

    private void ReloadLevel()
    {
        CameraFollow cameraFollow = FindObjectOfType<CameraFollow>();
        if (cameraFollow) cameraFollow.ResetOffest();

        LevelManager.sharedInstance.GenerateInitialBlocks();
        playerController.StartGame();
    }

    private void StopTime()
    {
        MenuManager.sharedInstance.ShowGameOverMenu(true);
        Time.timeScale = 0.0f;
    }

    public void CollectObject(CollectableController collectable)
    {
        collectedObjects = collectedObjects + collectable.value;
    }
}
