using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossProjectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField]
    private int damage = 10; // 피해량
    [SerializeField]
    private GameObject explosionEffect; // 폭발 이펙트 프리팹
    [SerializeField]
    private float lifeTime = 10f; // 발사체의 수명 (초)

    [Header("Launch Settings")]
    [SerializeField]
    private float flightTime = 2f; // 발사 시간 (초)
    [SerializeField]
    private Vector3 customAcceleration = Vector3.zero; // 사용자 정의 가속도

    private Rigidbody rb;

    private void Awake()
    {
        // Rigidbody 컴포넌트 가져오기
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody component missing on projectile.");
            Destroy(gameObject);
            return;
        }

        // Rigidbody 설정
        rb.isKinematic = false; // 물리 엔진에 의해 이동
        rb.drag = 0f; // 속도 감소 방지
        rb.angularDrag = 0f; // 회전 속도 감소 방지
        rb.useGravity = false; // 중력 비활성화 (사용자 정의 가속도 사용)
    }

    /// <summary>
    /// 발사체 설정 메서드
    /// </summary>
    /// <param name="spawnPos">발사 지점 위치</param>
    /// <param name="targetPos">목표 지점 위치</param>
    /// <param name="flightTimeSeconds">발사 시간 (초)</param>
    /// <param name="acceleration">사용자 정의 가속도 벡터</param>
    public void Setup(Vector3 spawnPos, Vector3 targetPos, float flightTimeSeconds, Vector3 acceleration)
    {
        transform.position = spawnPos;

        // 발사 시간 설정
        flightTime = flightTimeSeconds;

        // 사용자 정의 가속도 설정
        customAcceleration = acceleration;

        // 초기 속도 계산
        Vector3 initialVelocity = CalculateLaunchVelocity(spawnPos, targetPos, flightTime, customAcceleration);
        rb.velocity = initialVelocity;

        Debug.Log($"Projectile Setup: StartPos={spawnPos}, TargetPos={targetPos}, FlightTime={flightTime}, Custom Acceleration={customAcceleration}, Initial Velocity={initialVelocity}");

        // 발사체 수명 설정
        Destroy(gameObject, lifeTime);
    }

    /// <summary>
    /// 발사체가 목표 지점에 도달하도록 초기 속도 계산
    /// </summary>
    /// <param name="start">발사 지점</param>
    /// <param name="target">목표 지점</param>
    /// <param name="flightTime">발사 시간 (초)</param>
    /// <param name="acceleration">사용자 정의 가속도 벡터</param>
    /// <returns>초기 속도 벡터</returns>
    private Vector3 CalculateLaunchVelocity(Vector3 start, Vector3 target, float flightTime, Vector3 acceleration)
    {
        Vector3 displacement = target - start;
        Vector3 initialVelocity = (displacement - 0.5f * acceleration * Mathf.Pow(flightTime, 2)) / flightTime;
        return initialVelocity;
    }

    private void FixedUpdate()
    {
        // 사용자 정의 가속도 적용
        rb.AddForce(customAcceleration, ForceMode.Acceleration);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerAttack"))
        {
            Explode();
        }
        else if (other.CompareTag("Player"))
        {
            Status playerStatus = other.GetComponent<Status>();
            if (playerStatus != null)
            {
                playerStatus.DecreaseHP(damage);
            }
            Explode();
        }
        else if (other.CompareTag("Ground"))
        {
            Explode();
        }
    }

    /// <summary>
    /// 발사체 폭발 메서드
    /// </summary>
    private void Explode()
    {
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        // 발사체 파괴
        Destroy(gameObject);
    }

  
}
