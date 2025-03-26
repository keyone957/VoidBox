using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using UnityEngine.XR;

public enum BossState { None = -1, Idle = 0, Wander, Pursuit, Attack, Pattern1, Pattern2, Pattern3 }

public class BossFSM : MonoBehaviour
{
    [Header("Pursuit")]
    [SerializeField]
    private float targetRecognitionRange = 15f;
    [SerializeField]
    private float pursuitLimitRange = 20f;

    [Header("Attack")]
    [SerializeField]
    private GameObject bombPrefab; // ����� ���� ���ӵ� �߻�ü ������
    [SerializeField]
    private Transform projectileSpawnPoint;
    [SerializeField]
    private float attackRange = 12f;
    [SerializeField]
    private float attackRate = 5f;
    [SerializeField]
    private float playerHeadOffset = 1.5f; // �÷��̾� �Ӹ� ���� ������
    [SerializeField]
    private float projectileFlightTime = 2f; // �߻�ü ���� �ð� (��)

    [Header("Custom Acceleration Settings")]
    [SerializeField]
    private Vector3 customAcceleration = new Vector3(0f, 0f, 0f); // ����� ���� ���ӵ�

    private BossState bossState = BossState.None;
    private float lastAttackTime = 0f;

    private Status status;
    private NavMeshAgent navMeshAgent;
    [SerializeField]
    private Transform target;

    private void Start()
    {
        Setup(null); // target�� null�� �����Ͽ� �ڵ����� �÷��̾ ã���� ��
    }

    public void Setup(Transform targetTransform)
    {
        status = GetComponent<Status>();
        navMeshAgent = GetComponent<NavMeshAgent>();

        if (targetTransform == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                this.target = player.transform;
            }
            else
            {
                Debug.LogError("Player not found. Please assign the target in the Inspector or tag the player as 'Player'.");
                return;
            }
        }
        else
        {
            this.target = targetTransform;
        }

        navMeshAgent.updateRotation = false;

