using System.Collections;
using System.Collections.Generic;
using Unity.Profiling;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[System.Serializable]
public struct buildingCell
{
    public BuildingItem item;
    public GameObject objectInCell;

    public List<GameObject> otherObjectsInCell;
}

[System.Serializable]
public struct buildingGridCell
{
    public buildingCell[] cellsInGridCell;
}

public class NewBuildingSystem : MonoBehaviour
{
    public GameMaster gameMaster;
    public static NewBuildingSystem Instance;

    [Header("Building Variables")]
    [SerializeField] float gridSize;
    [SerializeField] LayerMask layersToBeBuiltOn;

    [SerializeField] int buildGridWidth, buildGridLength, buildGridHeight;
    [SerializeField] buildingGridCell[,] buildingGrid;
    [SerializeField] int buildHeight = 0;
    [SerializeField] float buildStep = 5;
    [SerializeField] Vector2 buildingOffset;

    [Header("Camera")]
    [SerializeField] GameObject buildingCamera;

    [Header("GameObjects")]
    [SerializeField] GameObject buildMarker;
    [SerializeField] BuildingItem itemToBuild;

    [Header("Building UI")]
    [SerializeField] List<BuildingItem> itemsThatCanBeBuilt;

    [Header("Audio")]
    public List<AudioClip> BuildSounds;
    public AudioSource Kit;

    [Header("KeyCodes")]
    [SerializeField] KeyCode buildUp;
    [SerializeField] KeyCode buildDown;

    [Header("Transforms")]
    [SerializeField] Transform playerTransform;

    float activeRotationAmt = 0f;
    GameObject objectToBeBuilt;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        GameObject gm = GameObject.Find("Game Master");
        gameMaster = gm.GetComponent<GameMaster>();

        Kit.volume = gameMaster.AudioMaster * gameMaster.AudioSFX;

        UpdateUI(itemsThatCanBeBuilt[0]);
        buildingOffset = new Vector2(Mathf.FloorToInt(buildGridWidth / 2), Mathf.FloorToInt(buildGridLength / 2));

        #region Initialize Building Grid
        buildingGrid = new buildingGridCell[buildGridWidth, buildGridLength];

        for (int i = 0; i < buildGridWidth; i++)
        {
            for(int f = 0; f < buildGridLength; f++)
            {
                buildingGrid[i, f].cellsInGridCell = new buildingCell[buildGridHeight];
            }
        }
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        //Only build when buildingCamera is active
        if (buildingCamera.activeSelf == false)
        {
            buildMarker.SetActive(false);
            return;
        }


        buildMarker.SetActive(true);


        #region Select Building System
        //Sets the Building object based on key pressed

        foreach (BuildingItem buildingItem in itemsThatCanBeBuilt)
        {
            if (Input.GetKey(buildingItem.keyLink))
            {
                UpdateUI(buildingItem);
            }
        }

        #endregion

        #region Rotation Of Buildings
        //Sets the rotation of the buildingObject
        if (GameMaster.Instance.RotateJustPressed)
        {
            activeRotationAmt += 90f;
        }
        buildMarker.transform.rotation = Quaternion.Euler(0, activeRotationAmt, 0);
        #endregion

        #region Height Setting
        if (Input.GetKeyDown(buildUp))
            buildHeight++;
        if(Input.GetKeyDown(buildDown))
            buildHeight--;

        buildHeight %= buildGridHeight - 1;

        if (buildHeight < 0)
            buildHeight = buildGridHeight;

        #endregion

        #region BuildingSystem

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //Debug.Log(Input.mousePosition);

        Debug.DrawRay(ray.origin, ray.direction, Color.red);

