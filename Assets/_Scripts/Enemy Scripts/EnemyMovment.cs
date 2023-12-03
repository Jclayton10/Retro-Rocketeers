using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovment : MonoBehaviour
{
    public Animator anim;

    public GameObject goal; // The player's transform
    public GameObject centrePoint; 


   
    private Rigidbody rb;

   

    public float speed;
    public float maxRangeToPlayer;
    public float attackRange;
    public float rangeOfRandomPointRadius;

    private bool isMovingTowardsGoal = false;
    private Vector3 currentDestination;

    [HideInInspector]
    public NavMeshAgent agent;

    private void Awake()
    {
        
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        agent.speed = speed;
        goal = GameObject.FindWithTag("PlayerObj");
    }
    private void Start()
    {
   
        if (agent.stoppingDistance <= 0)
        {
            agent.stoppingDistance = 1;
        }
    }

    void Update()
    {
        /*Vector3 velocityLastFrame;
        rb.velocity = Vector3.Lerp(rb.velocity, velocityLastFrame, Time.deltaTime); //Time.deltaTime may need to be increased
        */
        StopMovmentAndAttack();
   

        HandleNavigation();
        
        


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
                //anim.SetBool("isMoving", true);
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
                if (RandomPoint(centrePoint.transform.position, rangeOfRandomPointRadius, out point))
                {
                    Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f);
                    agent.SetDestination(point);
                    anim.SetBool("isMoving", true);
                    //currentDestination = point;
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
     public void StopMovmentAndAttack()
     {
         if (Vector3.Distance(transform.position, goal.transform.position) <= attackRange)
         {
             // Stop the enemy's movement.
             agent.isStopped = true;
             anim.SetBool("isMoving", false);
            anim.SetBool("isAttacking", true);

             
         }
         else
         {
            anim.SetBool("isAttacking", false);
            agent.isStopped = false;
             anim.SetBool("isMoving", true);
         }

     }
    
}

