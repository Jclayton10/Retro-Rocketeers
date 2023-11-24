using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/BuildingItem")]
public class BuildingItem : ScriptableObject
{
    public Vector3 buildOffset;
    public Vector2 loc;
    public KeyCode keyLink;
    public GameObject placedPrefab; //Prefab to be placed
    public GameObject renderPrefab; //Prefab to be rendered when building

    public BuildingItem[] itemsThatCanBeBuiltInSlot;
}
