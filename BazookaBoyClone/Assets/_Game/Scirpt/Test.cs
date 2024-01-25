using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] LineRenderer lineRenderer;

    // Update is called once per frame
    void Update()
    {
        float width = lineRenderer.startWidth;
        lineRenderer.material.SetTextureScale("_MainTex", new Vector2(1f / width, 1.0f));
    }
}
