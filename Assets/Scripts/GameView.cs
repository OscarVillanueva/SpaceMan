using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameView : MonoBehaviour
{

    [SerializeField] private TMP_Text coinsText;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text maxScoreText;

    private void Update()
    {
        if (GameManager.sharedInstance.currentGameState == GameState.inGame)
        {
            int coins = GameManager.sharedInstance.collectedObjects;
            float score = 0.0f;
            float maxScore = 0.0f;

            coinsText.text = coins.ToString();

            // f1 para cortar el float a un solo decimal
            scoreText.text = score.ToString("f1");
            maxScoreText.text = maxScore.ToString("f1");
        }
    }
}
