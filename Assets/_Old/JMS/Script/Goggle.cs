using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goggle : MonoBehaviour
{
    public OVRPassthroughLayer passthroughLayer;

    public void GoggleOn()
    {
        passthroughLayer.edgeRenderingEnabled = true;
        passthroughLayer.colorMapEditorBrightness = -0.4f;
    }

    public void GoggleOff()
    {
        passthroughLayer.edgeRenderingEnabled = false;
        passthroughLayer.colorMapEditorBrightness = 0f;
    }
}