        if (Physics.Raycast(ray, out hit, 50, layersToBeBuiltOn))
        {
            Debug.Log($"{hit.collider.name}, {hit.collider.gameObject.layer}");
            Vector3 hitPoint = Vector3.zero;
            Vector2 gridSlot = Vector2.zero;

            //Ground Logic
            if (hit.collider.gameObject.layer == 6)
            {
                gridSlot = new Vector2(Mathf.FloorToInt(hit.point.x / gridSize), Mathf.FloorToInt(hit.point.z / gridSize)) + buildingOffset;
                //Debug.Log($"Grid Slot: {gridSlot}");
                //Debug.Log("Hitting Ground");
                //Calculate the buildingPoint
                hitPoint = hit.point;
                hitPoint.x = Mathf.FloorToInt(hitPoint.x / gridSize) * gridSize + gridSize / 2;
                hitPoint.z = Mathf.FloorToInt(hitPoint.z / gridSize) * gridSize + gridSize / 2;
                hitPoint.y = buildHeight * buildStep;
                buildMarker.transform.position = hitPoint;
            }

            //Building Logic
            if (hit.collider.gameObject.layer == 7)
            {
                gridSlot = new Vector2(Mathf.FloorToInt(hit.point.x / gridSize), Mathf.FloorToInt(hit.point.z / gridSize)) + buildingOffset;
                //Debug.Log($"Grid Slot: {gridSlot}");
                //Debug.Log("Hitting Ground");
                //Calculate the buildingPoint
                hitPoint = hit.point;

                hitPoint.y = buildHeight * gridSize;

                //Ceiling Logic / Building Upwards Logic (Values don't need to be changed)
                hitPoint.x = Mathf.FloorToInt(hitPoint.x / gridSize) * gridSize + gridSize / 2;
                hitPoint.z = Mathf.FloorToInt(hitPoint.z / gridSize) * gridSize + gridSize / 2;

                //Building On Same Level Logic
                if(buildHeight == (int)(hitPoint.y / gridSize))
                {
                    Vector2 TopDownDirection = (new Vector2(playerTransform.transform.position.x, playerTransform.transform.position.z) - new Vector2(hitPoint.x, hitPoint.z)).normalized;

                    //Vertical Value
                    if (Mathf.Abs(TopDownDirection.x) > Mathf.Abs(TopDownDirection.y))
                    {
                        if (TopDownDirection.x < 0)
                            TopDownDirection = Vector2.right;
                        else
                            TopDownDirection = Vector2.left;
                    }
                    else
                    {
                        if (TopDownDirection.y < 0)
                            TopDownDirection = Vector2.up;
                        else
                            TopDownDirection = Vector2.down;
                    }

                    Debug.Log($"TopDownDirection: {TopDownDirection}");

                    hitPoint.x += TopDownDirection.x * gridSize;
                    hitPoint.z += TopDownDirection.y * gridSize;

                    gridSlot += TopDownDirection;
                }

                buildMarker.transform.position = hitPoint;
            }

            /*
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
            */
            hitPoint += Quaternion.Euler(0, activeRotationAmt + 90, 0) * itemToBuild.buildOffset;

            buildMarker.transform.position = hitPoint;

            Debug.Log(buildingGrid[(int)gridSlot.x, (int)gridSlot.y].cellsInGridCell[buildHeight].objectInCell);

            if (GameMaster.Instance.BuildJustPressed && hitPoint != Vector3.zero)
            {
                //Kit.clip = BuildSounds[Random.Range(0, BuildSounds.Count)];
                //Kit.Play();
                if (buildingGrid[(int)gridSlot.x, (int)gridSlot.y].cellsInGridCell[buildHeight].objectInCell == null)
                {
                    GameObject newBuilding = Instantiate(objectToBeBuilt, hitPoint, Quaternion.Euler(0, activeRotationAmt, 0));
                    buildingGrid[(int)gridSlot.x, (int)gridSlot.y].cellsInGridCell[buildHeight].objectInCell = newBuilding;
                    buildingGrid[(int)gridSlot.x, (int)gridSlot.y].cellsInGridCell[buildHeight].item = itemToBuild;
                }
                else
                {
                    foreach(BuildingItem item in buildingGrid[(int)gridSlot.x, (int)gridSlot.y].cellsInGridCell[buildHeight].item.itemsThatCanBeBuiltInSlot)
                    {
                        if(item == itemToBuild)
                        {
                            GameObject newBuilding = Instantiate(objectToBeBuilt, hitPoint, Quaternion.Euler(0, activeRotationAmt, 0));
                            if (buildingGrid[(int)gridSlot.x, (int)gridSlot.y].cellsInGridCell[buildHeight].otherObjectsInCell == null)
                                buildingGrid[(int)gridSlot.x, (int)gridSlot.y].cellsInGridCell[buildHeight].otherObjectsInCell = new List<GameObject>();
                            buildingGrid[(int)gridSlot.x, (int)gridSlot.y].cellsInGridCell[buildHeight].otherObjectsInCell.Add(newBuilding);
                            buildingGrid[(int)gridSlot.x, (int)gridSlot.y].cellsInGridCell[buildHeight].item = itemToBuild;
                        }
                    }
                }
            }
        }
        /*
        if (GameMaster.Instance.BuildJustPressed && hit.point != null)
        {
            Debug.Log(objectToBeBuilt.name);

            Kit.clip = BuildSounds[Random.Range(0, BuildSounds.Count)];
            Kit.Play();

            GameObject newBuilding = Instantiate(objectToBeBuilt, hit.point, Quaternion.Euler(0, activeRotationAmt, 0));
        }
        */

        #endregion
    }

    private void UpdateUI(BuildingItem item)
    {
        itemToBuild = item;
        if (item.renderPrefab.GetComponent<MeshFilter>())
            buildMarker.GetComponent<MeshFilter>().mesh = item.renderPrefab.GetComponent<MeshFilter>().sharedMesh;
        buildMarker.transform.localScale = item.renderPrefab.transform.localScale;
        objectToBeBuilt = item.placedPrefab;
    }
}
