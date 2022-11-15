using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float speed = 1.5f;
    [SerializeField] private int damage = 10;
    [SerializeField] private int points = 13;
    [SerializeField] private int manaPoints = 5;


    private Rigidbody2D rigidBody;

    private bool facingRight = false;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        float currentSpeed;

        if (facingRight)
        {
            // Mirando hacia la derecha
            currentSpeed = speed;
            this.transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            // Mirando a la derecha
            currentSpeed = -speed;
            this.transform.eulerAngles = Vector3.zero;
        }

        if (GameManager.sharedInstance.currentGameState == GameState.inGame)
        {
            rigidBody.velocity = new Vector2(currentSpeed, rigidBody.velocity.y);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Coin")) return;

        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController>().CollectHealth(damage * -1);
        }

        // si el enemigo no choco con enemigo o moneda giramos
        facingRight = !facingRight;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().Jump();
            collision.gameObject.GetComponent<PlayerController>().CollectMana(manaPoints);

            GameManager.sharedInstance.AddDefeatedEnemy(points);

            gameObject.GetComponent<SpriteRenderer>().flipY = true;
            Destroy(gameObject, 0.3f);
        }
    }

}
