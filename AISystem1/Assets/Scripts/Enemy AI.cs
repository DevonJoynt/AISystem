using UnityEngine;
using UnityEngine.AI; // Needed for NavMeshAgent movement

public class EnemyAI : MonoBehaviour
{
    // Patrol waypoints - enemy walks between 
    public Transform[] waypoints;

    // Reference to the player enemy will chase/attack
    public Transform player;

    // Distance from player before the enemy starts chasing
    public float chaseRange = 10f;

    // Distance from player before the enemy attacks
    public float attackRange = 2f;

    private NavMeshAgent agent;       // Unity's built-in pathfinding component
    private int currentWaypoint = 0;  // Keeps track of waypoint enemy is going to next

    // current state enemy is in
    private enum State { Patrolling, Chasing, Attacking }
    private State currentState;

    void Start()
    {
        // Get NavMeshAgent component from this enemy
        agent = GetComponent<NavMeshAgent>();

        // If player already assigned manually, store in the global blackboard
        if (player != null)
        {
            if (!FsmBlackboard.GlobalBlackboard.Variables.ContainsKey("PlayerTransform"))
            {
                FsmBlackboard.GlobalBlackboard.Set("PlayerTransform", player);
            }
        }
        else
        {
            // Otherwise, try to fetch player from blackboard
            if (FsmBlackboard.GlobalBlackboard.Variables.ContainsKey("PlayerTransform"))
            {
                player = FsmBlackboard.GlobalBlackboard.GetTransform("PlayerTransform");
            }
        }

        // Start patrolling waypoints assigned
        if (waypoints != null && waypoints.Length > 0)
        {
            agent.SetDestination(waypoints[currentWaypoint].position);
            SetState(State.Patrolling);
        }
    }

    void Update()
    {
        // Try to reassign player if lost for any reason
        if (player == null)
        {
            if (FsmBlackboard.GlobalBlackboard.Variables.ContainsKey("PlayerTransform"))
            {
                player = FsmBlackboard.GlobalBlackboard.GetTransform("PlayerTransform");
            }
        }

        // If player or waypoints aren't assigned, do nothing
        if (player == null || waypoints == null || waypoints.Length == 0)
            return;

        // Get distance between enemy and player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Decide what behavior is performed based on distance to player
        if (distanceToPlayer <= attackRange)
        {
            Attack(); // Player close enough to attack
        }
        else if (distanceToPlayer <= chaseRange)
        {
            ChasePlayer(); // Player within chase range
        }
        else
        {
            Patrol(); // Player too far, continue patrolling
        }
    }

    // Change current state and log it for debugging
    void SetState(State newState)
    {
        if (newState != currentState)
        {
            currentState = newState;
            Debug.Log($"Enemy is now: {currentState}");
        }
    }

    // Move along waypoint path
    void Patrol()
    {
        SetState(State.Patrolling);

        // If agent reached current destination, go to next waypoint
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
            agent.SetDestination(waypoints[currentWaypoint].position);
        }
    }

    // Chase player by setting agent's destination to player's position
    void ChasePlayer()
    {
        SetState(State.Chasing);

        if (player != null)
        {
            agent.SetDestination(player.position);
        }
    }

    // Stop movement and attack player 
    void Attack()
    {
        SetState(State.Attacking);
        agent.ResetPath(); // Stop NavMeshAgent from moving

        if (player != null)
        {
            transform.LookAt(player); // Face the player
            Debug.Log("Enemy is attacking!");
        }
    }
}

