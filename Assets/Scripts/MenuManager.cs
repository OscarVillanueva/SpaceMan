using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{

    [SerializeField] private Canvas inGameCanvas;
    [SerializeField] private Canvas gameOverCanvas;

    public static MenuManager sharedInstance;

    private void Awake()
    {
        if (sharedInstance == null) sharedInstance = this;
    }

    private void Start()
    {
        inGameCanvas.enabled = false;
        gameOverCanvas.enabled = false;
    }

    public void ShowGameOverMenu(bool enabled)
    {
        gameOverCanvas.enabled = enabled;
        inGameCanvas.enabled = !enabled;
    }

    public void ExitGame()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
