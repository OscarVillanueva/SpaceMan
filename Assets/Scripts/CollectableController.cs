using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableController : MonoBehaviour
{
    [SerializeField] private CollectableType collectableType = CollectableType.coin;
    public int value = 1;

    private PlayerController player;

    private void Start()
    {
        player = GameObject.FindObjectOfType<PlayerController>();
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

                if (player)
                {
                    player.CollectHealth(this.value);
                }

                break;

            case CollectableType.manaPotion:

                if (player)
                {
                    player.CollectMana(this.value);
                }

                break;
        }

        Destroy(gameObject, 0.1f);
    }
}
