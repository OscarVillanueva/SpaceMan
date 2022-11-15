using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOverCanvasController : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;

    public void SetScore(int score)
    {
        scoreText.text = "Score: " + score;
    }
}
