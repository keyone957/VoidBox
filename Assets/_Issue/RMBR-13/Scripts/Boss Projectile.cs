using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossProjectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField]
    private int damage = 10; // ���ط�
    [SerializeField]
    private GameObject explosionEffect; // ���� ����Ʈ ������
    [SerializeField]
    private float lifeTime = 10f; // �߻�ü�� ���� (��)

    [Header("Launch Settings")]
    [SerializeField]
    private float flightTime = 2f; // �߻� �ð� (��)
    [SerializeField]
    private Vector3 customAcceleration = Vector3.zero; // ����� ���� ���ӵ�

    private Rigidbody rb;

    private void Awake()
    {
        // Rigidbody ������Ʈ ��������
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody component missing on projectile.");
            Destroy(gameObject);
            return;
        }

        // Rigidbody ����
        rb.isKinematic = false; // ���� ������ ���� �̵�
        rb.drag = 0f; // �ӵ� ���� ����
        rb.angularDrag = 0f; // ȸ�� �ӵ� ���� ����
        rb.useGravity = false; // �߷� ��Ȱ��ȭ (����� ���� ���ӵ� ���)
    }

    /// <summary>
    /// �߻�ü ���� �޼���
    /// </summary>
    /// <param name="spawnPos">�߻� ���� ��ġ</param>
    /// <param name="targetPos">��ǥ ���� ��ġ</param>
    /// <param name="flightTimeSeconds">�߻� �ð� (��)</param>
    /// <param name="acceleration">����� ���� ���ӵ� ����</param>
    public void Setup(Vector3 spawnPos, Vector3 targetPos, float flightTimeSeconds, Vector3 acceleration)
    {
        transform.position = spawnPos;

        // �߻� �ð� ����
        flightTime = flightTimeSeconds;

        // ����� ���� ���ӵ� ����
        customAcceleration = acceleration;

        // �ʱ� �ӵ� ���
        Vector3 initialVelocity = CalculateLaunchVelocity(spawnPos, targetPos, flightTime, customAcceleration);
        rb.velocity = initialVelocity;

        Debug.Log($"Projectile Setup: StartPos={spawnPos}, TargetPos={targetPos}, FlightTime={flightTime}, Custom Acceleration={customAcceleration}, Initial Velocity={initialVelocity}");

        // �߻�ü ���� ����
        Destroy(gameObject, lifeTime);
    }

    /// <summary>
    /// �߻�ü�� ��ǥ ������ �����ϵ��� �ʱ� �ӵ� ���
    /// </summary>
    /// <param name="start">�߻� ����</param>
    /// <param name="target">��ǥ ����</param>
    /// <param name="flightTime">�߻� �ð� (��)</param>
    /// <param name="acceleration">����� ���� ���ӵ� ����</param>
    /// <returns>�ʱ� �ӵ� ����</returns>
    private Vector3 CalculateLaunchVelocity(Vector3 start, Vector3 target, float flightTime, Vector3 acceleration)
    {
        Vector3 displacement = target - start;
        Vector3 initialVelocity = (displacement - 0.5f * acceleration * Mathf.Pow(flightTime, 2)) / flightTime;
        return initialVelocity;
    }

    private void FixedUpdate()
    {
        // ����� ���� ���ӵ� ����
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
    /// �߻�ü ���� �޼���
    /// </summary>
    private void Explode()
    {
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        // �߻�ü �ı�
        Destroy(gameObject);
    }

  
}
