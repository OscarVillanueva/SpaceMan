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

    // LayerMask es para indicar o taggear ciertos layers es decir que cierto elemento visual pertenece a que grupo
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private LayerMask levelBlock3;

    [SerializeField] private const int SUPER_JUMP_COST = 5;
    [SerializeField] private const float SUPER_JUMP_FORCE = 1.5f;
    [SerializeField] private float jumpRaycastDistance = 1.5f;

    public float speed = 6.0f;

    private Rigidbody2D rigidBody;
    private Animator animator;
    private Vector3 startPosition;
    private float horizontal;
    private int healhPoints;
    private int manaPoints;
    private float lastDistanceTravelled;

    private const string STATE = "State";

    private const int INITIAL_HEALTH = 100;
    private const int INITIAL_MANA = 15;

    public const int MAX_HEALTH = 200;
    public const int MAX_MANA = 30;
    public const int MIN_HEALTH = 10;
    public const int MIN_MANA = 0;

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

        healhPoints = INITIAL_HEALTH;
        manaPoints = INITIAL_MANA;
    }

    // Update is called once per frame
    void Update()
    {
        // Si no estamos en partida no permitimos el movimiento del jugador
        if (GameManager.sharedInstance.currentGameState != GameState.inGame) return;

        if (IsTouchingTheGround() && Input.GetButtonDown("Jump"))
        {
            Jump(false);
        }

        if (IsTouchingTheGround() && Input.GetButtonDown("SuperJump") && manaPoints >= SUPER_JUMP_COST)
        {
            manaPoints = manaPoints - SUPER_JUMP_COST;
            Jump(true);
        }

        // Dibujamos el Gizmo para poder ver el raycast para mostrarlo en modo de depuración/Dev
        Debug.DrawRay(transform.position, Vector2.down * jumpRaycastDistance, Color.red);
        
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

        healhPoints = INITIAL_HEALTH;
        manaPoints = INITIAL_MANA;

        Invoke(nameof(RestartPosition), 0.2f);
    }

    private void RestartPosition()
    {
        transform.position = startPosition;
        rigidBody.velocity = Vector2.zero;

        GameObject cameraFollow = GameObject.Find("Main Camera");
        cameraFollow.GetComponent<CameraFollow>().ResetCameraPosition();
    }

    public void Jump(bool superJump = false)
    {

        float jumpForceFactor = jumpForce;

        if (superJump) jumpForceFactor = jumpForceFactor * SUPER_JUMP_FORCE;

        GetComponent<AudioSource>().Play();

        // ForceMode2D.Impulse es impulso que ocurre en un determinado momento
        // Force es una fuerza constante
        rigidBody.AddForce(Vector2.up * jumpForceFactor, ForceMode2D.Impulse);
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
        if (Physics2D.Raycast(transform.position, Vector2.down, jumpRaycastDistance, groundMask))
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

        float travelledDistance = GetTravelledDistance();
        float previousMaxDistance = PlayerPrefs.GetFloat("maxScore", 0.0f);

        if (travelledDistance > previousMaxDistance) PlayerPrefs.SetFloat("maxScore", travelledDistance);

        animator.SetFloat(STATE, 1.0f);
        GameManager.sharedInstance.GameOver();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verificamos que hayamos chocado con un LevelBlock3
        bool flag = (levelBlock3.value & (1 << collision.transform.gameObject.layer)) > 0;

        if (flag)
        {

            CameraFollow cameraFollow = FindObjectOfType<CameraFollow>();
            Dictionary<string, float> breaks = collision.GetComponentInParent<LevelBlock>().breaks;

            if (collision.name == "EndPoint") cameraFollow.ResetCameraPosition();

            float newOffsetY = TryToGetABreakPoint(breaks, collision.name);

            if (breaks != null && newOffsetY != 0)
                cameraFollow.ChangeCameraOffset(new(-6.0f, newOffsetY, -10.0f));

        }
    }

    private float TryToGetABreakPoint(Dictionary<string, float> keyValues, string key)
    {
        try
        {
            float value = keyValues[key];
            return value;
        }
        catch
        {
            return 0.0f;
        }
    }

    public void CollectHealth(int points)
    {
        healhPoints = healhPoints + points;

        if (healhPoints >= MAX_HEALTH) healhPoints = MAX_HEALTH;

        if (healhPoints <= 0) Die();

    }

    public void CollectMana(int points)
    {
        manaPoints = manaPoints + points;

        if (manaPoints >= MAX_MANA) manaPoints = MAX_MANA;

    }

    public int GetHealth()
    {
        return healhPoints;
    }

    public int GetMana()
    {
        return manaPoints;
    }

    public float GetTravelledDistance()
    {
        float currentDistance = transform.position.x - startPosition.x;

        lastDistanceTravelled = Mathf.Max(currentDistance, lastDistanceTravelled);

        return lastDistanceTravelled;
    }

    public void ChangeAnimationState(float state)
    {
        animator.SetFloat(STATE, state);
    }
}
