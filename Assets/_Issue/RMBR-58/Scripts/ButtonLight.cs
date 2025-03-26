using UnityEngine;

public class ButtonLight : MonoBehaviour
{
    public Material buttonMaterial; // ¹öÆ° Material
    public Color blinkColor = Color.red; // ±ôºýÀÓ »ö»ó
    public float blinkSpeed = 2f; // ±ôºýÀÓ ¼Óµµ
    [SerializeField] private bool isBlinking = true; // ±ôºýÀÓ »óÅÂ
    [SerializeField] private Light light;


    void Start()
    {
       
    }

    void Update()
    {
        if (isBlinking)
        {
            // Sine ÇÔ¼ö·Î ¹à±â º¯È­ (0~1)
            float emissionIntensity = Mathf.PingPong(Time.time * blinkSpeed, 1f);
            light.intensity = 30 * emissionIntensity;
            buttonMaterial.SetColor("_EmissionColor", blinkColor * emissionIntensity);
            DynamicGI.SetEmissive(GetComponent<Renderer>(), blinkColor * emissionIntensity);
        }
    }

    public void ToggleBlinking(bool state)
    {
        isBlinking = state; // ±ôºýÀÓ ÄÑ±â/²ô±â
        if (!isBlinking)
        {
            buttonMaterial.SetColor("_EmissionColor", Color.black); // Emission ²ô±â
            buttonMaterial.DisableKeyword("_EMISSION");
        }
        else
        {
            buttonMaterial.SetColor("_EmissionColor", blinkColor);
            buttonMaterial.EnableKeyword("_EMISSION");
        }
    }
}
