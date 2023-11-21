using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class MinimapRenderPass : ScriptableRenderPass
{
    private Material minimapMaterial;
    private RenderTargetIdentifier cameraColorTarget;

    public MinimapRenderPass(Material minimapMaterial)
    {
        this.minimapMaterial = minimapMaterial;
    }

    public void Setup(RenderTargetIdentifier colorTarget)
    {
        this.cameraColorTarget = colorTarget;
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        if (minimapMaterial == null)
        {
            Debug.LogError("Minimap Material is not set");
            return;
        }

        CommandBuffer cmd = CommandBufferPool.Get("Render Minimap");

        RenderTextureDescriptor cameraTextureDescriptor = renderingData.cameraData.cameraTargetDescriptor;
        cameraTextureDescriptor.depthBufferBits = 0;

        // Minimap Render Texture�̍쐬
        using (new ProfilingScope(cmd, new ProfilingSampler("Minimap")))
        {
            RenderTexture minimapTexture = RenderTexture.GetTemporary(cameraTextureDescriptor);
            Blit(cmd, cameraColorTarget, minimapTexture, minimapMaterial);
            Blit(cmd, minimapTexture, cameraColorTarget);

            RenderTexture.ReleaseTemporary(minimapTexture);
        }

        context.ExecuteCommandBuffer(cmd);
        CommandBufferPool.Release(cmd);
    }
}
