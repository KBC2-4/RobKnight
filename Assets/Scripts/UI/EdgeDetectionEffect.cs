using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class EdgeDetectionEffect : MonoBehaviour
{
    public Shader edgeDetectShader;
    private Material edgeDetectMaterial;

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (edgeDetectMaterial == null)
        {
            edgeDetectMaterial = new Material(edgeDetectShader);
            edgeDetectMaterial.hideFlags = HideFlags.HideAndDontSave;
        }

        Graphics.Blit(src, dest, edgeDetectMaterial);
    }
}

