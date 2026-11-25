using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference jumpAction;
    [SerializeField] private InputActionReference dashAction;

    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float dashForce;
    [SerializeField] private float dashDuration;

    [Header("Lock")]
    [SerializeField] private float jumpLockDuration;
    [SerializeField] private float dashLockDuration;
    [SerializeField] private float moveLockDuration;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField, Min(0.01f)] private float groundCheckRadius = 0.2f;

    [SerializeField] private BattleManager startBattle;

    private PlayerDeath death;
    private Rigidbody2D body;
    private PushBack back;

    private Vector2 moveInput;
    private Vector2 dashDirection;
    
    private bool isJumping;
    private bool jumpLockActive;
    private bool isDashing = false;
    private bool canDash = true;
    private bool deathState = false;
    private bool moveLock;
    private bool lockMove = true;

    private void OnEnable()
    {
        moveAction.action.performed += MovePreformed;
        moveAction.action.canceled += MoveCanceled;

        startBattle.OnFightStart += UnLockMove;
        death.OnPlayerDeath += DeathState;
        back.OnPushBack += CanMove;
    }

    private void OnDisable()
    {
        moveAction.action.performed -= MovePreformed;
        moveAction.action.canceled -= MoveCanceled;

        startBattle.OnFightStart -= UnLockMove;
        death.OnPlayerDeath -= DeathState;
        back.OnPushBack -= CanMove;
    }
    void Awake()
    {
        death = GetComponent<PlayerDeath>();
        body = GetComponent<Rigidbody2D>();
        back = GetComponent<PushBack>();
    }

    void Update()
    {
        if (lockMove || moveLock || deathState) return;

        Flip();

        if (jumpAction.action.WasPressedThisFrame() && !jumpLockActive)
        {
            isJumping = true;
        }

        if (dashAction.action.WasPressedThisFrame() && canDash)
        {
            dashDirection = moveInput;

            if (dashDirection != Vector2.zero)
            {
                if (transform.localScale.x > 0)
                {
                    dashDirection = Vector2.right;
                }
                else
                {
                    dashDirection = Vector2.left;
                }
                StartCoroutine(Dash());
            }
        }
    }

    private void FixedUpdate()
    {
        if (isDashing || deathState || lockMove || moveLock) return;
        
        body.linearVelocity = new Vector2(moveInput.x * moveSpeed, body.linearVelocity.y);

        if (isJumping)
        {
            isJumping = false;
            
            if (CheckIsGrounded()) 
            {
                body.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                StartCoroutine(JumpLoked());
            }
        }
    }

    private bool CheckIsGrounded()
    {
        return Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        ) != null;
    }
    private void Flip()
    {
        if (moveInput.x > 0)
        {
            transform.localScale = new Vector3(1, 1);
        }
        else if (moveInput.x < 0)
        {
            transform.localScale = new Vector3(-1, 1);
        }
    }

    private void MovePreformed(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void MoveCanceled(InputAction.CallbackContext context)
    {
        moveInput = Vector2.zero;
    }

    private void CanMove() 
    {
        moveLock = true;
        StartCoroutine(MoveLocked());
    }

    private void DeathState()
    {
        deathState = true;
    }

    private void UnLockMove() 
    {
        lockMove = false;
    }
    IEnumerator MoveLocked() 
    {
        yield return new WaitForSecondsRealtime(moveLockDuration);
        moveLock = false;
    }
    IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;

        body.linearVelocity = dashDirection.normalized * dashForce;

        yield return new WaitForSeconds(dashDuration);

        isDashing = false;

        body.linearVelocity = new Vector2(0f, body.linearVelocity.y);

        yield return new WaitForSeconds(dashLockDuration);

        canDash = true;
    }
    IEnumerator JumpLoked()
    {
        jumpLockActive = true;

        yield return new WaitForSecondsRealtime(jumpLockDuration);

        jumpLockActive = false;
    }
}