using Componets;
using Pathfinding;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField] private Vector3 destination;
    [SerializeField] private float nextPointDistance = 0.25f;
    [SerializeField] private float repathInterval = 0.35f;

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

    private void Start()
    {
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
    }

    private void RequestPath()
    {
        //DEBUG PURPOSES
        destination = GameObject.FindGameObjectWithTag("Player").transform.position;
        
        if (seeker == null)
        {
            return;
        }

        if (seeker.IsDone())
        {
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

        movement.MoveVelocity(moveDirection);

        bool targetIsHigher = toTarget.y > waypointJumpHeightThreshold;
        bool canJump = Time.time >= lastJumpTime + jumpCooldown;
        if (targetIsHigher && canJump && IsGrounded())
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
}
