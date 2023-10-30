using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapCamera : MonoBehaviour
{
    [SerializeField] Transform playerPos;
    [SerializeField] RenderTexture rendTexture;

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(playerPos.position.x, transform.position.y, playerPos.position.z);
    }
}
