using Meta.XR.BuildingBlocks;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class Lighter : MonoBehaviour
{
    [SerializeField] GameObject flame;
    private bool OnHand = false;
    [SerializeField] BurnDoc burnDoc;


    public void LighterOnOff()
    {
        if (OnHand)
        {
            if (flame.activeSelf)
            {
                flame.SetActive(false);
                burnDoc.fireOn = false;
                //라이터 애니메이션, 소리 재생
            }
            else
            {
                flame.SetActive(true);
                burnDoc.fireOn = true;
                //라이터 애니메이션, 소리 재생
            }
        }

    }
    public void SetOnHand(bool state)
    {
        OnHand = state;
    }



}
