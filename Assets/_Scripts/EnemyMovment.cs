using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovment : MonoBehaviour
{

    public Transform goal;
    //public GameObject[] enemies;

    public int currentHealth;
    public float speed;
    public float maxRange;
    public float minRange;



    NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {

        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;

    }

    // Update is called once per frame
    void Update()
    {

        /* if (goal != null)
         {
         float distanceToGoal = Vector3.Distance(transform.position, goal.position);
         if (distanceToGoal >= minRange && distanceToGoal <= maxRange) 
         {
             agent.SetDestination(goal.position);
         }
         else
         {
             agent.ResetPath();
         }

         }
        */



    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Damage Taken: " + damage);
        Debug.Log("Current Health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    public void Die()
    {
        if (currentHealth <=0)
        {
            Destroy(gameObject);
        }
    }
}
