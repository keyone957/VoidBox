using UnityEngine;
using System;
using System.Collections;
public class Pistol : WeaponBase
{
    private void Update()
    {
        Fire();
    }
    private void Fire()
    {
#if UNITY_EDITOR
        if (Input.GetKey(KeyCode.V))
        {
            if (state == WeaponState.Idle)
            {
                if (base._magazine > 0)
                {
                    StartCoroutine(base.SingleShot(this));
                }
            }
        }
#endif
        //if (!isGrabed) return;

        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
        {
            if (state == WeaponState.Idle)
            {
                if (base._magazine > 0)
                {
                    StartCoroutine(base.SingleShot(this));
                }
            }
        }
        if (OVRInput.GetDown(OVRInput.RawButton.X) || OVRInput.GetDown(OVRInput.RawButton.A))
        {
            if (state != WeaponState.Reloding && state != WeaponState.Shooting)
            {
                StartCoroutine(base.Reload());
            }
        }
    }
}
