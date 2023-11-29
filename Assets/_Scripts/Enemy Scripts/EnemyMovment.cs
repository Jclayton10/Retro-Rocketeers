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

    public float flashDuration = 0.1f;
    public Color flashColor = Color.red;

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
        //goal = GameObject.FindWithTag("Player");
        //centrePoint = GameObject.FindWithTag("Waypoint");
    }
    private void Start()
    {
   
        if (agent.stoppingDistance >= 0)
        {
            agent.stoppingDistance = 1;
        }
    }

    void Update()
    {
        /*Vector3 velocityLastFrame;
        rb.velocity = Vector3.Lerp(rb.velocity, velocityLastFrame, Time.deltaTime); //Time.deltaTime may need to be increased
        */
        //StopMovment();
        HandleNavigation();
        Debug.Log("AgentEnabled: "+agent.enabled);


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
     public void StopMovment()
     {
         if (Vector3.Distance(transform.position, goal.transform.position) <= attackRange)
         {
             // Stop the enemy's movement.
             agent.isStopped = true;
             //anim.SetBool("isMoving", false);

             // Call the method to perform the attack (you should define this method).
             //Attack();
         }
         else
         {
             agent.isStopped = false;
             //anim.SetBool("isMoving", true);
         }

     }
    
    public void Attack()
    {
        anim.SetTrigger("Attack");
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Sword"))
        {
           agent.enabled = false;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Sword"))
        {
            agent.enabled = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Sword"))
        {
            FlashEnemy(gameObject);
            StartCoroutine(WaitAndEnableAgent(3f));
            agent.enabled = true;
            HandleNavigation();
        }
    }
    IEnumerator WaitAndEnableAgent(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        agent.enabled = true;
        HandleNavigation(); // Restart navigation after waiting
    }
    void FlashEnemy(GameObject enemy)
    {
        Renderer enemyRenderer = enemy.GetComponent<Renderer>();
        if (enemyRenderer != null)
        {
            // Get the original material or color
            Material originalMaterial = enemyRenderer.material;

            // Check if the original material is null
            if (originalMaterial != null)
            {
                // Create a new material with the appropriate shader
                Material flashMaterial = new Material(Shader.Find("Standard"));
                flashMaterial.color = flashColor;

                // Assign the new material to the enemy
                enemyRenderer.material = flashMaterial;

                // Use a coroutine to reset the color after a short duration
                StartCoroutine(ResetMaterial(enemyRenderer, originalMaterial, flashMaterial, flashDuration));
            }
            else
            {
                Debug.LogWarning("Enemy's material is null. Ensure the enemy has a material that supports color changes.");
            }
        }

    }

    IEnumerator ResetMaterial(Renderer renderer, Material originalMaterial, Material flashMaterial, float duration)
    {
        // Wait for the specified duration
        yield return new WaitForSeconds(duration);

        // Reset the material
        renderer.material = originalMaterial;
    }
    

}
