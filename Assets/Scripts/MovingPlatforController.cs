using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatforController : MonoBehaviour
{
    private float horizontal = 0.0f;
    private Collider2D collider2d;
    private List<ContactPoint2D> contacts = new List<ContactPoint2D>();

    // Awake is called before start at object initialize
    void Awake()
    {
        collider2d = GetComponent<Collider2D>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other == null) return;

        // sacamos los contactos que tiene el player
        other.GetContacts(contacts);

        // si no se detectan contactos no hacemos nada
        if (contacts.Count == 0) return;

        if (other.CompareTag("Player"))
        {

            // sacamos el movimiento horizontal para que gire el personaje
            float temp = Input.GetAxisRaw("Horizontal");
            if (temp != 0) horizontal = temp == -1 ? -0.5f : 0.5f;

            // Verificar que el jugador este dentro de los limites de x
            if (IsPlayerOnTop())
            {
                    other.GetComponent<PlayerController>().ChangeAnimationState(0);

                    other.transform.position = new Vector3(
                        transform.position.x + horizontal,
                        other.transform.position.y,
                        other.transform.position.z
                    );
            }

            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Al salir el player de la plataforma reinicionamos el movimiento horizontal
        horizontal = 0.0f;
    }

    private bool IsPlayerOnTop() {
        Vector3 cp = contacts[0].point;

        float xMin = collider2d.bounds.min.x;
        float xMax = collider2d.bounds.max.x;
        float yMin = collider2d.bounds.min.y;
        float yMax = collider2d.bounds.max.y;
        
        // comparamos los contactos con el border
        return (cp.x >= xMin && cp.x <= xMax) && (cp.y >= yMin && cp.y <= yMax);
    }

}
