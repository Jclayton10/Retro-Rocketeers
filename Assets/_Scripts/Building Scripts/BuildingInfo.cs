using System.Collections.Generic;
using UnityEngine;

public class BuildingInfo : MonoBehaviour
{
    //The offset for placing
    public Vector3 heightOffset;
    public float buildingOffset;
    public List<Transform> buildOffPoints;

    //This is going to be used with decorations, such as plants
    bool ignoreBuildOff;
}
