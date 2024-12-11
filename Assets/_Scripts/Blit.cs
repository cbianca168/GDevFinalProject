using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using static Blit.BlitPass;

public class Blit : ScriptableRendererFeature
{

    public class BlitPass : ScriptableRenderPass
    {
        public enum RenderTarget
        {
            Color,
            RenderTexture,
        }

        public Material blitMaterial = null;
        public int blitShaderPassIndex = 0;
        public FilterMode filterMode { get; set; }

        private RenderTargetIdentifier source { get; set; }
        private RTHandle destination { get; set; }

        RTHandle m_TemporaryColorTexture;
        string m_ProfilerTag;

        public BlitPass(RenderPassEvent renderPassEvent, Material blitMaterial, int blitShaderPassIndex, string tag)
        {
            this.renderPassEvent = renderPassEvent;
            this.blitMaterial = blitMaterial;
            this.blitShaderPassIndex = blitShaderPassIndex;
            m_ProfilerTag = tag;
            //m_TemporaryColorTexture.Init("_TemporaryColorTexture");
            m_TemporaryColorTexture = RTHandles.Alloc("_TemporaryColorTexture", name: "_TemporaryColorTexture");

           // renderTarget.Init("_ShaderProperty");
           // renderTarget = RTHandles.Alloc("_ShaderProperty", name: "_ShaderProperty");


        }

        /*public void Setup(RenderTargetIdentifier source, RTHandle destination)
        {
            this.source = source;
            this.destination = destination;
        }*/

        public void Setup(RTHandle destination)
        {
            this.destination = destination;
        }


        /*public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer cmd = CommandBufferPool.Get(m_ProfilerTag);

            RenderTextureDescriptor opaqueDesc = renderingData.cameraData.cameraTargetDescriptor;
            opaqueDesc.depthBufferBits = 0;

            // Can't read and write to same color target, use a TemporaryRT
            if (destination == renderingData.cameraData.renderer.cameraColorTargetHandle)
            {
                //cmd.GetTemporaryRT(m_TemporaryColorTexture.id, opaqueDesc, filterMode);
                cmd.GetTemporaryRT(Shader.PropertyToID(m_TemporaryColorTexture.name), opaqueDesc, filterMode);

                //cmd.GetTemporaryRT(renderTarget.id, targetDescriptor, filterMode);
                //cmd.GetTemporaryRT(Shader.PropertyToID(renderTarget.name), targetDescriptor, filterMode);

                //Blit(cmd, source, m_TemporaryColorTexture.Identifier, blitMaterial, blitShaderPassIndex);
                CoreUtils.DrawFullScreen(cmd, blitMaterial, m_TemporaryColorTexture, null, blitShaderPassIndex);

                //Blit(cmd, m_TemporaryColorTexture.Identifier(), source);
                CoreUtils.DrawFullScreen(cmd, blitMaterial, source, null, blitShaderPassIndex);
            }
            else
            {
                //Blit(cmd, source, destination.Identifier(), blitMaterial, blitShaderPassIndex);
                CoreUtils.DrawFullScreen(cmd, blitMaterial, destination, null, blitShaderPassIndex);

            }

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }*/

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer cmd = CommandBufferPool.Get(m_ProfilerTag);

            // Source is the camera color target
            var source = renderingData.cameraData.renderer.cameraColorTargetHandle;

            // If destination is null, use camera color target as destination
            var finalDestination = destination ?? renderingData.cameraData.renderer.cameraColorTargetHandle;

            // Allocate a temporary texture if needed
            if (destination == renderingData.cameraData.renderer.cameraColorTargetHandle)
            {
                m_TemporaryColorTexture = RTHandles.Alloc(
                    Vector2.one,
                    TextureXR.slices,
                    DepthBits.None,
                    renderingData.cameraData.cameraTargetDescriptor.graphicsFormat,
                    filterMode,
                    name: "_TemporaryColorTexture"
                );

                CoreUtils.DrawFullScreen(cmd, blitMaterial, m_TemporaryColorTexture, null, blitShaderPassIndex);
                CoreUtils.DrawFullScreen(cmd, blitMaterial, source, null, blitShaderPassIndex);
            }
            else
            {
                CoreUtils.DrawFullScreen(cmd, blitMaterial, finalDestination, null, blitShaderPassIndex);
            }

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void FrameCleanup(CommandBuffer cmd)
        {
            //if (destination == RenderTargetHandle.CameraTarget)
            //cmd.ReleaseTemporaryRT(Shader.PropertyToID(m_TemporaryColorTexture.name));

            // Release the temporary RTHandle if it's allocated
            if (m_TemporaryColorTexture != null)
            {
                RTHandles.Release(m_TemporaryColorTexture);
                m_TemporaryColorTexture = null;
            }
        }
    }

    [System.Serializable]
    public class BlitSettings
    {
        public RenderPassEvent Event = RenderPassEvent.AfterRenderingOpaques;

        public Material blitMaterial = null;
        public int blitMaterialPassIndex = 0;
        public Target destination = Target.Color;
        public string textureId = "_BlitPassTexture";
    }

    public enum Target
    {
        Color,
        Texture
    }

    public BlitSettings settings = new BlitSettings();
    RTHandle m_RenderTextureHandle;

    BlitPass blitPass;

    public override void Create()
    {
        var passIndex = settings.blitMaterial != null ? settings.blitMaterial.passCount - 1 : 1;
        settings.blitMaterialPassIndex = Mathf.Clamp(settings.blitMaterialPassIndex, -1, passIndex);
        blitPass = new BlitPass(settings.Event, settings.blitMaterial, settings.blitMaterialPassIndex, name);
        //m_RenderTextureHandle.Init(settings.textureId);
        m_RenderTextureHandle = RTHandles.Alloc(settings.textureId);

        // renderTarget.Init("_ShaderProperty");
        // renderTarget = RTHandles.Alloc("_ShaderProperty", name: "_ShaderProperty");

    }

    /*public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        var src = renderer.cameraColorTargetHandle;
        var dest = (settings.destination == Target.Color) ? renderingData.cameraData.renderer.cameraColorTargetHandle : m_RenderTextureHandle;

        if (settings.blitMaterial == null)
        {
            Debug.LogWarningFormat("Missing Blit Material. {0} blit pass will not execute. Check for missing reference in the assigned renderer.", GetType().Name);
            return;
        }

        blitPass.Setup(src, dest);
        renderer.EnqueuePass(blitPass);
    }*/

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (settings.blitMaterial == null)
        {
            Debug.LogWarningFormat("Missing Blit Material. {0} blit pass will not execute. Check for missing reference in the assigned renderer.", GetType().Name);
            return;
        }

        // Pass only the destination; defer source determination to the pass
        var dest = (settings.destination == Target.Color) ? null : m_RenderTextureHandle;
        blitPass.Setup(dest);
        renderer.EnqueuePass(blitPass);
    }

}
