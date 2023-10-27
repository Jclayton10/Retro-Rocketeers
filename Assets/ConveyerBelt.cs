using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyerBelt : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5;
    [SerializeField] List<Rigidbody> objectsOnConveyer = new List<Rigidbody>();

    private void Update()
    {
        foreach (Rigidbody rb in objectsOnConveyer)
        {
            rb.velocity = -transform.forward * moveSpeed;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            return;

        if(other.GetComponent<Rigidbody>() != null)
        {
            objectsOnConveyer.Add(other.GetComponent<Rigidbody>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
            return;

        if (other.GetComponent<Rigidbody>() != null)
        {
            objectsOnConveyer.Remove(other.GetComponent<Rigidbody>());
        }
    }
}
