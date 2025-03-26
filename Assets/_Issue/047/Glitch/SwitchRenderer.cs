using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class SwitchRenderer : MonoBehaviour
{
    public UniversalRenderPipelineAsset pipelineAssetA;
    public UniversalRenderPipelineAsset pipelineAssetB;

    [SerializeField] bool usePipelineA = true;
    [SerializeField] bool situationActivated = true;

    void Update()
    {
        if (!situationActivated)
        {
            if (!usePipelineA)
            {
                ChangeRenderer(pipelineAssetA);
                usePipelineA = true;
            }
        }
        else
        {
            if (usePipelineA)
            {
                ChangeRenderer(pipelineAssetB);
                usePipelineA = false;
            }
        }
    }

    void ChangeRenderer(UniversalRenderPipelineAsset pipelineAsset)
    {
        GraphicsSettings.renderPipelineAsset = pipelineAsset;
    }
}
