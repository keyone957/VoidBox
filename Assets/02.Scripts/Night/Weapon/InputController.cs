using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum eEventTypeeee
{
    OnWeaponChange,
}
public class GlobalEventDelegate
{
}
public class InputController : MonoBehaviour
{
    public static InputController Instance;

    OVRInput.Axis1D chargeBtn = OVRInput.Axis1D.PrimaryIndexTrigger;
    OVRInput.Button fireBtn = OVRInput.Button.PrimaryIndexTrigger;
    OVRInput.Button reloadBtn = OVRInput.Button.Two;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else if (Instance != null)
        {
            Destroy(this);
        }
    }
}
