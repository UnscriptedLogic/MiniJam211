using System;
using System.Runtime.CompilerServices;
using Components;
using DefaultNamespace;
using JetBrains.Annotations;
using Pathfinding;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Assertions.Must;

[System.Serializable]
public struct EntityDetails
{
    [SerializeField] private string name;
    [SerializeField] private Sprite icon;
    
    public string Name => name;
    public Sprite Icon => icon;
}

public class Entity : MonoBehaviour, IInteractable
{
    [SerializeField] private EntityDetails entityDetails;

    public EntityDetails EntityDetails => entityDetails;

    [SerializeField] private Vector3 destination;
    [SerializeField] private float nextPointDistance = 0.25f;
    [SerializeField] private float repathInterval = 0.35f;

    [SerializeField] private float stopRange = 1f;

    [SerializeField] private MovementPhysicsComponent2D movement;
    [SerializeField] private Seeker seeker;

    [Header("Jump Navigation")]
    [SerializeField] private Vector3 groundCheckPoint;
    [SerializeField] private float groundCheckRadius = 0.12f;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float waypointJumpHeightThreshold = 0.2f;
    [SerializeField] private float jumpCooldown = 0.15f;

    private Path path;
    private int currentWaypoint;
    private float repathTimer;
    private float lastJumpTime = -999f;

    [Header("Attention")]
    [SerializeField] private AttentionComponent attentionComponent;
    [SerializeField] private InteractionComponent interactionComponent;
    [SerializeField] private float interactionCooldown;
    private float cooldownTimer;

    [SerializeField] private float attentionDecayInterval;

    private float decayTimer;

    [SerializeField] private float entityType; //0 = seeker, 1 = giver

    [Header("Sprite Stages")]
    [SerializeField] private Sprite[] animationSprites1;
    [SerializeField] private Sprite[] animationSprites2;
    [SerializeField] private Sprite[] animationSprites3;
    [SerializeField] private Sprite[] animationSprites4;
    [SerializeField] private Sprite[] animationSprites5;
    [SerializeField] private SpriteRenderer spriteRenderer;

    //Animation variables
    private Sprite[] currentAnimation;
    private int spriteType;
    private float frameTime = 0.15f;
    private int currentFrame;
    private float timer;

    //Movement variables
    public int boundsMinX;
    public int boundsMaxX;
    private float randomX;
    private bool needsToJump;

    
    private void Start()
    {
        AnimateSprite(spriteType);
        RequestPath();
    }

    private void OnPathCompleted(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    private void Update()
    {
        repathTimer += Time.deltaTime;
        if (repathTimer >= repathInterval)
        {
            repathTimer = 0f;
            RequestPath();
        }

        FollowPath();


        decayTimer += Time.deltaTime;
        if (decayTimer >= attentionDecayInterval && attentionComponent != null)
        {
            decayTimer = 0f;
            attentionComponent.DecayAttention();
        }

        cooldownTimer -= Time.deltaTime;

        if (interactionComponent != null && interactionComponent.HasInteractablesInRange)
        {
            OnInteractPerformed();
        }

        if (currentAnimation != null && currentAnimation.Length > 0)
        {
            timer += Time.deltaTime;
            if (timer >= frameTime)
            {
                timer = 0f;
                currentFrame = (currentFrame + 1) % currentAnimation.Length;
                spriteRenderer.sprite = currentAnimation[currentFrame];
            }
        }

    }

    private void RequestPath()
    {
        //DEBUG PURPOSES
        GetRandomPosition2D();

        if (seeker == null)
        {
            return;
        }

        if (seeker.IsDone())
        {
            if (Vector3.Distance(destination, transform.position) <= stopRange)
            {
                movement.MoveVelocity(Vector2.zero);
                return;
            }

            seeker.StartPath(transform.position, destination, OnPathCompleted);
        }


    }

    private void FollowPath()
    {
        if (movement == null)
        {
            return;
        }

        if (path == null || path.vectorPath == null || path.vectorPath.Count == 0)
        {
            movement.MoveVelocity(Vector2.zero);
            return;
        }

        if (currentWaypoint >= path.vectorPath.Count)
        {
            movement.MoveVelocity(Vector2.zero);
            return;
        }

        Vector3 currentTarget = path.vectorPath[currentWaypoint];
        Vector2 toTarget = currentTarget - transform.position;

        if (toTarget.magnitude <= nextPointDistance)
        {
            currentWaypoint++;
            if (currentWaypoint >= path.vectorPath.Count)
            {
                movement.MoveVelocity(Vector2.zero);
                return;
            }

            currentTarget = path.vectorPath[currentWaypoint];
            toTarget = currentTarget - transform.position;
        }

        Vector2 moveDirection = new Vector2(Mathf.Sign(toTarget.x), 0f);
        if (Mathf.Abs(toTarget.x) < 0.02f)
        {
            moveDirection.x = 0f;
        }

        if (MathF.Sign(toTarget.x) == -1)
        {
            transform.eulerAngles = new Vector2(0, 0);
        }

        if (MathF.Sign(toTarget.x) == 1)
        {
            transform.eulerAngles = new Vector2(0, 180);
        }

        movement.MoveVelocity(moveDirection);

        bool canJump = Time.time >= lastJumpTime + jumpCooldown;
        if (needsToJump && canJump && IsGrounded())
        {
            movement.Jump();
            lastJumpTime = Time.time;
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(transform.position + groundCheckPoint, groundCheckRadius, groundMask);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position + groundCheckPoint, groundCheckRadius);
    }
    private void OnInteractPerformed()
    {
        if (interactionComponent.HasInteractablesInRange && cooldownTimer <= 0f)
        {
            if (interactionComponent == null) return;
            if (!interactionComponent.HasInteractablesInRange) return;

            var first = interactionComponent.GetFirstInteractableInRange;
            if (first.Item2 == null) return;


            Component targetComponent = first.Item2 as Component;
            if (targetComponent == null) return;

            if (targetComponent.gameObject == gameObject) return;

            first.Item2.Interact(interactionComponent);

            cooldownTimer = interactionCooldown;
        }
    }
    public bool CanInteract(InteractionComponent instigator)
    {
        //For any conditional stuff

        return true;
    }

    public void Interact(InteractionComponent instigator)
    {
        AttentionGiverComponent attentionGiverComponent = instigator.GetComponent<AttentionGiverComponent>();
        if (attentionGiverComponent == null) return;

        attentionGiverComponent.GiveAttention(attentionGiverComponent.AttentionValue, attentionComponent);
    }

    public string GetActionName()
    {
        return "Give Attention";
    }

    

    public void AnimateSprite(int type)
    {
        spriteType = type;

        switch (spriteType)
        {
            case 0:
                currentAnimation = animationSprites1;
                break;
            case 1:
                currentAnimation = animationSprites2;
                break;
            case 2:
                currentAnimation = animationSprites3;
                break;
            case 3:
                currentAnimation = animationSprites4;
                break;
        }

        if (currentAnimation != null && currentAnimation.Length > 0)
        {
            spriteRenderer.sprite = currentAnimation[currentFrame];
        }
    }

    private void GetRandomPosition2D()
    {
        randomX = UnityEngine.Random.Range(boundsMinX, boundsMaxX);
        destination = new Vector2(randomX, transform.position.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("NPC"))
        {
            needsToJump = true;
        }
        else { needsToJump = false; }
    }
}