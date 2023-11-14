using System.Collections.Generic;
using UnityEngine;

public class ConveyerBelt : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5;
    private void OnTriggerStay(Collider other)
    {
        if(other.tag != "IgnoreConveyer")
        {
            if (other.transform.parent != null)
            {
                if (other.transform.parent.GetComponent<Rigidbody>() != null)
                    other.transform.parent.GetComponent<Rigidbody>().velocity = Vector3.Lerp(other.transform.parent.GetComponent<Rigidbody>().velocity, moveSpeed * transform.forward, Time.deltaTime);
            }
            else
            {
                if(other.GetComponent<Rigidbody>() != null)
                    other.GetComponent<Rigidbody>().velocity = Vector3.Lerp(other.GetComponent<Rigidbody>().velocity, moveSpeed * transform.forward, Time.deltaTime);
            }
        }
    }
}
