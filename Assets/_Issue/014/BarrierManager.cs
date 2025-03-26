using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierManager : MonoBehaviour
{
    public int maxBarrierHits = 3;
    private int currentBarrierHits;
    private bool isBarrierActive = false;

    public float shockwaveRadius = 5f; // ������� �ݰ�
    public float shockwaveForce = 10f; // ������� ��
    public LayerMask enemyLayer; // ������ ���� ���̾�
    public float stopDelay = 0.1f; // ����� �� ���� �ð�


    void Start()
    {
        currentBarrierHits = maxBarrierHits;
        isBarrierActive = true;
    }

    public void ChargeBarrier()
    {
        isBarrierActive = true;
        currentBarrierHits = Mathf.Min(currentBarrierHits + 1, maxBarrierHits);
    }

    public void TakeHit()
    {
        if (isBarrierActive)
        {
            currentBarrierHits--;
            if (currentBarrierHits <= 0)
            {
                DeactivateBarrier();
            }
        }
    }

    private void DeactivateBarrier()
    {
        isBarrierActive = false;
        TriggerShockwave();

        // ��������
        StartCoroutine(TemporaryInvincibility());
    }

    private void TriggerShockwave()
    {
        Collider[] enemies = Physics.OverlapSphere(transform.position, shockwaveRadius, enemyLayer);
        foreach (Collider enemy in enemies)
        {
            Rigidbody enemyRigidbody = enemy.GetComponent<Rigidbody>();
            if (enemyRigidbody != null)
            {
                Vector3 direction = enemy.transform.position - transform.position;
                direction.y = 0; // ���� �������θ� ���� ���ϱ� ���� Y�� �� ����
                enemyRigidbody.AddForce(direction.normalized * shockwaveForce, ForceMode.Impulse);

                // ���� ���� �ð� �Ŀ� ���߰� �ϴ� �ڷ�ƾ ����
                StartCoroutine(StopEnemy(enemyRigidbody));
            }
        }
    }

    private IEnumerator StopEnemy(Rigidbody enemyRigidbody)
    {
        yield return new WaitForSeconds(stopDelay);
        enemyRigidbody.velocity = Vector3.zero;
        enemyRigidbody.angularVelocity = Vector3.zero;
       
    }


    private IEnumerator TemporaryInvincibility()
    {
        GetComponent<Player>().SetInvincible(true);
        yield return new WaitForSeconds(2); // 2�ʰ� ����
        GetComponent<Player>().SetInvincible(false);
    }

    public bool IsBarrierActive()
    {
        return isBarrierActive;
    }
}
