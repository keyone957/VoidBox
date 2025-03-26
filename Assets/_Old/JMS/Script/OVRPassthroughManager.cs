using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OVRPassthroughManager : MonoBehaviour
{
    public OVRPassthroughLayer passthroughLayer;
    public OVRInput.Button button;
    public OVRInput.Controller controller;
    public List<Gradient> colorMapGradients;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(button, controller))
        {
            passthroughLayer.hidden = !passthroughLayer.hidden;
        }
    }

    public void SetOpacity(float value)
    {
        passthroughLayer.textureOpacity = value;
    }

    public void SetColorMapGradient(int index)
    {
        passthroughLayer.colorMapEditorGradient = colorMapGradients[index];
    }

    public void SetBrightness(float value)
    {
        passthroughLayer.colorMapEditorBrightness = value;
    }
    public void SetContrast(float value)
    {
        passthroughLayer.colorMapEditorContrast = value;
    }
    public void SetPosterize(float value)
    {
        passthroughLayer.colorMapEditorPosterize = value;
    }

    public void SetEdgeRedering(bool value)
    {
        passthroughLayer.edgeRenderingEnabled = value;
    }

    public void SetEdgeRed(float value)
    {
        Color newColor = new Color(value, passthroughLayer.edgeColor.g, passthroughLayer.edgeColor.b);
        passthroughLayer.edgeColor = newColor;
    }

    public void SetEdgeGreen(float value)
    {
        Color newColor = new Color(passthroughLayer.edgeColor.r, value, passthroughLayer.edgeColor.b);
        passthroughLayer.edgeColor = newColor;
    }
    public void SetEdgeBlue(float value)
    {
        Color newColor = new Color(passthroughLayer.edgeColor.r, passthroughLayer.edgeColor.g, value);
        passthroughLayer.edgeColor = newColor;
    }
}
