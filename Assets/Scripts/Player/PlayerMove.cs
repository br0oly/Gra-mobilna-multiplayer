using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference dashAction;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float dashForce;
    [SerializeField] private float dashLoockCoolDown = 5f;

    private Vector2 moveInput;
    private Vector2 dashInput;

    private Rigidbody2D body;

    private bool isDashing;
    private bool dashLoock;

    private void OnEnable()
    {
        moveAction.action.performed += OnMovePreformed;
        moveAction.action.canceled += OnMoveCanceled;
        
    }

    private void OnDisable()
    {
        moveAction.action.performed -= OnMovePreformed;
        moveAction.action.canceled -= OnMoveCanceled;
        
    }

    public void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (dashAction.action.WasPressedThisFrame() && !dashLoock) 
        {
            dashInput = moveInput.normalized;

            if (dashInput != Vector2.zero)
            {
                isDashing = true;
            }
        }
    }

    private void FixedUpdate()
    {
        if (!isDashing) 
        {
            body.linearVelocity = moveInput.normalized * moveSpeed;
        }
        else if (isDashing == true) 
        {
            body.AddForce(dashInput * dashForce, ForceMode2D.Impulse);
            isDashing = false;
            dashLoock = true;
            StartCoroutine(DashCoolDown());
        }
    }

    IEnumerator DashCoolDown ()
    {
        yield return new WaitForSecondsRealtime(dashLoockCoolDown);
        
        dashLoock = false;
    }

    private void OnMovePreformed(InputAction.CallbackContext context) 
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        moveInput = Vector2.zero;
    }
}
