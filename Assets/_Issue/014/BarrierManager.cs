using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierManager : MonoBehaviour
{
    public int maxBarrierHits = 3;
    private int currentBarrierHits;
    private bool isBarrierActive = false;

    public float shockwaveRadius = 5f; // 충격파의 반경
    public float shockwaveForce = 10f; // 충격파의 힘
    public LayerMask enemyLayer; // 적들이 속한 레이어
    public float stopDelay = 0.1f; // 충격파 후 멈출 시간


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

        // 무적상태
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
                direction.y = 0; // 수평 방향으로만 힘을 가하기 위해 Y축 힘 제거
                enemyRigidbody.AddForce(direction.normalized * shockwaveForce, ForceMode.Impulse);

                // 적을 일정 시간 후에 멈추게 하는 코루틴 실행
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
        yield return new WaitForSeconds(2); // 2초간 무적
        GetComponent<Player>().SetInvincible(false);
    }

    public bool IsBarrierActive()
    {
        return isBarrierActive;
    }
}
