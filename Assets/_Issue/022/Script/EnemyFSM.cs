using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using UnityEngine.XR;

public enum EnemyState { None = -1, Idle = 0, Wander, Pursuit, Attack, Dying}

public class EnemyFSM : MonoBehaviour
{
    [Header("Pursuit")]
    [SerializeField]
    private float targetRecognitionRange = 15;
    [SerializeField]
    private float pursuitLimitRange = 20;

    [Header("Attack")]
    [SerializeField]
    private int damage = 1;
    [SerializeField]
    private GameObject projectilePrefab;
    [SerializeField]
    private Transform projectileSpawnPoint;
    [SerializeField]
    private float attackRange = 12;
    [SerializeField]
    private float attackRate = 5;
    [SerializeField]
    private GameObject enemyRay;
    private Animator laserAnimator; // Setup시 enemyRay에서 찾아올 예정
    [SerializeField]
    private float rayRate = 3;

    [Header("Item")]
    [SerializeField]
    private GameObject item;

    [Header("Animator")]
    [SerializeField]
    private Animator animator;

    private EnemyState enemyState = EnemyState.None;
    private float lastAttackTime = 0;

    private Status status;
    private NavMeshAgent navMeshAgent;
    private Transform target;
    private EnemyMemoryPool enemyMemoryPool;
    private NightGameSystem nightGameSystem;

    private Vector3 targetPosition;
    [SerializeField] private Collider[] damageColliders;
    [SerializeField] private AudioClip[] audioClips;
    ////////////////////////////
    [SerializeField] private bool isMelee;
    public void SetUp3(Transform target)
    {
        status = GetComponent<Status>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        this.target = target;
        navMeshAgent.updateRotation = false;
        laserAnimator = enemyRay.GetComponentInChildren<Animator>(); // Setup시 enemyRay에서 찾아올 예정
    }
    ////////////////////////////
    public void Setup(Transform target, EnemyMemoryPool enemyMemoryPool)
    {
        status = GetComponent<Status>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        this.target = target;
        this.enemyMemoryPool = enemyMemoryPool;
        navMeshAgent.updateRotation = false;
        laserAnimator = enemyRay.GetComponentInChildren<Animator>(); // Setup시 enemyRay에서 찾아올 예정
    }
    public void SetUp2(Transform target, NightGameSystem nightGameSystem)
    {
        status = GetComponent<Status>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        this.target = target;
        this.nightGameSystem= nightGameSystem;

        navMeshAgent.updateRotation = false;
        laserAnimator = enemyRay.GetComponentInChildren<Animator>(); // Setup시 enemyRay에서 찾아올 예정
    }
    private void OnEnable()
    {
        ChangeState(EnemyState.Idle);
        StartCoroutine(RandomPlaySound());
    }

    private void OnDisable()
    {
        StopCoroutine(enemyState.ToString());

        enemyState = EnemyState.None;
    }

    public void ChangeState(EnemyState newState)
    {
        if(enemyState == newState) return;
        StopCoroutine(enemyState.ToString());
        animator.SetBool(enemyState.ToString(), false);
        enemyState = newState;
        StartCoroutine(enemyState.ToString());
        animator.SetBool(enemyState.ToString(), true);
    }
    private IEnumerator RandomPlaySound()
    {
        float randomTimer = UnityEngine.Random.Range(4f, 7f);
        int randomSoundClipNum = Random.Range(0, audioClips.Length);
        while (enemyState != EnemyState.Dying)
        {
            yield return new WaitForSeconds(randomTimer);
            SoundManager.instance.PlaySound(audioClips[randomSoundClipNum].name, SoundType.SFX);
            randomSoundClipNum = Random.Range(1, audioClips.Length);
            randomTimer = Random.Range(4f, 7f);
        }
    }
    private IEnumerator Dying()
    {
        StartCoroutine(DissolveAndDestroy()); // Trigger dissolve effect
        yield return new WaitForSeconds(2.0f);
    }

    private IEnumerator Idle()
    {
        StartCoroutine("AutoChangeFromIdleToWander");

        while(true)
        {
            CalculateDistanceTOTargetAndSelectState();
            yield return null;
        }
    }

    private IEnumerator AutoChangeFromIdleToWander()
    {
        int changeTime = Random.Range(1, 5);
        yield return new WaitForSeconds(changeTime);

        ChangeState(EnemyState.Wander);
    }

