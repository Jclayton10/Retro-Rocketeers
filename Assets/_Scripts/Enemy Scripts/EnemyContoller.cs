using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyContoller : MonoBehaviour
{
    private WaveSpawner waveSpawner;
    public static EnemyContoller enemyContoller { get; private set; }
    [Header("Enemy Movment Varibles")]
    public Animator anim;
    public GameObject goal; // The player's transform
    public GameObject centrePoint;
    public float speed;
    public float maxRangeToPlayer;
    public float attackRange;
    public float rangeOfRandomPointRadius;

    public bool isAttaking = false;
    
    private bool isMovingTowardsGoal = false;
    private Vector3 currentDestination;

    [HideInInspector]
    public NavMeshAgent agent;

    [Header("Enemy Varibles")]
    public AudioSource enemyHitSound;
    public int damageAmount = 10;
    public int currentHealth = 100;
    public SphereCollider enemySphereCollider;
    private int maxHealth;

    [Header("Enemy HealthBar Varibles")]
    public Slider enemyHealthSlider;

    private void Awake()
    {
        enemyContoller = this;
        waveSpawner = FindFirstObjectByType<WaveSpawner>(); // Finds the WaveSpawner script
        // Movment
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
        goal = GameObject.FindWithTag("PlayerObj");
        

    }
    // Start is called before the first frame update
    void Start()
    {
        maxHealth = currentHealth;

        SetMaxHealthUI(maxHealth);
        if (agent.stoppingDistance <= 0)
        {
            agent.stoppingDistance = 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        StopMovmentAndAttack();

        HandleNavigation();
    }
    public void SetMaxHealthUI(int health)
    {
        enemyHealthSlider.maxValue = health;
        enemyHealthSlider.value = health;
    }
    public void SetHealthUI(int health)
    {
        enemyHealthSlider.value = health;
    }
    // Use this function if you want the enemy to take damage from somthing for example the sword
    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        SetHealthUI(currentHealth);
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        AchievementManager.enemyAchCount += 1;

        // Check if the script is not null before accessing it
        if (waveSpawner != null)
        {
            // Access the waveSpawner object
            waveSpawner.waves[waveSpawner.currentWaveIndex].enemiesLeft--;
        }
        enemyHitSound.Stop();
        Destroy(gameObject);

    }

    // These Handle the Enemys movment. It will patorl around randomy using
    // the RangeOfRandomPointRadius varible. This varible is a radius around the enemy and this
    // will pick a point in that radis and the enemy will move to it. It will not genrate a point to move
    // to until the the enemy has reached that point. If the enemy is in range of the player the enemy will go \
    // to the player.
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
            // Stop the enemy's movement when in range of player and attack the player.
            agent.isStopped = true;
            anim.SetBool("isMoving", false);
            isAttaking = true;
            anim.SetBool("isAttacking", true);
            
        }
        else if (Vector3.Distance(transform.position, goal.transform.position) >= attackRange)
        {
            anim.SetBool("isAttacking", false);
            agent.isStopped = false;
            anim.SetBool("isMoving", true);
            isAttaking = false;
            
        }

    }//end of enemy movment stuff
}
