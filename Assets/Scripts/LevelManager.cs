using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    public static LevelManager sharedInstance;

    [SerializeField] private List<LevelBlock> levelBlocks;
    [SerializeField] private Transform levelStartPosition; 

    private List<LevelBlock> currentLevelBlocks;

    private void Awake()
    {
        if (sharedInstance == null)
        {
            sharedInstance = this;
            currentLevelBlocks = new List<LevelBlock>();

        }
    }
    private void Start()
    {
        // GenerateInitialBlocks();
    }

    public void AddLevelBlock()
    {
        int randomIndex = Random.Range(0, levelBlocks.Count);

        LevelBlock block;
        Vector3 spawnPostion;

        if (currentLevelBlocks.Count == 0)
        {
            block = Instantiate(levelBlocks[0]);
            spawnPostion = levelStartPosition.position;
        }
        else
        {
            block = Instantiate(levelBlocks[randomIndex]);
            spawnPostion = currentLevelBlocks[currentLevelBlocks.Count - 1].endPoint.position;
        }

        // Agregamos al bloque el level manager como hijo, false para evitar que los hijos mantengan las transformaciones del padre
        block.transform.SetParent(this.transform, false);

        // Unimos el punto de final del bloque anterior con el inicio del nuevo bloque
        Vector3 correction = spawnPostion - block.startPoint.position;

        block.transform.position = correction;
        currentLevelBlocks.Add(block);
    }

    public void RemoveLevelBlock()
    {
        LevelBlock oldBlock = currentLevelBlocks[0];
        currentLevelBlocks.Remove(oldBlock);
        Destroy(oldBlock.gameObject, 0.5f);
    }

    public void ClearLevelBlocks()
    {
        while (currentLevelBlocks.Count > 0)
        {
            RemoveLevelBlock();
        }
    }

    public void GenerateInitialBlocks()
    {
        for (int i = 0; i < 2; i++)
        {
            AddLevelBlock();
        }
    }
}
