using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class OnHitEffectController : MonoBehaviour
{
    [SerializeField] private Image bloodEffectImage;

    public Volume OnHitVolume; // Global Volume 오브젝트 연결
    private Vignette vignette; // Vignette 효과
    
    private bool isHit = false;
    public float fadeSpeed = 1.0f; // 비네트가 사라지는 속도
    public Color hitColor = Color.red; // 피격 시 비네트 색상

    private Color startColor;
    private float startAlpha = 0.1f;
    private float duration = 1f;

    void Start()
    {
        startColor = bloodEffectImage.color;
        startColor.a = startAlpha;
        bloodEffectImage.color = startColor;
        bloodEffectImage.enabled = false;

        if (OnHitVolume.profile.TryGet(out vignette))
        {
            // 초기 강도 설정 (보이지 않게)
            vignette.intensity.value = 0f;
            vignette.color.value = hitColor;
        }
        GlobalEvent.RegisterEvent(EventType.OnPlayerHitEffect, StartBloodEffect);
    }

    void Update()
    {
        // 디버그용 코드: 마우스 왼쪽 버튼 클릭 시 피격 효과 트리거
        if (Input.GetMouseButtonDown(0))
        {
            StartBloodEffect();
        }

        // 비네트 강도 감소 처리
        if (isHit)
        {
            vignette.intensity.value = Mathf.Lerp(vignette.intensity.value, 0f, fadeSpeed * Time.deltaTime);

            if (vignette.intensity.value <= 0.01f)
            {
                vignette.intensity.value = 0f;
                isHit = false;
            }
        }
    }
    private void StartBloodEffect()
    {
        StartCoroutine(HitEffectRoutine());
    }
    private IEnumerator HitEffectRoutine()
    {
        float alpha = bloodEffectImage.color.a;
        float startAlpha = alpha;
        float timer = 0;
        Color color = bloodEffectImage.color;
        bloodEffectImage.enabled = true;
        while (timer <= duration)
        {
            timer += Time.deltaTime;
            alpha = Mathf.Lerp(startAlpha, 0f, timer / duration);
            color.a = alpha;
            bloodEffectImage.color = color;
            yield return null;
        }
        Debug.Log("End Effect");
        bloodEffectImage.enabled = false;
        bloodEffectImage.color = startColor;
    }
    public void OnHit()
    {
        // 피격 시 강도를 높여 비네트 효과 활성화
        if (vignette != null)
        {
            vignette.intensity.value = 0.5f; // 비네트 강도 조절 가능
            isHit = true;
        }
    }
}
