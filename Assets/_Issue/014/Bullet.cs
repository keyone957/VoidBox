using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
        //Player 스크립트가 부착되어있다면
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            //player.TakeDamage();
            Destroy(gameObject);
        }
        else
        {
           //벽에 부딪히거나 했을때 destroy
            Destroy(gameObject);
        }
    }
}
