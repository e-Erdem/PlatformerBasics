using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private InputActionAsset inputActions;

    private InputAction movementAction;
    private InputAction jumpAction;
    private InputAction attack1Action;

    private Rigidbody2D rb;
    private SpriteRenderer spi;
    public Animator animator;
    private Vector2 movement;
    private float speed = 9f;
    private float jumpForce = 5000f;

    public Collider2D legCollider;
    public Collider2D terrainCollider;
    public Transform attackPoint;
    public Transform attackPointLeft;
    private Transform attackPosition;
    public LayerMask enemyLayers;

    public float attackRange = 0.5f;
    private float attackRate = 3f;
    private float nextAttackIn = 0f;
    private int attackDamage = 25;
    private bool isAttackRN = false;

    private void Awake()
    {
        
        rb = GetComponent<Rigidbody2D>();
        spi = GetComponent<SpriteRenderer>();

        inputActions = GetComponent<PlayerInput>().actions;
        movementAction = inputActions.FindAction("Move");
        jumpAction = inputActions.FindAction("Jump");
        attack1Action = inputActions.FindAction("Attack1");

        movementAction.Enable();
        jumpAction.Enable();
        attack1Action.Enable();

        movementAction.performed += OnMovementPerformed;
        movementAction.canceled += OnMovementCanceled;

        jumpAction.performed += OnJumpPerformed;

        attack1Action.performed += OnAttack1Performed;
    }


    private void OnMovementPerformed(InputAction.CallbackContext context)
    {
       // if (isAttackRN == false)
        //{
            //Debug.Log("Move");
            movement = context.ReadValue<Vector2>();
            if (movement.x == -1)
            {
                spi.flipX = true;
            }
            else
            {
                spi.flipX = false;
            }
            animator.SetFloat("Speed", 1);
       // }
    }

    private void OnMovementCanceled(InputAction.CallbackContext context)
    {
        movement = Vector2.zero;
        animator.SetFloat("Speed", 0);
    }

    private void OnJumpPerformed(InputAction.CallbackContext context)
    {
        if(legCollider.IsTouching(terrainCollider))
        {
            animator.SetTrigger("isJump");
            Debug.Log("Jump Performed");
            rb.AddForce(new Vector2(0f, jumpForce));
        }
    }

    private void OnAttack1Performed(InputAction.CallbackContext context)
    {

        if (Time.time >= nextAttackIn)
            {
                if (spi.flipX == true)
                {
                    attackPosition = attackPointLeft;
                }
                else
                {
                    attackPosition = attackPoint;
                }
                //isAttackRN = true;
                animator.SetTrigger("Attack1");

                Collider2D[] enemyColliders = Physics2D.OverlapCircleAll(attackPosition.position, attackRange, enemyLayers);
                foreach (Collider2D enemy in enemyColliders)
                {
                    Debug.Log("ENEMY HIT" + enemy.name);
                    enemy.GetComponent<Enemy>().takeDamage(attackDamage);
                }
                nextAttackIn = Time.time + 1f / attackRate;
                //isAttackRN = false;
        }
    }

    private void FixedUpdate()
    {

        if(!legCollider.IsTouching(terrainCollider))
        {
            speed = 4f;
            rb.AddForce(new Vector2(0f, -jumpForce / 24));
            animator.SetTrigger("freeFall");
        }
        if (legCollider.IsTouching(terrainCollider))
        {

            speed = 9f;
            animator.SetTrigger("isFall");
        }


        // Move the player according to the movement vector and the speed factor
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
    }

    private void OnDisable()
    {
        movementAction.Disable();
        jumpAction.Disable();

        movementAction.performed -= OnMovementPerformed;
        movementAction.canceled -= OnMovementCanceled;

        jumpAction.performed -= OnJumpPerformed;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        Gizmos.DrawWireSphere(attackPointLeft.position, attackRange);
    }
}
