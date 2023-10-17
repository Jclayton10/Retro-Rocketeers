using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Minimap : MonoBehaviour
{
    public Transform target; // The object you want to track on the minimap
    public RenderTexture minimapTexture; // The RenderTexture to render the minimap to
    public RawImage minimapImage; // The UI RawImage to display the minimap

    private Camera minimapCamera;

    private void Start()
    {
        minimapCamera = gameObject.GetComponent<Camera>();
    }

    private void OnPostRender()
    {
        Graphics.Blit(minimapImage.material.mainTexture, minimapTexture);
    }
}