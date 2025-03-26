using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    public Light flashlight; // ������ Light
    public Texture2D cookieTexture; // Ÿ���� Light Cookie
    public float minIntensity = 0.8f; // �ּ� ���
    public float maxIntensity = 2f; // �ִ� ���
    public float flickerSpeed = 5f; // ������ �ӵ�
    private float targetIntensity;

    void Start()
    {
        if (flashlight == null)
            flashlight = GetComponent<Light>();
        flashlight.cookie = cookieTexture; // Light Cookie ����
        SetRandomIntensity();
    }

    void Update()
    {
        // ��� ����
        flashlight.intensity = Mathf.Lerp(flashlight.intensity, targetIntensity, Time.deltaTime * flickerSpeed);

        // ��ǥ ��⿡ �����ϸ� ���ο� ��ǥ ����
        if (Mathf.Abs(flashlight.intensity - targetIntensity) < 0.1f)
        {
            SetRandomIntensity();
        }
    }

    void SetRandomIntensity()
    {
        targetIntensity = Random.Range(minIntensity, maxIntensity);
    }
}
