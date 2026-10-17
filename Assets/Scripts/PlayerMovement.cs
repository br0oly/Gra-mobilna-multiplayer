using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference dashAction;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float dashSpeed;
    [SerializeField] private Rigidbody2D body;

    private Vector2 moveInput;
    private Vector2 dashInput;
    private bool willDash;

    public void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (dashAction.action.IsPressed())
        {
            if (willDash == false)
            {
                willDash = true;
            }
        }
    }

    public void FixedUpdate()
    {
        body.MovePosition(body.position + moveInput.normalized * moveSpeed * Time.deltaTime);
        if (willDash)
        {
            PlayerDash();
        }

    }

    private void OnEnable()
    {
        moveAction.action.performed += OnMovePerformed;
        moveAction.action.canceled += OnMoveCanceled;


    }

    private void OnDisable()
    {
        moveAction.action.performed -= OnMovePerformed;
        moveAction.action.canceled -= OnMoveCanceled;
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        moveInput = Vector2.zero;
    }
    private void PlayerDash()
    {
        Debug.Log("Daszuję!!!!!!!!!!!");
        body.AddForce(body.position * dashSpeed, ForceMode2D.Impulse);
        willDash = false;
    }
}
