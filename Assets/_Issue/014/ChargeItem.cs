using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeItem : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        //Player ��ũ��Ʈ�� �����Ǿ��ִٸ�
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            player.RechargeBarrier();
            Destroy(gameObject);
        }
        
    }
}
