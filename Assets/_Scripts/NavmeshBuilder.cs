using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics.Internal;
using UnityEngine;
using UnityEngine.AI;
using Unity.AI.Navigation;
public class NavmeshBuilder : MonoBehaviour
{
    public NavMeshSurface navMeshSurface;
    private bool isBakingInProgress = false;

    // Call this method whenever the player places a prefab with NavMeshObstacle component.
    public void BakeNavMeshOnPrefabPlacement()
    {
        if (!isBakingInProgress)
        {
            StartCoroutine(BuildNavMeshAsync());
        }
    }

    private IEnumerator BuildNavMeshAsync()
    {
        isBakingInProgress = true;

        // Disable the NavMeshSurface component temporarily to prevent automatic baking
        navMeshSurface.enabled = false;

        // Yield a frame to ensure that the previous bake request is canceled
        yield return null;

        // Re-enable the NavMeshSurface component
        navMeshSurface.enabled = true;

        // Bake the NavMesh using the NavMesh Surface component in the background
        navMeshSurface.BuildNavMesh();

        // Wait for a short time before checking if the baking is complete
        yield return new WaitForSeconds(0.1f); // Adjust the delay as needed

        while (!NavMesh.SamplePosition(Vector3.zero, out _, 0.1f, NavMesh.AllAreas))
        {
            // Continue checking until a valid NavMesh position is sampled
            yield return null;
        }

        isBakingInProgress = false;

        Debug.Log("NavMesh baking completed.");
    }
}
