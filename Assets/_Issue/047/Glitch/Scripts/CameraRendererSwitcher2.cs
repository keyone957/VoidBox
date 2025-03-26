using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CameraRendererSwitcher2 : MonoBehaviour
{
    public Camera camera;
    public UniversalRenderPipelineAsset urpAsset;
    public void SetRenderer(int rendererIndex)
    {
        var urp = GraphicsSettings.renderPipelineAsset as UniversalRenderPipelineAsset;
        if (urp != null && urp == urpAsset)
        {
            camera.GetUniversalAdditionalCameraData().SetRenderer(rendererIndex);
        }
    }

    [SerializeField] bool usePipelineA = true;
    [SerializeField] bool situationActivated = true;
    void Update()
    {
        if (!situationActivated)
        {
            if (!usePipelineA)
            {
                SetRenderer(0);
                usePipelineA = true;
            }
        }
        else
        {
            if (usePipelineA)
            {
                SetRenderer(1);
                usePipelineA = false;
            }
        }
    }
}

