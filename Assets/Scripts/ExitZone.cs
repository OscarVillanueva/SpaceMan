using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) {
            LevelManager.sharedInstance.AddLevelBlock();
            LevelManager.sharedInstance.RemoveLevelBlock();
        }
    }
}