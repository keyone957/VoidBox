using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightGameSystem : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private GameObject enemySpawnPointPrefab;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private FadeScreen fadeScreen;
    [SerializeField] private WeaponInven weaponInven;
    [Header("WaveSetting")]
    [SerializeField] private float timeBetweenWave = 5f; // 웨이브와 웨이브 사이 시간.
    [SerializeField] private int maxWave; //마지막 웨이브
    [SerializeField] private int[] perWaveSecond;//웨이브 수에 따른 제한시간 설정 (맨마지막은 보스 스테이지)
    [SerializeField] private int[] perWaveMonsterCount;
    [Header("MonsterSpawnSetting")]
    [SerializeField] private float betweenEnemySpawnTime; //몬스터 소환할때 사이 시간.
    [SerializeField] private float enemySpawnLatancy; //스폰 지연시간
    [SerializeField] private Transform[] cover;
    private MemoryPool spawnPointMemoryPool;
    private MemoryPool enemyMemoryPool;
    private Vector2Int mapSize = new Vector2Int(100, 100);
    private int curWave = 0;
    private int perWaveEnemyCnt;//웨이브당 소환할 몬스터 수
    private int curWaveEnemyCnt;//현 웨이브에 남아있는 몬스터 수
    private bool isPlayerDead = false;
    private Transform targetTransform;
    private bool[] isOccupied;
    private Coroutine waveTimerCoroutine;
    private Coroutine waveCoroutine;
    private Coroutine bossStageCoroutine;
    /// //////////////////////////////////////////////////테스트용 

    private int currentSpawnIndex = 0;
    [SerializeField] private List<Transform> monsterPoints;

    /// ////////////////////////////////////////////////////////
    public event Action<int> OnWaveStart;
    public event Action<int> OnWaveEnd;
    public event Action OnAllWavesComplete;
    public event Action OnBossStageStart;
    public event Action OnBossDie;
    private void OnValidate()
    {
        //에디터 상에서 maxWave를 설정하면 각 wave마다 시간초,몬스터수 를 설정해줌
        //맨 마지막은 보스 스테이지 시간초
        maxWave = Mathf.Max(1, maxWave);
        if (perWaveSecond == null || perWaveSecond.Length != maxWave)
        {
            System.Array.Resize(ref perWaveSecond, maxWave + 1);
        }
        if (perWaveMonsterCount == null || perWaveMonsterCount.Length != maxWave)
        {
            System.Array.Resize(ref perWaveMonsterCount, maxWave);
        }
    }

    private void Awake()
    {
        spawnPointMemoryPool = new MemoryPool(enemySpawnPointPrefab);
        enemyMemoryPool = new MemoryPool(enemyPrefab);
    }


    private void Start()
    {
        isOccupied = new bool[4] { false, false, false, false };
        // waveCoroutine = StartCoroutine(RunWave());==> 웨이브 실행시키려면 주석풀기
    }

    private void Update()
    {
        // K 키를 누르면 모든 적을 제거
        if (Input.GetKeyDown(KeyCode.K))
        {
            KillAllEnemies();
        }

        // I 키를 누르면 플레이어가 죽은 상태로 설정
        if (Input.GetKeyDown(KeyCode.I))
        {
            isPlayerDead = true;
            HandlePlayerDeath();
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            SpawnEnemyTest();
        }
    }
    /// /////////////////////////////////////////////////////////////////////////////////////////////////////
    /// 테스트용 코드 ==> 추후 삭제


    private void SpawnEnemyTest()
    {
        if (monsterPoints.Count == 0) return;

        // 몬스터 소환 포인트 순차적으로 소환, 끝에 도달하면 다시 처음으로 돌아감
        GameObject spawnPoint = spawnPointMemoryPool.ActivatePoolItem();
        spawnPoint.transform.position = monsterPoints[currentSpawnIndex].position;

        StartCoroutine(TestSpawn(spawnPoint));

        currentSpawnIndex = (currentSpawnIndex + 1) % monsterPoints.Count;
    }

    private IEnumerator TestSpawn(GameObject point)
    {

        yield return new WaitForSeconds(enemySpawnLatancy); // 스폰 지연 시간
        GameObject enemy = enemyMemoryPool.ActivatePoolItem();
        enemy.transform.position = point.transform.position;
        for (int i = 0; i < isOccupied.Length; i++)
        {
            targetTransform = target;
            if (isOccupied[i] == false)
            {
                isOccupied[i] = true;
                targetTransform = cover[i];
                break;
            }
            else
            {
                continue;
            }
        }

        if (targetTransform != target)
        {
            enemy.GetComponent<CoverFSM>().SetUp2(target, targetTransform, this);
            enemy.GetComponent<EnemyFSM>().SetUp2(target, this);
        }
        else
        {
            enemy.GetComponent<EnemyFSM>().SetUp2(target, this);
        }

        spawnPointMemoryPool.DeactivatePoolItem(point);
    }
    /// ////////////////////////////////////////////////////////////////////////////////////////////////////////
    private void KillAllEnemies()
    {
        foreach (var enemy in FindObjectsOfType<EnemyFSM>())
        {
            enemy.TakeDamage(int.MaxValue);
        }
    }

    private void ResetWave()
    {
        enemyMemoryPool.DeactivateAllPoolItems();
        spawnPointMemoryPool.DeactivateAllPoolItems();
        foreach (var enemyRay in GameObject.FindGameObjectsWithTag("EnemyRay"))
        {
            Destroy(enemyRay);
        }

        foreach (var item in GameObject.FindGameObjectsWithTag("Item"))
        {
            Destroy(item);
        }
    }

    private IEnumerator RunWave()
    {
        while (curWave < maxWave)
        {
            yield return StartCoroutine(StartWave());
            if (curWave < maxWave)
            {
                yield return new WaitForSeconds(timeBetweenWave);
            }
        }
        OnAllWavesComplete?.Invoke();
        Debug.Log("모든 웨이브 클리어");
        bossStageCoroutine = StartCoroutine(StartBossStage());
    }

    private IEnumerator StartWave()
    {
        Debug.Log(curWave + 1 + "웨이브 시작");
        OnWaveStart?.Invoke(curWave + 1);

        perWaveEnemyCnt = perWaveMonsterCount[curWave];
        curWaveEnemyCnt = perWaveEnemyCnt;

        float waveDuration = perWaveSecond[curWave];
        float waveStartTime = Time.time;
        waveTimerCoroutine = StartCoroutine(WaveTimer(waveDuration));
        while (perWaveEnemyCnt > 0 && Time.time - waveStartTime < waveDuration)
        {
            if (isPlayerDead)
            {
                if (waveTimerCoroutine != null)
                {
                    StopCoroutine(waveTimerCoroutine);
                }
                yield break;
            }

            SpawnEnemy();
            perWaveEnemyCnt--;
            yield return new WaitForSeconds(betweenEnemySpawnTime);
        }

        if (waveTimerCoroutine != null)
        {
            StopCoroutine(waveTimerCoroutine);
        }

        yield return new WaitUntil(() => curWaveEnemyCnt == 0 || Time.time - waveStartTime >= waveDuration);

        curWave++;
        // weaponInven.ChangeWeaponByWave(curWave);
        // ==> 초기화 안해줘서 버그나서 일단 주석처리

        Debug.Log(curWave + "웨이브 " + (Time.time - waveStartTime >= waveDuration ? "시간 초과로 " : "") + "클리어!");
        OnWaveEnd?.Invoke(curWave);
    }

    private IEnumerator WaveTimer(float duration)
    {
        float startTime = Time.time;
        while (Time.time - startTime < duration)
        {
            Debug.Log($"웨이브 남은 시간: {duration - (Time.time - startTime):F2}초");
            yield return new WaitForSeconds(1f);
        }
    }

    private void HandlePlayerDeath()
    {
        Debug.Log("플레이어 사망," + (curWave + 1) + " 웨이브 다시 시작!");

        if (waveCoroutine != null)
        {
            StopCoroutine(waveCoroutine);
        }
        if (bossStageCoroutine != null)
        {
            StopCoroutine(bossStageCoroutine);
        }

        ResetWave();
        isPlayerDead = false; // 플레이어 상태 리셋
        fadeScreen.FadeIn(); //fadein 시작
        waveCoroutine = StartCoroutine(RunWave());
    }

    private IEnumerator StartBossStage()
    {
        Debug.Log("보스 스테이지 시작!");
        OnBossStageStart?.Invoke();
        curWaveEnemyCnt = 1;

        float bossStageDuration = perWaveSecond[maxWave]; // 보스 스테이지 시간
        float bossStageStartTime = Time.time;

        waveTimerCoroutine = StartCoroutine(WaveTimer(bossStageDuration));

        // 보스 스테이지에서 적 한 마리 소환
        //TODO: 보스몬스터 구현
        SpawnEnemy();

        yield return new WaitUntil(() => curWaveEnemyCnt == 0 || Time.time - bossStageStartTime >= bossStageDuration);

        if (waveTimerCoroutine != null)
        {
            StopCoroutine(waveTimerCoroutine);
        }

        if (curWaveEnemyCnt == 0)
        {
            Debug.Log("보스 처치 완료!");
            OnBossDie?.Invoke();
        }
        else
        {
            Debug.Log("보스 스테이지 시간 초과!");
        }
    }

    private void SpawnEnemy()
    {
        GameObject spawnPoint = spawnPointMemoryPool.ActivatePoolItem();
        spawnPoint.transform.position = GetRandomSpawnPosition();
        // StartCoroutine(SpawnEnemyAtPoint(spawnPoint));
        StartCoroutine("SpawnEnemyAtPoint", spawnPoint);
    }

    private IEnumerator SpawnEnemyAtPoint(GameObject point)
    {
        // yield return new WaitForSeconds(enemySpawnLatancy); // 스폰 지연 시간
        // GameObject enemy = enemyMemoryPool.ActivatePoolItem();
        // enemy.transform.position = point.transform.position;
        // enemy.GetComponent<EnemyFSM>().SetUp2(target, this);
        // // enemy.GetComponent<CoverFSM>().Setup(target,enemyMemoryPool);
        // //enemy.GetComponent<CoverFSM>().SetUp2(target,this);
        // spawnPointMemoryPool.DeactivatePoolItem(point);

        yield return new WaitForSeconds(enemySpawnLatancy); // 스폰 지연 시간
        GameObject enemy = enemyMemoryPool.ActivatePoolItem();
        enemy.transform.position = point.transform.position;

        // CoverFSM 및 EnemyFSM 설정
        // targetTransform = target;
        for (int i = 0; i < isOccupied.Length; i++)
        {
            targetTransform = target;
            if (isOccupied[i] == false)
            {
                isOccupied[i] = true;
                targetTransform = cover[i];
                break;
            }
            else
            {
                continue;
            }
        }

        if (targetTransform != target)
        {
            enemy.GetComponent<CoverFSM>().SetUp2(target, targetTransform, this);
        }
        else
        {
            enemy.GetComponent<EnemyFSM>().SetUp2(target, this);
        }

        spawnPointMemoryPool.DeactivatePoolItem(point);
    }

    private Vector3 GetRandomSpawnPosition()
    {
        return new Vector3(
            UnityEngine.Random.Range(-mapSize.x * 0.49f, mapSize.x * 0.49f),
            1,
            UnityEngine.Random.Range(-mapSize.y * 0.49f, mapSize.y * 0.49f)
        );
    }

    private int SetEnemyCountPerWave()
    {
        return curWave + 2;
    }

    public void EnemyDie(GameObject enemy)
    {
        enemyMemoryPool.DeactivatePoolItem(enemy);
        curWaveEnemyCnt--;
    }
}