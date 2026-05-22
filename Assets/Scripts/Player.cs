using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Components;
using DefaultNamespace;
using DefaultNamespace.FunctionLibraries;

public class Player : MonoBehaviour
{
    [Header("Ground Check")]
    [SerializeField] private Vector3 groundCheckPoint;
    [SerializeField] private float groundCheckRadius = 0.15f;
    [SerializeField] private LayerMask groundLayerMask;

    [Header("Jump")]
    [SerializeField] private int maxJumpCount = 2;
    [SerializeField] private int jumpCounter;

    [Header("Components")]
    [SerializeField] private MovementPhysicsComponent2D movementPhysicsComponent;
    [SerializeField] private InteractionComponent interactionComponent;
    
    public bool IsGrounded { get; private set; }
    
    [Header("Input")]
    [SerializeField] private InputActionAsset input;
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction interactAction;
    
    private void Start()
    {
        moveAction = input.FindAction("Move");
        jumpAction = input.FindAction("Jump");
        interactAction = input.FindAction("Interact");
        
        jumpAction.performed += OnJumpPerformed;
        interactAction.performed += OnInteractPerformed;
    }
    
    private void Update()
    {
        CheckGrounded();

        if (IsGrounded)
        {
            jumpCounter = 0;
        }

        Vector2 velocity = moveAction.ReadValue<Vector2>();
        movementPhysicsComponent.MoveVelocity(velocity);
        
        if (interactionComponent.HasInteractablesInRange)
        {
            interactionComponent.ShowInteract(interactionComponent.GetFirstInteractableInRange.Item2);
        }
        else
        {
            interactionComponent.HideInteract();
        }
    }

    private void OnJumpPerformed(InputAction.CallbackContext _)
    {
        if (IsGrounded || jumpCounter < maxJumpCount)
        {
            movementPhysicsComponent.Jump();
            jumpCounter++;
        }
    }
    
    private void OnInteractPerformed(InputAction.CallbackContext obj)
    {
        Debug.Log("Interacted!");
        if (interactionComponent.HasInteractablesInRange)
        {
            interactionComponent.GetFirstInteractableInRange.Item2.Interact(interactionComponent);
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
