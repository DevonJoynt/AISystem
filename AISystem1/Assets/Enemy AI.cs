using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform[] waypoints;
    public Transform player;
    public float chaseRange = 10f;
    public float attackRange = 2f;

    private NavMeshAgent agent;
    private int currentWaypoint = 0;

    private enum State { Patrolling, Chasing, Attacking }
    private State currentState;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (waypoints.Length > 0)
        {
            agent.SetDestination(waypoints[currentWaypoint].position);
            SetState(State.Patrolling);
        }
    }

    void Update()
    {
        if (player == null || waypoints.Length == 0) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            Attack();
        }
        else if (distanceToPlayer <= chaseRange)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }
    }

    void SetState(State newState)
    {
        if (newState != currentState)
        {
            currentState = newState;
            Debug.Log("Enemy is now: " + currentState.ToString());
        }
    }

    void Patrol()
    {
        SetState(State.Patrolling);

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
            agent.SetDestination(waypoints[currentWaypoint].position);
        }
    }

    void ChasePlayer()
    {
        SetState(State.Chasing);
        agent.SetDestination(player.position);
    }

    void Attack()
    {
        SetState(State.Attacking);
        agent.ResetPath();
        transform.LookAt(player);
        Debug.Log("Enemy is attacking!");
    }
}
