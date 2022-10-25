using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Estado de animaciones
 * 0 -> idle
 * 0.2 -> running
 * 0.4 -> StartingJump
 * 0.6 -> HighstPoint
 * 0.8 -> Falling
 * 1 -> died
 */

public class PlayerController : MonoBehaviour
{

    [SerializeField] private float jumpForce = 6.0f;
    [SerializeField] private float speed = 6.0f;

    // LayerMask es para indicar o taggear ciertos layers es decir que cierto elemento visual pertenece a que grupo
    [SerializeField] private LayerMask groundMask;

    private Rigidbody2D rigidBody;
    private Animator animator;
    private Vector3 startPosition;
    private float horizontal;

    private const string STATE = "State";

    // Start is called before the first frame update
    void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        animator.SetFloat(STATE, 0);

        // Guardamos la posición original
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Si no estamos en partida no permitimos el movimiento del jugador
        if (GameManager.sharedInstance.currentGameState != GameState.inGame) return;

        if (IsTouchingTheGround() && Input.GetButtonDown("Jump"))
        {
            Jump();
        }

        // Dibujamos el Gizmo para poder ver el raycast para mostrarlo en modo de depuración/Dev
        Debug.DrawRay(transform.position, Vector2.down * 1.5f, Color.red);
        
    }

    private void FixedUpdate()
    {
        // Si no estamos en partida no permitimos el movimiento del jugador
        if (GameManager.sharedInstance.currentGameState != GameState.inGame) return;

        MovePlayer();

        // Flipeamos al player según sea el caso
        if (horizontal < 0.0f) transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        else if (horizontal > 0.0f) transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        // Setteamos la animación
        if (horizontal != 0 && rigidBody.velocity.y == 0) animator.SetFloat(STATE, 0.2f);
        else SetJumpingAnimation(rigidBody.velocity.y);
    }


    public void StartGame()
    {

        animator.SetFloat(STATE, 0);

        Invoke(nameof(RestartPosition), 0.2f);
    }

    private void RestartPosition()
    {
        transform.position = startPosition;
        rigidBody.velocity = Vector2.zero;
    }

    private void Jump()
    {
        // ForceMode2D.Impulse es impulso que ocurre en un determinado momento
        // Force es una fuerza constante
        rigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    private void MovePlayer()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        Vector2 moveDirection = new Vector2(horizontal, 0).normalized;
        rigidBody.velocity = new Vector2(moveDirection.x * speed, rigidBody.velocity.y);
    }

    // Indica si el player esta tocando el suelo
    private bool IsTouchingTheGround()
    {
        // Dibuja el rayo desde el centro del objeto
        // Desde donde esta el player traza un rayo imaginario hacia abajo con una distancia maxima de 20cm 
        // y esta tocando el suelo?
        if (Physics2D.Raycast(transform.position, Vector2.down, 1.5f, groundMask))
        {
            // Reactivamos la animación
            //animator.enabled = false;
            return true;
        }

        //animator.enabled = true;
        return false;
    }

    private void SetJumpingAnimation(float velocity)
    {
        float value = 0;

        if ( velocity != 0 )
        {
            if (velocity > 0.3f) value = 0.4f; // comenzando el salto
            if (velocity <= 0.3 && velocity > -3f) value = 0.6f; // punto más alto
            if (velocity < -3) value = 0.8f; // cayendo
        }


        animator.SetFloat(STATE, value);
    }

    public void Die()
    {
        animator.SetFloat(STATE, 1.0f);
        GameManager.sharedInstance.GameOver();
        //Destroy(gameObject, 0.5f);
    }
}
