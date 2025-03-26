using UnityEngine;

public class Shotgun : WeaponBase
{
    [SerializeField] private float spreadAngle = 15f;   //���� ����
    [SerializeField] private int pelletCount;           //źȯ ��

    private void Update()
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
                if (base._magazine > 0) StartCoroutine(base.ScatterShot(this, pelletCount, spreadAngle));
            }
        }
#endif

        if (!isGrabed) return;
        
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
        {
            if (state == WeaponState.Idle)
            {
                if (base._magazine > 0) StartCoroutine(base.ScatterShot(this, pelletCount, spreadAngle));
            }
        }

        if (OVRInput.Get(OVRInput.RawButton.A) && state != WeaponState.Reloding)
        {
            StartCoroutine(base.PumpReload());
        }
    }
}