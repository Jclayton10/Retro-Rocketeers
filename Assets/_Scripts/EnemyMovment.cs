using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovment : MonoBehaviour
{

    public GameObject goal; // The player's transform
    public GameObject centrePoint;


    private NavMeshAgent agent;
    private Rigidbody rb;
 

    public float speed;
    public float maxRangeToPlayer;
    public float range;

    private bool isMovingTowardsGoal = false;
    private Vector3 currentDestination;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        agent.speed = speed;
        goal = GameObject.FindWithTag("Player");
        centrePoint = GameObject.FindWithTag("Waypoint");
    }

    void Update()
    {
        HandleNavigation();

        // Check if the enemy is on a platform and trigger a jump

    }

    private void HandleNavigation()
    {
        if (goal != null)
        {
            float distanceToGoal = Vector3.Distance(transform.position, goal.transform.position);

            if (distanceToGoal <= maxRangeToPlayer)
            {
                // Set the enemy's destination to the player's position
                agent.SetDestination(goal.transform.position);
                isMovingTowardsGoal = true;
                currentDestination = goal.transform.position;
            }
            else if (isMovingTowardsGoal && agent.remainingDistance <= agent.stoppingDistance)
            {
                // Reset the path if the enemy was previously moving towards the goal and reached it
                agent.ResetPath();
                isMovingTowardsGoal = false;
            }
        }
        else
        {
            isMovingTowardsGoal = false;
        }

        if (!isMovingTowardsGoal && !agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                // Only generate a new random point if the agent has reached the current one
                Vector3 point;
                if (RandomPoint(centrePoint.transform.position, range, out point))
                {
                    Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f);
                    agent.SetDestination(point);
                    currentDestination = point;
                }
            }
        }
    }
    private bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        Vector3 randomDirection = Random.insideUnitSphere * range;
        randomDirection += center;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, range, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }
}
