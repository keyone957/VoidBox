using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    public Light flashlight; // 손전등 Light
    public Texture2D cookieTexture; // 타원형 Light Cookie
    public float minIntensity = 0.8f; // 최소 밝기
    public float maxIntensity = 2f; // 최대 밝기
    public float flickerSpeed = 5f; // 깜빡임 속도
    private float targetIntensity;

    void Start()
    {
        if (flashlight == null)
            flashlight = GetComponent<Light>();
        flashlight.cookie = cookieTexture; // Light Cookie 설정
        SetRandomIntensity();
    }

    void Update()
    {
        // 밝기 보간
        flashlight.intensity = Mathf.Lerp(flashlight.intensity, targetIntensity, Time.deltaTime * flickerSpeed);

        // 목표 밝기에 도달하면 새로운 목표 설정
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
