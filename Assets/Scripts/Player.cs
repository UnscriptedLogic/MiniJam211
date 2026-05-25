using System.Collections.Generic;
using Components;
using DefaultNamespace;
using DefaultNamespace.FunctionLibraries;
using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEditor.Experimental.GraphView.GraphView;

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
    [SerializeField] private AttentionGiverComponent attentionGiverComponent;
    
    public AttentionGiverComponent AttentionGiverComponent => attentionGiverComponent;
    
    public bool IsGrounded { get; private set; }
    
    [Header("Input")]
    [SerializeField] private InputActionAsset input;
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction interactAction;

    [Header("UI")] 
    [SerializeField] private UIPlayerHUD playerHUDPrefab;
    
    private UIPlayerHUD playerHUD;

    private LevelManagerScript levelManager;

    //Animation Variables
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] animations;
    private float timer;
    private float frameTime = 0.15f;
    private int currentFrame;
    public void Initialize(LevelManagerScript levelManager)
    {
        this.levelManager = levelManager;
        
        playerHUD = Instantiate(playerHUDPrefab);
        playerHUD.Initialize(this, levelManager);
        
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

        if (animations != null && animations.Length > 0)
        {
            timer += Time.deltaTime;
            if (timer >= frameTime)
            {
                timer = 0f;
                currentFrame = (currentFrame + 1) % animations.Length;
                spriteRenderer.sprite = animations[currentFrame];
            }
        }

        if (velocity.x > 0f)
        {
            transform.eulerAngles = new Vector2(0, 0);
        }
        if (velocity.x < 0f)
        {
            transform.eulerAngles = new Vector2(0, 180);
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
        if (interactionComponent.HasInteractablesInRange)
        {
            interactionComponent.GetFirstInteractableInRange.Item2.Interact(interactionComponent);
        }
        
        // if (interactionComponent.HasInteractablesInRange && playerAttentionValue >= .1f && !cooldown)
        // {
        //     interactionComponent.GetFirstInteractableInRange.Item2.Interact(interactionComponent);
        // }

        // if (!cooldown)
        // {
        //     playerAttentionValue -= .1f;
        //     playerAttentionBar.value = playerAttentionValue;
        // }
        
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
