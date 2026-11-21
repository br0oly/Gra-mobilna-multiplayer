using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;

public class PlayerNetworkMovement : MonoBehaviour
{
    [Header("Input")] private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction dashAction;

    [Header("Movement")] [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float dashForce;
    [SerializeField] private float dashDuration;

    [Header("Lock")] [SerializeField] private float jumpLockDuration;
    [SerializeField] private float dashLockDuration;
    [SerializeField] private float moveLockDuration;

    [Header("Ground Check")] [SerializeField]
    private Transform groundCheck;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField, Min(0.01f)] private float groundCheckRadius = 0.2f;

    [SerializeField] bool useNetworkOwnership;
    private NetworkObject networkObject;

    bool CanControl =>
        !useNetworkOwnership ||
        (networkObject != null && networkObject.IsSpawned && networkObject.IsOwner);

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

    void Awake()
    {
        networkObject = GetComponent<NetworkObject>();
        var actions = InputSystem.actions;
        moveAction = actions.FindAction("Player/Move");
        jumpAction = actions.FindAction("Player/Jump");
        dashAction = actions.FindAction("Player/Dash");

        death = GetComponent<PlayerDeath>();
        body = GetComponent<Rigidbody2D>();
        back = GetComponent<PushBack>();
    }

    void Update()
    {
        if (!CanControl) return;

        moveInput = moveAction.ReadValue<Vector2>();

        if (deathState) return;
        if (moveLock) return;

        if (jumpAction.WasPressedThisFrame() && !jumpLockActive)
        {
            isJumping = true;
        }

        if (dashAction.WasPressedThisFrame() && canDash)
        {
            if (dashAction.WasPressedThisFrame() && canDash && moveInput.x != 0f)
            {
                dashDirection = moveInput.x > 0f ? Vector2.right : Vector2.left;
                StartCoroutine(Dash());
            }
        }
    }

    private void OnEnable()
    {
        if (death != null)
            death.OnPlayerDeath += DeathState;

        if (back != null)
            back.OnPushBack += CantMove;
    }

    private void OnDisable()
    {
        if (death != null)
            death.OnPlayerDeath -= DeathState;

        if (back != null)
            back.OnPushBack -= CantMove;
    }

    private void FixedUpdate()
    {
        if (!CanControl) return;
        if (isDashing) return;

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
    private void CantMove()
    {
        moveLock = true;
        StartCoroutine(MoveLocked());
    }

    private void DeathState()
    {
        deathState = true;
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
        body.linearVelocity = dashDirection * dashForce;

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