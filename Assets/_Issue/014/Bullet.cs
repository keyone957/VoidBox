using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
        //Player ��ũ��Ʈ�� �����Ǿ��ִٸ�
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            //player.TakeDamage();
            Destroy(gameObject);
        }
        else
        {
           //���� �ε����ų� ������ destroy
            Destroy(gameObject);
        }
    }
}
