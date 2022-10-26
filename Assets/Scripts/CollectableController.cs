using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableController : MonoBehaviour
{
    [SerializeField] private CollectableType collectableType = CollectableType.coin;
    public int value = 1;

    private SpriteRenderer spriteRenderer;
    private CircleCollider2D itemCollider;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        itemCollider = GetComponent<CircleCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Collect();
        }
    }


    private void Collect()
    {
        switch (collectableType)
        {
            case CollectableType.coin:
                GameManager.sharedInstance.CollectObject(this);
                break;
            case CollectableType.healthPotion:
                break;
            case CollectableType.manaPotion:
                break;
        }

        Destroy(gameObject, 0.1f);
    }
}
