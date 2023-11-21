using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class MinimapRendererFeature : ScriptableRendererFeature
{
    [System.Serializable]
    public class MinimapSettings
    {
        public RenderPassEvent renderPassEvent = RenderPassEvent.AfterRenderingOpaques;
        public Material minimapMaterial = null;
        public Camera minimapCamera = null;
    }

    public MinimapSettings settings = new MinimapSettings();
    private MinimapRenderPass minimapRenderPass;

    public override void Create()
    {
        minimapRenderPass = new MinimapRenderPass(settings.minimapMaterial)
        {
            renderPassEvent = settings.renderPassEvent
        };
    }   

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        // ���s���Ƀ~�j�}�b�v�p�̃J������T��
        if (settings.minimapCamera == null)
        {
            settings.minimapCamera = GameObject.FindWithTag("MiniMapCamera").GetComponent<Camera>();
        }
        
        if (settings.minimapCamera != null)
        {
            Debug.Log("�~�j�}�b�v�p�J���������o���܂����B");
            minimapRenderPass.Setup(renderer.cameraColorTarget);
            renderer.EnqueuePass(minimapRenderPass);
        }
    }
}