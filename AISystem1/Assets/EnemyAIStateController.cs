using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAIStateController : MonoBehaviour
{
    // AI State Machine
    public enum EnemyState
    {
        Patrol,
        Chase,
        Attack
    }

    [Header("State Machine")]
    public EnemyState currentState = EnemyState.Patrol;

    [Header("Detection Settings")]
    public float chaseRadius = 10f;
    public float attackRadius = 2f;
    public float returnToPatrolRadius = 15f;

    [Header("Movement")]
    public float patrolSpeed = 2f;
    public float chaseSpeed = 4f;

    [Header("Combat")]
    public float attackDamage = 10f;
    public float attackRate = 1f;
    private float nextAttackTime;

    [Header("Patrol")]
    public Transform[] waypoints;
    private int currentWaypointIndex = 0;

    // References
    private NavMeshAgent agent;
    private Transform player;
    private Animator animator;
    private float distanceToPlayer;

    private void Start()
    {
        // Get component references
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        // Find the player
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (player == null)
        {
            Debug.LogWarning("Player not found. Make sure player has 'Player' tag.");
        }

        // Initialize agent
        agent.speed = patrolSpeed;

        // Start patrolling
        if (waypoints.Length > 0)
        {
            SetDestinationToCurrentWaypoint();
        }
        else
        {
            Debug.LogWarning("No waypoints assigned to enemy. Will remain stationary.");
        }
    }

    private void Update()
    {
        if (player != null)
        {
            // Update distance to player
            distanceToPlayer = Vector3.Distance(transform.position, player.position);

            // Evaluate and update the current state
            EvaluateState();

            // Execute the current state behavior
            ExecuteState();
        }
    }

    private void EvaluateState()
    {
        switch (currentState)
        {
            case EnemyState.Patrol:
                // If player is within chase radius, switch to chase
                if (distanceToPlayer <= chaseRadius)
                {
                    ChangeState(EnemyState.Chase);
                }
                break;

            case EnemyState.Chase:
                // If player is within attack radius, switch to attack
                if (distanceToPlayer <= attackRadius)
                {
                    ChangeState(EnemyState.Attack);
                }
                // If player is too far, return to patrol
                else if (distanceToPlayer > returnToPatrolRadius)
                {
                    ChangeState(EnemyState.Patrol);
                }
                break;

            case EnemyState.Attack:
                // If player moves out of attack range, switch back to chase
                if (distanceToPlayer > attackRadius)
                {
                    ChangeState(EnemyState.Chase);
                }
                break;
        }
    }

    private void ExecuteState()
    {
        switch (currentState)
        {
            case EnemyState.Patrol:
                Patrol();
                break;

            case EnemyState.Chase:
                Chase();
                break;

            case EnemyState.Attack:
                Attack();
                break;
        }
    }

    private void ChangeState(EnemyState newState)
    {
        // Exit current state
        switch (currentState)
        {
            case EnemyState.Patrol:
                // Nothing special to do when exiting patrol
                break;

            case EnemyState.Chase:
                // Nothing special to do when exiting chase
                break;

            case EnemyState.Attack:
                // Maybe reset attack animation
                if (animator) animator.SetBool("IsAttacking", false);
                break;
        }

        // Enter new state
        switch (newState)
        {
            case EnemyState.Patrol:
                agent.speed = patrolSpeed;
                if (animator) animator.SetBool("IsChasing", false);

                // Set destination to nearest waypoint
                if (waypoints.Length > 0)
                {
                    SetDestinationToCurrentWaypoint();
                }
                break;

            case EnemyState.Chase:
                agent.speed = chaseSpeed;
                if (animator) animator.SetBool("IsChasing", true);
                break;

            case EnemyState.Attack:
                agent.velocity = Vector3.zero;  // Stop moving
                if (animator) animator.SetBool("IsAttacking", true);
                break;
        }

        // Update the current state
        currentState = newState;
        Debug.Log($"{gameObject.name} changed state to {newState}");
    }

    private void Patrol()
    {
        // Check if we've reached the waypoint
        if (waypoints.Length == 0) return;

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            // Move to the next waypoint
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            SetDestinationToCurrentWaypoint();
        }
    }

    private void Chase()
    {
        // Update destination to player's position
        agent.SetDestination(player.position);
    }

    private void Attack()
    {
        // Face the player
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

        // Attack at specified rate
        if (Time.time >= nextAttackTime)
        {
            // Perform attack
            PerformAttack();
            nextAttackTime = Time.time + 1f / attackRate;
        }
    }

    private void PerformAttack()
    {
        // Trigger attack animation
        if (animator) animator.SetTrigger("Attack");

        // Apply damage to player - Modified to use SendMessage instead of direct component reference
        if (player != null)
        {
            // This will call a "TakeDamage" function on the player if it exists
            player.SendMessage("TakeDamage", attackDamage, SendMessageOptions.DontRequireReceiver);

            // Alternative approach - you can implement your own damage system here
            // For example, you could use Physics.OverlapSphere to find all colliders in attack range
            // and apply damage to any that have the Player tag
        }

        Debug.Log($"{gameObject.name} attacked player for {attackDamage} damage");
    }

    private void SetDestinationToCurrentWaypoint()
    {
        if (waypoints.Length == 0) return;
        agent.SetDestination(waypoints[currentWaypointIndex].position);
    }

    // Visualize detection ranges in the editor
    private void OnDrawGizmosSelected()
    {
        // Draw chase radius
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRadius);

        // Draw attack radius
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);

        // Draw return to patrol radius
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, returnToPatrolRadius);

        // Draw patrol path
        Gizmos.color = Color.green;
        if (waypoints.Length > 0)
        {
            for (int i = 0; i < waypoints.Length; i++)
            {
                if (waypoints[i] != null)
                {
                    // Draw connection line between waypoints
                    if (i < waypoints.Length - 1 && waypoints[i + 1] != null)
                    {
                        Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
                    }
                    else if (waypoints[0] != null) // Connect last to first
                    {
                        Gizmos.DrawLine(waypoints[i].position, waypoints[0].position);
                    }

                    // Draw waypoint
                    Gizmos.DrawSphere(waypoints[i].position, 0.5f);
                }
            }
        }
    }
}