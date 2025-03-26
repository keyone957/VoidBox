using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using UnityEngine.XR;

// EnemyState { None = -1, Idle = 0, Wander, Pursuit, Attack, }
public class SuicideFSM : MonoBehaviour
{
    [Header("Pursuit")]
    [SerializeField]
    private float targetRecognitionRange = 15;
    [SerializeField]
    private float pursuitLimitRange = 20;

    [Header("Attack")]
    [SerializeField]
    private float explodeRange = 5;
    [SerializeField]
    private int damage = 5;
    [SerializeField]
    private float timeToExplode = 3f;
    [SerializeField]
    private GameObject explosionPrefab;
    public float timeWithinAttackRange;

    [Header("Item")]
    [SerializeField]
    private GameObject item;

    // [Header("Animator")] // No animator in SuicideFSM
    // [SerializeField]
    // private Animator animator;

    private EnemyState enemyState = EnemyState.None;
    // private float lastAttackTime = 0;

    private Status status;
    private NavMeshAgent navMeshAgent;
    private Transform target;
    private EnemyMemoryPool enemyMemoryPool;
    private NightGameSystem nightGameSystem;
    [SerializeField] private Collider[] damageColliders;
    [SerializeField] private AudioClip[] audioClips;

    ////////////////////////////
    public void SetUp3(Transform target)
    {
        status = GetComponent<Status>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        this.target = target;
        navMeshAgent.updateRotation = false;
    }
    ////////////////////////////
    public void Setup(Transform target, EnemyMemoryPool enemyMemoryPool)
    {
        status = GetComponent<Status>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        this.target = target;
        this.enemyMemoryPool = enemyMemoryPool;
        navMeshAgent.updateRotation = false;
    }
    public void SetUp2(Transform target, NightGameSystem nightGameSystem)
    {
        status = GetComponent<Status>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        this.target = target;
        this.nightGameSystem= nightGameSystem;

        navMeshAgent.updateRotation = false;

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
        // animator.SetBool(enemyState.ToString(), false); // Animation
        enemyState = newState;
        StartCoroutine(enemyState.ToString());
        // animator.SetBool(enemyState.ToString(), true); // Animation
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
    private IEnumerator RandomPlaySound()
    {
        float randomTimer = UnityEngine.Random.Range(4f, 7f);
        int randomSoundClipNum = Random.Range(1, audioClips.Length);
        while (enemyState != EnemyState.Dying)
        {
            yield return new WaitForSeconds(randomTimer);
            SoundManager.instance.PlaySound(audioClips[randomSoundClipNum].name, SoundType.SFX);
            randomSoundClipNum = Random.Range(1, audioClips.Length);
            randomTimer = Random.Range(4f, 7f);
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
        timeWithinAttackRange = 0f;

        while(true)
        {
            LookRotationToTarget();
            CalculateDistanceTOTargetAndSelectState();

            timeWithinAttackRange += Time.deltaTime;

            if(timeWithinAttackRange >= timeToExplode)
            {
                StartCoroutine(Explode());
                yield break;
            }
            yield return null;
        }
    }

    IEnumerator Explode()
    {
        // Find all colliders within the range
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explodeRange);
        
        foreach (Collider hit in hitColliders)
        {
            // Check if the collider belongs to an object tagged "Player" and has a <Player> component
            if (hit.CompareTag("Player") && hit.GetComponent<Player>() is not null)
            {
                var playerComponent = hit.GetComponent<Player>();
                if (playerComponent != null)
                {
                    // Deal damage using the provided GlobalEvent system
                    GlobalEvent.CallEvent<int>(EventType.OnPlayerHealthDecreased, damage);
                    Debug.Log("Damage dealt to the player: " + damage);
                    SoundManager.instance.PlaySound("Night1_EnemySuicide",SoundType.SFX,false);
                }
            }
        }

        Instantiate(explosionPrefab, transform.position, transform.rotation);

        TakeDamage(status.MaxHP); // Suicide
        // TODO change to CurrentHP?
        yield return null;
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
        if (target == null)
        {
            Debug.Log("TargetISNull");
            return;
        }
        float distance = Vector3.Distance(target.position, transform.position);

        if (distance <=explodeRange)
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
        Gizmos.DrawWireSphere(transform.position, explodeRange);
        
    }

    public void TakeDamage(int damage)
    {
        bool isDie = status.DecreaseHP(damage);

        // animator.SetTrigger("hit"); // Animation

        if (isDie == true)
        {
            for (int i = 0; i < damageColliders.Length; i++)
            {
                damageColliders[i].enabled = false;
            }
            NewNightScene.instance.curWaveEnemyCnt--;
            // animator.SetTrigger("death");   // Animation
            
            // nightGameSystem.EnemyDie(gameObject);
            ChangeState(EnemyState.Dying);

            //nightGameSystem.GetComponent<EnemyMemoryPool>().DeactivateEnemy(gameObject); // Ensure enemy is deactivated properly
            //enemyMemoryPool.DeactivateEnemy(gameObject);
            
            // Instantiate(item, transform.position, transform.rotation);
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
