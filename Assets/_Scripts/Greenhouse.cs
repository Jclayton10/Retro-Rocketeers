using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Greenhouse : MonoBehaviour
{
    private InventoryManagement inv;

    public ItemClass generated;

    public float Max = 10;

    public float TickSet;
    private float Tick;

    public int Current;
    public int chance;


    void Awake()
    {
        GameObject inventory = GameObject.Find("Inventory");
        inv = inventory.GetComponent<InventoryManagement>();

        Tick = TickSet;
    }

    void Update()
    {
        Tick -= 1 * Time.deltaTime;
        if (Tick < 0)
        {
            Tick = TickSet;
            int temp = Random.Range(0, chance);
            if (temp < 1 && Current < Max)
            {
                Current++;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && Current > 0)
        {
            inv.Add(generated, Current);
            Current = 0;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Current > 0)
        {
            inv.Add(generated, Current);
            Current = 0;
        }
    }







}