    private IEnumerator Wander()
    {
        while (true)
        {
            if (navMeshAgent == null)
            {
                yield return null;
            }
            else
            {
                navMeshAgent.speed = status.WalkSpeed;
                navMeshAgent.SetDestination(target.position);

                LookRotationToTarget();
                CalculateDistanceTOTargetAndSelectState();
            }
            yield return null;
        }
    }
    private Vector3 CalculateWanderPosition()
    {
        float wanderRadius = 10;
        int wanderJitter = 0;
        int wanderJitterMin = 0;
        int wanderJitterMax = 360;

        Vector3 rangePosition = Vector3.zero;
        Vector3 rangeScale = Vector3.one * 100.0f;

        wanderJitter = Random.Range(wanderJitterMin, wanderJitterMax);
        Vector3 targetPosition = transform.position + SetAngle(wanderRadius, wanderJitter);

        targetPosition.x = Mathf.Clamp(targetPosition.x, rangePosition.x - rangeScale.x * 0.5f, rangePosition.x + rangeScale.x * 0.5f);
        targetPosition.y = 0.0f;
        targetPosition.z = Mathf.Clamp(targetPosition.z, rangePosition.z - rangeScale.z * 0.5f, rangePosition.z + rangeScale.z * 0.5f);

        return targetPosition;
    }

    private Vector3 SetAngle(float radius, int angle)
    {
        Vector3 position = Vector3.zero;

        position.x = Mathf.Cos(angle) * radius;
        position.z = Mathf.Sin(angle) * radius;

        return position;
    }

    private IEnumerator Pursuit()
    {
        while (true)
        {
            navMeshAgent.speed = status.RunSpeed;
            navMeshAgent.SetDestination(target.position);

            LookRotationToTarget();
            CalculateDistanceTOTargetAndSelectState();

            yield return null;
        }
    }

    private IEnumerator Attack()
    {
        navMeshAgent.ResetPath();

        while(true)
        {
            LookRotationToTarget();
            CalculateDistanceTOTargetAndSelectState();

            if(Time.time - lastAttackTime > attackRate)
            {
                lastAttackTime = Time.time;
                // GameObject ray = Instantiate(enemyRay, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
                // ray.transform.localScale = new Vector3(0.03f, Vector3.Distance(target.position, transform.position), 0.03f);
                StartCoroutine(RayAndShoot(enemyRay));
            }
            yield return null;
        }
    }

    IEnumerator RayAndShoot(GameObject ray)
    {
        // yield return new WaitForSeconds(3.0f);
        // Destroy(ray);
        // yield return new WaitForSeconds(1.0f);
        LineRenderer lineRenderer = ray.GetComponent<LineRenderer>();
        lineRenderer.enabled = true;
        // if (lineRenderer == null)
        // {
        //     Debug.LogWarning("No LineRenderer component found on the ray GameObject!");
        //     yield break;
        // }

        // float duration = 3f; // Time to keep the ray alive
        float elapsedTime = 0f;

        while (elapsedTime < rayRate)
        {
            if (lineRenderer != null && target != null)
            {
                // Update the start and end positions of the line
                lineRenderer.SetPosition(0, projectileSpawnPoint.position);
                if (!isMelee) lineRenderer.SetPosition(1, target.position); // TODO Modify Position - new Vector3(0, -0.8f, 0));
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        lineRenderer.enabled = false;
        targetPosition = target.position;
        // clone.GetComponent<EnemyProjectile>().Setup(target.position);

        // Instantiate the ray object
        // GameObject rayInstance = Instantiate(rayPrefab, projectileSpawnPoint.position, Quaternion.identity);

        // Keep updating the ray's position and direction for 3 seconds
        // float elapsedTime = 0f;

        // while (elapsedTime < rayRate && ray != null)
        // {
        //     // Update the ray's position and rotation to point from the projectileSpawnPoint to the target
        //     ray.transform.position = projectileSpawnPoint.position;
        //     Vector3 direction = (target.position - new Vector3(0, -0.8f, 0) - projectileSpawnPoint.position).normalized;
        //     ray.transform.rotation = Quaternion.LookRotation(direction) * Quaternion.Euler(90f, 0f, 0f);

        //     elapsedTime += Time.deltaTime;
        //     yield return null;
        // }

        // // Destroy the ray object after 3 seconds
        // if (ray != null)
        // {
        //     Destroy(ray);
        // }

        yield return new WaitForSeconds(0.8f);

        //Attack
        animator.SetTrigger("attackDelayEnd");
    }

    public void OnAttack() // Called in animation event
    {
        Debug.Log("Attacked Now!!");

        if (projectilePrefab != null)
        {
            // Calculate the direction from the spawn point to the target
            Vector3 direction = (targetPosition - projectileSpawnPoint.position).normalized;

            // Shoot a ray in the calculated direction
            Ray ray = new Ray(projectileSpawnPoint.position, direction);
            RaycastHit hit;

            // Check if the ray hits something
            if (Physics.Raycast(ray, out hit))
            {
                // Check if the hit object has the "Player" tag and a Player component
                Collider other = hit.collider;
                if (other.CompareTag("Player") && other.GetComponent<Player>() != null)
                {
                    Debug.Log("Player hit!");

                    GlobalEvent.CallEvent<int>(EventType.OnPlayerHealthDecreased, damage);
                }
            }

            // Compute the rotation to look in the direction
            Quaternion rotation = Quaternion.LookRotation(direction);

            // Instantiate the clone with the pre-configured rotation
            GameObject clone = Instantiate(projectilePrefab, projectileSpawnPoint.position, rotation);
        }
        else
        {
            GlobalEvent.CallEvent<int>(EventType.OnPlayerHealthDecreased, damage); // EnemyMelee
        }
    }

    private void LookRotationToTarget()
    {
        Vector3 to = new Vector3(target.position.x, 0, target.position.z);
        Vector3 from = new Vector3(transform.position.x, 0 , transform.position.z);

        Quaternion rotation = Quaternion.LookRotation(to - from);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.01f);
    }

    private void CalculateDistanceTOTargetAndSelectState()
    {
        if (target == null) return;

        float distance = Vector3.Distance(target.position, transform.position);

        if (distance <=attackRange)
        {
            ChangeState(EnemyState.Attack);
        }
        else if (distance <= targetRecognitionRange)
        {
            ChangeState(EnemyState.Pursuit);
        }
        /*else if (distance >= pursuitLimitRange)
        {
            ChangeState(EnemyState.Wander);
        }*/
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        //Error
        //Gizmos.DrawRay(transform.position, navMeshAgent.destination - transform.position);
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, targetRecognitionRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, pursuitLimitRange);

        Gizmos.color = new Color(0.39f, 0.04f, 0.04f);
        Gizmos.DrawWireSphere(transform.position, attackRange);
        
    }

