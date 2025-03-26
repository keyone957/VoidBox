using UnityEngine;

public class ButtonLight : MonoBehaviour
{
    public Material buttonMaterial; // ��ư Material
    public Color blinkColor = Color.red; // ������ ����
    public float blinkSpeed = 2f; // ������ �ӵ�
    [SerializeField] private bool isBlinking = true; // ������ ����
    [SerializeField] private Light light;


    void Start()
    {
       
    }

    void Update()
    {
        if (isBlinking)
        {
            // Sine �Լ��� ��� ��ȭ (0~1)
            float emissionIntensity = Mathf.PingPong(Time.time * blinkSpeed, 1f);
            light.intensity = 30 * emissionIntensity;
            buttonMaterial.SetColor("_EmissionColor", blinkColor * emissionIntensity);
            DynamicGI.SetEmissive(GetComponent<Renderer>(), blinkColor * emissionIntensity);
        }
    }

    public void ToggleBlinking(bool state)
    {
        isBlinking = state; // ������ �ѱ�/����
        if (!isBlinking)
        {
            buttonMaterial.SetColor("_EmissionColor", Color.black); // Emission ����
            buttonMaterial.DisableKeyword("_EMISSION");
        }
        else
        {
            buttonMaterial.SetColor("_EmissionColor", blinkColor);
            buttonMaterial.EnableKeyword("_EMISSION");
        }
    }
}
