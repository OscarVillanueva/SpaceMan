using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigRockController : MonoBehaviour
{
    [SerializeField] private float wiggleRate = 0.5f;
    [SerializeField] private float movingSpace = 5f;

    private Rigidbody2D rigidBody;

    // Start is called before the first frame update
    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(GettingToFall());
        }
    }

    IEnumerator GettingToFall()
    {
        yield return new WaitForSeconds(0.5f);

        int times = 10;
        float orginalX = transform.position.x;

        while (times > 0)
        {
            transform.position = new Vector2(orginalX - movingSpace, transform.position.y);
            yield return new WaitForSeconds(times * wiggleRate);
            transform.position = new Vector2(orginalX, transform.position.y);
            yield return new WaitForSeconds(times * wiggleRate);
            transform.position = new Vector2(orginalX + movingSpace, transform.position.y);
            yield return new WaitForSeconds(times * wiggleRate);

            times = times - 1;
        }

        rigidBody.gravityScale = 0.96f;
        rigidBody.constraints &= ~RigidbodyConstraints2D.FreezePositionY;
    }

}