    public void TakeDamage(int damage)
    {
        bool isDie = status.DecreaseHP(damage);


        if (isDie == true)
        {
            for (int i = 0; i < damageColliders.Length; i++)
            {
                damageColliders[i].enabled = false;
            }
            NewNightScene.instance.curWaveEnemyCnt--;
            animator.SetTrigger("death");   // Animation
            // nightGameSystem.EnemyDie(gameObject);
            ChangeState(EnemyState.Dying);
            //nightGameSystem.GetComponent<EnemyMemoryPool>().DeactivateEnemy(gameObject); // Ensure enemy is deactivated properly
            //enemyMemoryPool.DeactivateEnemy(gameObject);
            
            // Instantiate(item, transform.position, transform.rotation);
        }
        else
        {
            animator.SetTrigger("hit"); // Animation
        }
    }

    private IEnumerator DissolveAndDestroy()
    {
        float dissolveDuration = 2.0f; // Duration of dissolve effect
        float dissolveThreshold = 1.0f;

        // Get all renderers in the LOD Group
        LODGroup lodGroup = GetComponentInChildren<LODGroup>();
        if (lodGroup == null)
        {
            Debug.LogWarning("No LODGroup component found.");
            Destroy(gameObject);
            yield break;
        }

        Renderer[] renderers = lodGroup.GetLODs()[0].renderers;

        // Validate all materials for "_DissolveThreshold" property
        foreach (Renderer renderer in renderers)
        {
            if(null == renderer)
            {
                continue;
            }
            if(renderer.enabled)
            {
                foreach (Material mat in renderer.materials)
                {
                    if (!mat.HasProperty("_DissolveThreshold"))
                    {
                        Debug.LogWarning("Material does not support dissolve effect.");
                        Destroy(gameObject);
                        yield break;
                    }
                }
            }
        }

        // Gradually decrease the dissolve threshold over time
        while (dissolveThreshold > 0f)
        {
            dissolveThreshold -= Time.deltaTime / dissolveDuration;

            foreach (Renderer renderer in renderers)
            {
                if(null == renderer)
                {
                    continue;
                }
                if(renderer.enabled)
                {
                    foreach (Material mat in renderer.materials)
                    {
                        mat.SetFloat("_DissolveThreshold", dissolveThreshold);
                    }
                }
            }

            yield return null; // Wait for the next frame
        }

        Destroy(gameObject); // Destroy the enemy after the dissolve effect
    }
}
