using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSystem : MonoBehaviour
{
    [SerializeField] GameObject buildingCamera;

    [SerializeField] GameObject buildMarker;
    [SerializeField] LayerMask layersToBeBuiltOn;
    [SerializeField] LayerMask buildings;
    [SerializeField] float snapDistance = 2;

    [SerializeField] List<GameObject> objectsThatCanBeBuilt;

    [Header("Set Dynamically")]
    [SerializeField] GameObject objectToBeBuilt;
    [SerializeField] float activeRotationAmt = 0;

    private void Update()
    {
        if (buildingCamera.activeSelf == false)
        {
            buildMarker.SetActive(false);
            return;
        }
        buildMarker.SetActive(true);

        #region Select Building System

        if (Input.GetKeyDown(KeyCode.Alpha1))
            objectToBeBuilt = objectsThatCanBeBuilt[0];
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            objectToBeBuilt = objectsThatCanBeBuilt[1];
        buildMarker.GetComponent<MeshFilter>().mesh = objectToBeBuilt.GetComponent<MeshFilter>().sharedMesh;
        buildMarker.transform.localScale = objectToBeBuilt.transform.localScale;

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
                hit.point += objectToBeBuilt.GetComponent<BuildingInfo>().heightOffset;
                hit.point += new Vector3(closestBuildingPoint.transform.position.x - closestBuildingPoint.transform.parent.position.x, 0, closestBuildingPoint.transform.position.z - closestBuildingPoint.transform.parent.position.z) * objectToBeBuilt.GetComponent<BuildingInfo>().buildingOffset;
            }
            buildMarker.transform.position = hit.point;
        }
        if (Input.GetKeyDown(KeyCode.Mouse1) && hit.point != null)
        {
            Debug.Log(objectToBeBuilt.name);
            GameObject newBuilding = Instantiate(objectToBeBuilt, hit.point, Quaternion.Euler(0, activeRotationAmt, 0));
        }
        #endregion
    }
}
