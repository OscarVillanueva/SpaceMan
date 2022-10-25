using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameState currentGameState;

    public static GameManager sharedInstance;

    private PlayerController playerController;

    private void Awake()
    {
        if (sharedInstance == null) sharedInstance = this;
    }

    private void Start()
    {
        SetGameState(GameState.menu);
        playerController = GameObject.FindObjectOfType<PlayerController>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Submit") && currentGameState != GameState.inGame)
        {
            StartGame();
        }
    }

    public void StartGame()
    {
        SetGameState(GameState.inGame);
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
                currentGameState = GameState.inGame;
                playerController.StartGame();
                Time.timeScale = 1.0f;
                break;

            case GameState.gameOver:
                currentGameState = GameState.gameOver;
                break;
        }

        currentGameState = newGameState;

    }

}