        ChangeState(BossState.Idle);
    }

    private void OnEnable()
    {
        ChangeState(BossState.Idle);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        bossState = BossState.None;
    }

    public void ChangeState(BossState newState)
    {
        if (bossState == newState) return;

        Debug.Log("Changing State to: " + newState);

        StopAllCoroutines();
        bossState = newState;

        switch (bossState)
        {
            case BossState.Idle:
                StartCoroutine(Idle());
                break;
            case BossState.Wander:
                StartCoroutine(Wander());
                break;
            case BossState.Pursuit:
                StartCoroutine(Pursuit());
                break;
            case BossState.Attack:
                StartCoroutine(Attack());
                break;
            case BossState.Pattern1:
                StartCoroutine(Pattern1());
                break;
            case BossState.Pattern2:
                StartCoroutine(Pattern2());
                break;
            case BossState.Pattern3:
                StartCoroutine(Pattern3());
                break;
            default:
                Debug.LogWarning("Unhandled state: " + bossState);
                break;
        }
    }

    private IEnumerator Idle()
    {
        Debug.Log("State: Idle");
        while (bossState == BossState.Idle)
        {
            CalculateDistanceToTargetAndSelectState();
            yield return null;
        }
    }

    private IEnumerator Wander()
    {
        Debug.Log("State: Wander");
        float wanderRadius = 10f;
        float wanderTimer = 5f;
        float timer = 0f;

        navMeshAgent.speed = status.WalkSpeed;

        while (bossState == BossState.Wander)
        {
            timer += Time.deltaTime;

            if (timer >= wanderTimer)
            {
                Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
                navMeshAgent.SetDestination(newPos);
                timer = 0f;
            }

            CalculateDistanceToTargetAndSelectState();
            yield return null;
        }
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layerMask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;

        NavMeshHit navHit;

        if (NavMesh.SamplePosition(randDirection, out navHit, dist, layerMask))
        {
            return navHit.position;
        }

        return origin;
    }

    private IEnumerator Pursuit()
    {
        Debug.Log("State: Pursuit");
        navMeshAgent.speed = status.RunSpeed;

        while (bossState == BossState.Pursuit)
        {
            navMeshAgent.SetDestination(target.position);
            LookRotationToTarget();
            CalculateDistanceToTargetAndSelectState();
            yield return null;
        }
    }

    private IEnumerator Attack()
    {
        Debug.Log("State: Attack");
        navMeshAgent.ResetPath();

        // ù �߻� ���� ���� �ð��� ��
        if (Time.time - lastAttackTime < attackRate)
        {
            yield return new WaitForSeconds(attackRate - (Time.time - lastAttackTime));
        }

        while (bossState == BossState.Attack)
        {
            LookRotationToTarget();
            CalculateDistanceToTargetAndSelectState();

            if (Time.time - lastAttackTime > attackRate)
            {
                lastAttackTime = Time.time;

                // ���� ��ȯ ����
                if (ShouldChangeToPattern1())
                {
                    ChangeState(BossState.Pattern1);
                    yield break;
                }
                else if (ShouldChangeToPattern2())
                {
                    ChangeState(BossState.Pattern2);
                    yield break;
                }
                else if (ShouldChangeToPattern3())
                {
                    ChangeState(BossState.Pattern3);
                    yield break;
                }
                else
                {
                    // �⺻ ���� ����
                    PerformBasicAttack();
                }
            }
            yield return null;
        }
    }

    private void PerformBasicAttack()
    {
        // �⺻ ���� ���� ����
        Debug.Log("Performing Basic Attack!");
    }

    private bool ShouldChangeToPattern1()
    {
        // ���� ���� ������ ���� ���� ��ȯ ���� ����
        // ��: 30% Ȯ���� ����1�� ��ȯ
        return true;
    }

    private bool ShouldChangeToPattern2()
    {
        // ���� ���� ������ ���� ���� ��ȯ ���� ����
        return false; // ����: ���� ���� �� ��
    }

    private bool ShouldChangeToPattern3()
    {
        // ���� ���� ������ ���� ���� ��ȯ ���� ����
        return false; // ����: ���� ���� �� ��
    }

    private IEnumerator Pattern1() // ����� ���� ���ӵ� �߻� ����
    {
        Debug.Log("State: Pattern1");
        navMeshAgent.ResetPath();

        // ���� ���� �ִϸ��̼� ��� (��: �غ� ����)
        PlayPrePatternAnimation(1);

        // �ִϸ��̼� ��� �ð�
        yield return new WaitForSeconds(1.0f);

        // ���� ����: ����� ���� ���ӵ� �߻�
        int bombCount = 1; // �� ���� �߻�ü �߻�
        for (int i = 0; i < bombCount; i++)
        {
            FireBomb();
            yield return new WaitForSeconds(0.5f); // �߻� �� ���� ���� ����
        }

        // ���� ���� �ִϸ��̼� ��� (��: ������ ����)
        PlayPostPatternAnimation(1);

        // ���� ���� �� ���� ��ȯ
        ChangeState(BossState.Attack);
    }

    private void FireBomb()
    {
        if (bombPrefab == null)
        {
            Debug.LogError("Bomb Prefab is not assigned.");
            return;
        }

        if (target == null)
        {
            Debug.LogError("Target is not assigned.");
            return;
        }

        // �÷��̾��� ���� ��ġ�� �����ϰ�, y ���� �����Ͽ� �÷��̾� �Ӹ� ���̷� ����
        Vector3 targetPosition = target.position;
        targetPosition.y += playerHeadOffset; // �÷��̾� �Ӹ� ���̷� ����

        Debug.Log($"Firing bomb from {projectileSpawnPoint.position} towards {targetPosition}");

        // ��ź ����
        GameObject bomb = Instantiate(bombPrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
        BossProjectile bombScript = bomb.GetComponent<BossProjectile>();

        if (bombScript == null)
        {
            Debug.LogError("BossProjectile script is missing on the bombPrefab.");
            Destroy(bomb); // ��ũ��Ʈ�� ������ �߻�ü ����
            return;
        }

        // ����� ���� ���ӵ� ���� (��: �������� ���ӵ� �߰�)
        Vector3 acceleration = customAcceleration;

        // BossProjectile�� Setup �޼��带 ȣ���Ͽ� ����� ���� ���ӵ� ����
        bombScript.Setup(projectileSpawnPoint.position, targetPosition, projectileFlightTime, acceleration);

        Debug.Log($"Fired Bomb towards {targetPosition} with flight time {projectileFlightTime} seconds and acceleration {acceleration}.");
    }

    private IEnumerator Pattern2() // ���� ������ ����
    {
        Debug.Log("State: Pattern2");
        // ����2 ����
        yield return null;
    }

    private IEnumerator Pattern3() // ���� ������ ����
    {
        Debug.Log("State: Pattern3");
        // ����3 ����
        yield return null;
    }

    private void LookRotationToTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        direction.y = 0f;

        if (direction != Vector3.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 5f);
        }
    }

    private void CalculateDistanceToTargetAndSelectState()
    {
        if (target == null)
        {
            Debug.LogError("Target is null in CalculateDistanceToTargetAndSelectState()");
            return;
        }

        float distance = Vector3.Distance(target.position, transform.position);

        if (distance <= attackRange)
        {
            ChangeState(BossState.Attack);
        }
        else if (distance <= targetRecognitionRange)
        {
            ChangeState(BossState.Pursuit);
        }
        else if (distance >= pursuitLimitRange)
        {
            ChangeState(BossState.Wander);
        }
        else
        {
            ChangeState(BossState.Idle);
        }
    }

    private void PlayPrePatternAnimation(int patternNumber)
    {
        Debug.Log($"Playing Pre-Pattern {patternNumber} Animation");
        // ���� �ִϸ��̼� ��� ���� ����
    }

    private void PlayPostPatternAnimation(int patternNumber)
    {
        Debug.Log($"Playing Post-Pattern {patternNumber} Animation");
        // ���� �ִϸ��̼� ��� ���� ����
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, targetRecognitionRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, pursuitLimitRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    public void TakeDamage(int damageAmount)
    {
        bool isDie = status.DecreaseHP(damageAmount);

        if (isDie)
        {
            Debug.Log("Boss Died");
            Destroy(gameObject);
        }
    }
}