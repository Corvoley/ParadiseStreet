using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Action OnPackageCollected;
    public Action OnEnemyCollision;
    public Action OnPowerUpCollected;


    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private float movementSpeed;

    [SerializeField] public SpriteRenderer groundSprite;

    [SerializeField] private GameObject indicationArrow;
    [SerializeField] private GameObject hitParticlePrafab;
    [SerializeField] private Transform hitTransform;



    private Vector2 inputVector;
    private PlayerInputActions playerInputActions;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        playerInputActions.Player.Pause.performed += Pause_performed;
    }

    private void Pause_performed(InputAction.CallbackContext obj)
    {
        GameManager.Instance.PauseGame();       
    }


    // Update is called once per frame
    void Update()
    {
        InputHandler();
        IndicationArrowHandler();
    }
    private void FixedUpdate()
    {
        Movement();
    }

    private void Movement()
    {
        Vector2 boundsPos = new Vector2(
            Mathf.Clamp(rb.position.x, -groundSprite.sprite.bounds.size.x / 2 * groundSprite.transform.localScale.x, groundSprite.sprite.bounds.size.x / 2 * groundSprite.transform.localScale.x),
            Mathf.Clamp(rb.position.y, -groundSprite.sprite.bounds.size.y / 2 * groundSprite.transform.localScale.y, groundSprite.sprite.bounds.size.y / 2 * groundSprite.transform.localScale.y));

        rb.position = boundsPos;
        Vector2 force = new Vector2(movementSpeed * inputVector.x, movementSpeed * inputVector.y);


        rb.AddForce(force);

    }
    private void InputHandler()
    {
        inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();
    }
    private void IndicationArrowHandler()
    {
        if (GameManager.CurrentPackage.transform != null)
        {
            if (!GameManager.CurrentPackage.GetComponent<SpriteRenderer>().isVisible)
            {
                indicationArrow.gameObject.SetActive(true);
                indicationArrow.transform.up = GameManager.CurrentPackage.transform.position - indicationArrow.transform.position;

            }
            else
            {
                indicationArrow.gameObject.SetActive(false);
            }
        }

    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Package"))
        {
            OnPackageCollected?.Invoke();
            Destroy(collision.gameObject);
        }
        if (collision.CompareTag("Enemy"))
        {
            OnEnemyCollision?.Invoke();
            if (GameManager.Instance.canBeDamaged)
            {
                SpawnHitParticle(collision.transform);
            }

        }
        if (collision.CompareTag("PowerUp"))
        {
            OnPowerUpCollected?.Invoke();
            collision.gameObject.GetComponent<PowerUp>().ActivatePowerUpBehaviour(this);
            Destroy(collision.gameObject);

        }
    }

    private void SpawnHitParticle(Transform enemyTransform)
    {
        var particle = Instantiate(hitParticlePrafab, hitTransform);
        hitTransform.up = enemyTransform.position - hitTransform.transform.position;

    }

    private void OnDestroy()
    {
        playerInputActions.Player.Disable();
        playerInputActions.Player.Pause.performed -= Pause_performed;
    }
}
