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
                //������ �ִϸ��̼�, �Ҹ� ���
            }
            else
            {
                flame.SetActive(true);
                burnDoc.fireOn = true;
                //������ �ִϸ��̼�, �Ҹ� ���
            }
        }

    }
    public void SetOnHand(bool state)
    {
        OnHand = state;
    }



}
