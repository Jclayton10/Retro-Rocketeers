using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemProducer : MonoBehaviour
{
    [SerializeField] StorageItem itemRef;
    [SerializeField] SlotClass item;

    [SerializeField] float delay;

    [SerializeField] GameObject objectPrefab;
    [SerializeField] float productionRadius;
    [SerializeField] Vector3 conveyorBeltOffset;

    private void Start()
    {
        Invoke("ProduceItem", delay);
    }

    private void ProduceItem()
    {

        Invoke("ProduceItem", delay);
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, productionRadius);
        foreach (var hitCollider in hitColliders)
        {
            if(hitCollider.CompareTag("Conveyor"))
            {
                Instantiate(objectPrefab, hitCollider.transform.position + conveyorBeltOffset, Quaternion.identity);
                return;
            }
        }

        //No Conveyer Belts so just add to storage
        AddItem();
    }

    private void AddItem()
    {
        if (itemRef.hasSpace(item))
        {
            itemRef.AddItem(item);
        }
    }
}
