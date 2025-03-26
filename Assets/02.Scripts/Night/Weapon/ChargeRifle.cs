using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ChargeRifle : WeaponBase
{
    private float chargeTime;
    private float maxCharge;

    private bool isCharging;
    private bool isFullCharge;

    public Slider chargeSlider;

    private void Awake()
    {
        Init();
    }

    public void SetSliderValue(float chargeTime)
    {
        chargeSlider.value = chargeTime;
    }
    public void Init()
    {
        chargeTime = 0;
        maxCharge = 2;
        isCharging = false;
        isFullCharge = false;

        chargeSlider.minValue = 0;
        chargeSlider.maxValue = maxCharge;
    }
    void Update()
    {
        Fire();
    }
    private void Fire()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.V))
        {
            if (state == WeaponState.Idle)
            {
                if (base._magazine > 0)
                {
                    StartCoroutine(StartCharging());
                }
                else StartCoroutine(base.Reload());
            }
        }

        if (Input.GetKeyUp(KeyCode.V))
        {
            if (isFullCharge)
            {
                StartCoroutine(base.SingleShot(this, BulletType.Pierce));
            }

            isCharging = false;
            isFullCharge = false;
            chargeTime = 0;
            SetSliderValue(chargeTime);
        }
#endif
        if (!isGrabed) return;

        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch) && base._magazine > 0)
        {
            if (state == WeaponState.Idle)
            {
                isCharging = true;
                StartCoroutine(StartCharging());
            }
        }
        if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
        {
            if (isFullCharge)
            {
                StartCoroutine(base.SingleShot(this, BulletType.Pierce));
            }
            isCharging = false;
            isFullCharge = false;
            chargeTime = 0;
            SetSliderValue(chargeTime);
        }
        if (OVRInput.GetDown(OVRInput.Button.Two))
        {
            StartCoroutine(base.Reload());
        }
    }
    private IEnumerator StartCharging()
    {
        isCharging = true;
        chargeTime = 0;

        while (isCharging && chargeTime < maxCharge)
        {
            chargeTime += Time.deltaTime;
            Debug.Log("차지 중: " + chargeTime);
            SetSliderValue(chargeTime);

            if (chargeTime >= maxCharge)
            {
                Debug.Log("차지 완료!");
                isFullCharge = true;
                break;
            }

            yield return null;
        }
        isCharging = false;
    }
}
