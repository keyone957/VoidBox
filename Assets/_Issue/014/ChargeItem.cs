using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeItem : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        //Player 스크립트가 부착되어있다면
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            player.RechargeBarrier();
            Destroy(gameObject);
        }
        
    }
}
