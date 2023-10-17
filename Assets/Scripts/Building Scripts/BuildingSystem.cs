using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Recipe
{
    public ItemClass item;
    public int amt;
}

[System.Serializable]
public struct BuildingUIItem
{
    public Vector2 loc;
    public KeyCode buttonLinked;
    public GameObject prefab;
    public List<Recipe> itemsRequired;
}

public class BuildingSystem : MonoBehaviour
{
    public static BuildingSystem buildingSystem;

    public GameObject inventoryUI;
    public GameObject hotbarUI;
    public GameObject InventorySelector;
    public GameObject BuildingSelector;
    public GameObject buildingUI;

    [SerializeField] GameObject buildingCamera;

    [SerializeField] GameObject buildMarker;
    [SerializeField] LayerMask layersToBeBuiltOn;
    [SerializeField] LayerMask buildings;
    [SerializeField] float snapDistance = 2;

    [SerializeField] List<BuildingUIItem> objectsThatCanBeBuilt;
    [SerializeField] bool canBuild;

    [Header("Materials")]
    [SerializeField] Material canBuildMat;
    [SerializeField] Material cantBuildMat;

    [Header("Set Dynamically")]
    [SerializeField] BuildingUIItem objectToBeBuilt;
    [SerializeField] float activeRotationAmt = 0;

    private void Start()
    {
        buildingSystem = this;
    }

    private void Update()
    {
        if (buildingCamera.activeSelf == false)
        {
            buildMarker.SetActive(false);
            return;
        }
        buildMarker.SetActive(true);

        #region Select Building System

        foreach(BuildingUIItem buildingItem in objectsThatCanBeBuilt)
        {
            if (Input.GetKey(buildingItem.buttonLinked))
            {
                UpdateUI(buildingItem);
            }
        }

        #endregion

        #region Rotation Of Buildings
        if (Input.GetKeyDown(KeyCode.R))
        {
            activeRotationAmt += 90f;
        }
        buildMarker.transform.rotation = Quaternion.Euler(0, activeRotationAmt, 0);
        #endregion

        #region BuildingSystem with Raycasts and Snapping
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.Log(Input.mousePosition);

        Debug.DrawRay(ray.origin, ray.direction, Color.red);

        Transform closestBuildingPoint = null;

        if (Physics.Raycast(ray, out hit, 50, layersToBeBuiltOn))
        {
            Collider[] objectsInSphere = Physics.OverlapSphere(hit.point, snapDistance, buildings);
            
            if(objectsInSphere.Length > 0)
            {
                float closestDistance = Mathf.Infinity;

                foreach(Collider collider in objectsInSphere)
                {
                    foreach (Transform buildOffPoint in collider.GetComponent<BuildingInfo>().buildOffPoints)
                    {
                        if (Vector3.Distance(buildMarker.transform.position, buildOffPoint.position) < closestDistance)
                        {
                            closestDistance = Vector3.Distance(buildMarker.transform.position, buildOffPoint.position);
                            closestBuildingPoint = buildOffPoint;
                        }
                    }
                }

                hit.point = closestBuildingPoint.position;
                hit.point += objectToBeBuilt.prefab.GetComponent<BuildingInfo>().heightOffset;
                hit.point += new Vector3(closestBuildingPoint.transform.position.x - closestBuildingPoint.transform.parent.position.x, 0, closestBuildingPoint.transform.position.z - closestBuildingPoint.transform.parent.position.z) * objectToBeBuilt.prefab.GetComponent<BuildingInfo>().buildingOffset;
            }
            buildMarker.transform.position = hit.point;
            foreach(Recipe recipe in objectToBeBuilt.itemsRequired)
            {
                if(InventoryManagement.inventoryManagement.Contains(recipe.item, recipe.amt) != null)
                {
                    continue;
                }
                canBuild = false;
            }

            if (canBuild)
                buildMarker.GetComponent<MeshRenderer>().material = canBuildMat;
            else
                buildMarker.GetComponent<MeshRenderer>().material = cantBuildMat;
        }
        if (canBuild && Input.GetKeyDown(KeyCode.Mouse1) && hit.point != null)
        {
            Debug.Log(objectToBeBuilt.prefab.name);
            GameObject newBuilding = Instantiate(objectToBeBuilt.prefab, hit.point, Quaternion.Euler(0, activeRotationAmt, 0));
        }
        #endregion
    }

    private void UpdateUI(BuildingUIItem item)
    {
        objectToBeBuilt = item;
        buildMarker.GetComponent<MeshFilter>().mesh = item.prefab.GetComponent<MeshFilter>().sharedMesh;
        buildMarker.transform.localScale = item.prefab.transform.localScale;
    }

    public void ToggleBuildMode()
    {
        if (buildingCamera.activeSelf)
        {
            inventoryUI.SetActive(false);
            hotbarUI.SetActive(false);
            buildingUI.SetActive(true);
            InventorySelector.SetActive(false);
            BuildingSelector.SetActive(true);
        }
        else
        {
            inventoryUI.SetActive(false);
            hotbarUI.SetActive(true);
            buildingUI.SetActive(false);
            InventorySelector.SetActive(true);
            BuildingSelector.SetActive(false);
        }
    }
}
