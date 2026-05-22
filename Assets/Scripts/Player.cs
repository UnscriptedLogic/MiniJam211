using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Componets;

public class Player : MonoBehaviour
{
    [FormerlySerializedAs("movementComponent")]
    [Header("Movement")]
    [SerializeField] private MovementPhysicsComponent2D movementPhysicsComponent;

    [Header("Ground Check")]
    [SerializeField] private Vector3 groundCheckPoint;
    [SerializeField] private float groundCheckRadius = 0.15f;
    [SerializeField] private LayerMask groundLayerMask;

    [Header("Jump")]
    [SerializeField] private int maxJumpCount = 2;
    [SerializeField] private int jumpCounter;

    public bool IsGrounded { get; private set; }
    
    [Header("Input")]
    [SerializeField] private InputActionAsset input;
    private InputAction moveAction;
    private InputAction jumpAction;
    
    private void Start()
    {
        moveAction = input.FindAction("Move");
        jumpAction = input.FindAction("Jump");
        
        jumpAction.performed += OnJumpPerformed;
    }
    
    void Update()
    {
        CheckGrounded();

        if (IsGrounded)
        {
            jumpCounter = 0;
        }

        Vector2 velocity = moveAction.ReadValue<Vector2>();
        movementPhysicsComponent.MoveVelocity(velocity);
    }

    private void OnJumpPerformed(InputAction.CallbackContext _)
    {
        if (IsGrounded || jumpCounter < maxJumpCount)
        {
            movementPhysicsComponent.Jump();
            jumpCounter++;
        }
    }

    private void CheckGrounded()
    {
        if (groundCheckPoint == null)
        {
            IsGrounded = false;
            return;
        }

        IsGrounded = Physics2D.OverlapCircle( transform.position +
            groundCheckPoint,
            groundCheckRadius,
            groundLayerMask
        );
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheckPoint == null)
        {
            return;
        }

        Gizmos.color = IsGrounded ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position + groundCheckPoint, groundCheckRadius);
    }
}
