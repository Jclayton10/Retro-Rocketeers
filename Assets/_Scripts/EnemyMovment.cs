using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovment : MonoBehaviour
{
  
    public Transform goal;
    //public GameObject[] enemies;

    public int health;
    public float speed;
    public float maxRange;
    public float minRange;

   
    NavMeshAgent agent; 
    // Start is called before the first frame update
    void Start()
    {

        agent = GetComponent<NavMeshAgent>();
       
    }

    // Update is called once per frame
    void Update()
    {
        if (goal != null) 
        {
            agent.SetDestination(goal.position);
        }

    }
}
