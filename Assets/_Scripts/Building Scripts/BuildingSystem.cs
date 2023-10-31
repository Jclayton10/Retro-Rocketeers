using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct BuildingUIItem
{
    public Vector2 loc;
    public KeyCode buttonLinked;
    public GameObject prefab;
}

public class BuildingSystem : MonoBehaviour
{
    public GameMaster GM;


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

    [Header("Set Dynamically")]
    [SerializeField] GameObject objectToBeBuilt;
    [SerializeField] float activeRotationAmt = 0;

    [Header("Audio")]
    public List<AudioClip> BuildSounds;
    public AudioSource Kit;

    private void Start()
    {
        buildingSystem = this;

        GameObject gm = GameObject.Find("Game Master");
        GM = gm.GetComponent<GameMaster>();
        Kit.volume = GM.AudioMaster * GM.AudioSFX;
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

        foreach (BuildingUIItem buildingItem in objectsThatCanBeBuilt)
        {
            if (Input.GetKey(buildingItem.buttonLinked))
            {
                UpdateUI(buildingItem);
            }
        }

        #endregion

        #region Rotation Of Buildings
        if (Input.GetKeyDown(GameMaster.Instance.rotateKey))
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

            if (objectsInSphere.Length > 0)
            {
                float closestDistance = Mathf.Infinity;

                foreach (Collider collider in objectsInSphere)
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
        if (Input.GetKeyDown(GameMaster.Instance.buildingKey) && hit.point != null)
        {
            Debug.Log(objectToBeBuilt.name);

            Kit.clip = BuildSounds[Random.Range(0, BuildSounds.Count)];
            Kit.Play();

            GameObject newBuilding = Instantiate(objectToBeBuilt, hit.point, Quaternion.Euler(0, activeRotationAmt, 0));
        }
        #endregion
    }

    private void UpdateUI(BuildingUIItem item)
    {
        objectToBeBuilt = item.prefab;
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
