using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEditor.Rendering;
using UnityEngine;

public class NewNightScene : MonoBehaviour
{
    public static NewNightScene instance { get; private set; }
    private enum NightState
    {
        None,
        Tutorial,
        EndTutorial,
        WaveStart,
        GameEnd,
    }
    [SerializeField] private Transform target;
    [SerializeField] private GameObject normalEnemy;
    [SerializeField] private GameObject rangeEnemy;
    [SerializeField] private GameObject coverEnemy;
    [SerializeField] private GameObject suicideEnemy;
    [SerializeField] private FadeScreen fadeScreen;
    [SerializeField] private WeaponInven weaponInven;
    [Header("WaveSetting")]
    [Tooltip("웨이브와 웨이브 사이시간")]
    [SerializeField] private float timeBetweenWave; // 웨이브와 웨이브 사이 시간.
    [SerializeField] private int maxWave; //마지막 웨이브
    [SerializeField] private int[] perWaveSecond; //웨이브 수에 따른 제한시간 설정 (맨마지막은 보스 스테이지)
    [SerializeField] private  WaveInfoSO[] waveInfo;
    [Header("MonsterSpawnSetting")]
    [SerializeField] private Transform[] cover;//엄폐물
    [Header("각 웨이브별 몬스터 소환위치")] 
    [SerializeField] private GameObject[] perWaveSpawnPoints;

    [SerializeField] private ExplosionForce explodeMgr;

    [Space]
    [SerializeField] private NightState currentState;
    [SerializeField] private NightState nextState;
    [SerializeField] private string[] tutorialUINames = { "Pointer", "Shot", "Reload" };
    [SerializeField] private float bgmControlTimer;
    private int curWave = 0;
    public int curWaveEnemyCnt=0; //현 웨이브에 남아있는 몬스터 수
    private bool isPlayerDead = false;
    private Transform targetTransform;
    private bool[] isOccupied;
    private WaveInfoSO curWaveInfo;
    
    private Coroutine waveTimerCoroutine;
    private Coroutine waveCoroutine;
    private Coroutine bossStageCoroutine;

    public event Action<int> OnWaveStart;
    public event Action<int> OnWaveEnd;
    public event Action OnAllWavesComplete;

    public MeshRenderer[] MeshRenderer;
    public GameObject[] passthroughWalls;
    private void OnValidate()
    {
        //에디터 상에서 maxWave를 설정하면 각 wave마다 시간초,몬스터수 를 설정해줌
        //맨 마지막은 보스 스테이지 시간초
        maxWave = Mathf.Max(1, maxWave);
        if (perWaveSecond == null || perWaveSecond.Length != maxWave)
        {
            System.Array.Resize(ref perWaveSecond, maxWave);
        }
        if (waveInfo == null || waveInfo.Length != maxWave)
        {
            System.Array.Resize(ref waveInfo,maxWave);
        }

        if (perWaveSpawnPoints == null || perWaveSpawnPoints.Length != maxWave)
        {
            System.Array.Resize(ref perWaveSpawnPoints,maxWave);
        }
    }
    void Awake()
    {
        if (null == instance)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        isOccupied = new bool[5] { false, false, false, false, false };
        // nextState = NightState.Tutorial;
        // foreach (var t in MeshRenderer) 
        // {
        //     t.sortingOrder = 0;
        // }
        nextState = NightState.Tutorial;
        SoundManager.instance.PlaySound("Night1_벽이 무너지기 전", SoundType.BGM, true);
        foreach (var t in MeshRenderer)
        {
            t.sortingOrder = 0;
        }
        if (SceneDataManager.instance != null && SceneDataManager.instance.WaveNum > 0)
        {
            curWave = SceneDataManager.instance.WaveNum-1; // 0-based index로 변환
            StartCoroutine(StartTutorial());
        }
        else
        {
            StartCoroutine(StartTutorial());
        }

    }

    #region Tutorial
    public void Update()
    {
        currentState = nextState;
        if (currentState != nextState)
        {
            currentState = nextState;
            switch (currentState)
            {
                case NightState.None:
                    nextState = NightState.Tutorial;
                    break;
                case NightState.Tutorial:
                    break;
                case NightState.EndTutorial:
                    break;
                case NightState.WaveStart:
                    Debug.Log("넘어오니?");
                    StartCoroutine(DelayedStartWave(10f));
                    WeaponCollection.instance.weaponUI.gameObject.SetActive(true);
                    break;
            }
        }
    }

    /// <summary>
    /// 추후 분리
    /// </summary>
    /// <returns></returns>
    private IEnumerator StartTutorial()
    {
        Debug.Log("StartTutorial");
        while (TutorialUI.Instance is null || Input.GetKey(KeyCode.L)) yield return null;

        var tutorialUI = TutorialUI.Instance;
        tutorialUI.Init();

        DialogManager.instance.ShowDialog("F1");
        Debug.Log($"why {tutorialUINames[0]} : {tutorialUI._Images[0].imageName}");
        TutorialUI.Instance.ShowImage(tutorialUI.defaultPosition, "Pointer", true);

        while (!WristWatchUI.Instance.gameObject.activeInHierarchy || Input.GetKey(KeyCode.L)) yield return null;
        tutorialUI.stopCoroutine = true;

        
        tutorialUI.ShowImage(tutorialUI.defaultPosition, "Shot", true);

        bool boolean = false;
        while (WeaponCollection.instance.currentWeapon is null || Input.GetKey(KeyCode.L)) yield return null;
        DialogManager.instance.ShowDialog("F1A");
        var currentWeapon = WeaponCollection.instance.currentWeapon;
        
        while (!boolean)
        {
            if (Input.GetKey(KeyCode.L)) break;
            if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, OVRInput.Controller.RTouch))
            {
                StartCoroutine(currentWeapon.SingleShot(currentWeapon));
                boolean = true;
            }
            yield return null;
        }
        var isReloaded = false;

        tutorialUI.stopCoroutine = true;

        DialogManager.instance.ShowDialog("F1B");
        tutorialUI.ShowImage(tutorialUI.defaultPosition, "Reload", true);

        while (!isReloaded)
        {
            if (Input.GetKey(KeyCode.L)) break;
            if (OVRInput.GetDown(OVRInput.RawButton.X) || OVRInput.GetDown(OVRInput.RawButton.A))
            {
                StartCoroutine(currentWeapon.Reload());

                break;
            }
            yield return null;
        }
        tutorialUI.stopCoroutine = true;
        DialogManager.instance.ShowDialog("F2");
        GlobalEvent.CallEvent(EventType.UpdateWeaponUI);
        nextState = NightState.EndTutorial;
        explodeMgr.Explode();

        EndingEvent.instance.ConvertCombatEnv();
        SoundManager.instance.FadeOutBGM(bgmControlTimer);
        SoundManager.instance.DelayPlaySound("Night1_벽 무너진 후_전투 시작 전", SoundType.BGM, bgmControlTimer + 0.5f, true);
        yield return new WaitForSeconds(10f);

        currentWeapon.EndTutorial();
        Debug.LogWarning("웨이브 스타트");
        //nextState = NightState.WaveStart;
        StartCoroutine(DelayedStartWave(10f));

        SoundManager.instance.FadeOutBGM(bgmControlTimer);
        SoundManager.instance.DelayPlaySound("Night1_벽 무너지고 적이 첫 진입할 시 소리", SoundType.BGM, bgmControlTimer + 0.5f, true);
    }

    #endregion
    private IEnumerator DelayedStartWave(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        Debug.Log("딜레이 스타트 웨이브");
        //foreach (var item in passthroughWalls)
        //{
        //    item.SetActive(false);
        //}
        SoundManager.instance.FadeOutBGM(bgmControlTimer);
        SoundManager.instance.DelayPlaySound("Night1_Cyborgs Attack_Loop2_BGM", SoundType.BGM, bgmControlTimer, true);
        waveCoroutine = StartCoroutine(RunWave());
    }
    //private void Update()
    //{
    //    // K 키를 누르면 모든 적을 제거
    //    if (Input.GetKeyDown(KeyCode.K))
    //    {
    //        KillAllEnemies();
    //    }

    //    // I 키를 누르면 플레이어가 죽은 상태로 설정
    //    if (Input.GetKeyDown(KeyCode.I))
    //    {
    //        isPlayerDead = true;
    //        HandlePlayerDeath();
    //    }
    //}
    private void KillAllEnemies()
    {
        foreach (var enemy in FindObjectsOfType<EnemyFSM>())
        {
            enemy.TakeDamage(int.MaxValue);
        }
    }

    private void ResetWave()
    {
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
        Debug.Log("모든 웨이브 클리어==================!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        
        EndingEvent.instance.PlayEnding();
        yield return new WaitForSeconds(timeBetweenWave);
    }

    private IEnumerator StartWave()
    {
        Debug.Log(curWave + 1 + "웨이브 시작");
        OnWaveStart?.Invoke(curWave + 1);
        
        curWaveInfo = waveInfo[curWave];
        float waveDuration = perWaveSecond[curWave];
        float waveStartTime = Time.time;
        //WaveInfoSo가져와서 웨이브 정보 세팅
        curWaveEnemyCnt += curWaveInfo.perWaveMonsterCnt;
        waveTimerCoroutine = StartCoroutine(WaveTimer(waveDuration));
        while ( Time.time - waveStartTime < waveDuration)
        {
            if (isPlayerDead)
            {
                if (waveTimerCoroutine != null)
                {
                    StopCoroutine(waveTimerCoroutine);
                }
                yield break;
            }
            
            SpawnEnemyGroup(curWaveInfo.firstGroupEnemies, 0);
            yield return new WaitForSeconds(curWaveInfo.delayBetweenGroups);
            SpawnEnemyGroup(curWaveInfo.secondGroupEnemies, curWaveInfo.firstGroupEnemies.Count);
            break;
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

    private void SpawnEnemyGroup(List<EnemySpawnInfo> enemyGroup, int startIndex)
    {
        for (int i = 0; i < enemyGroup.Count; i++)
        {
            Transform spawnPoint = perWaveSpawnPoints[curWave].transform.GetChild(i + startIndex);
            EnemySpawnInfo spawnInfo = enemyGroup[i];
            // GameObject enemyPrefab = spawnInfo.enemyType == EnemyType.NORMAL ? normalEnemy : coverEnemy;
            GameObject enemyPrefab;

            if(spawnInfo.enemyType == EnemyType.NORMAL)
            {
                enemyPrefab = normalEnemy;
            }
            else if(spawnInfo.enemyType == EnemyType.COVER)
            {
                enemyPrefab = coverEnemy;
            }
            else if(spawnInfo.enemyType==EnemyType.RANGE)
            {
                enemyPrefab = rangeEnemy;
            }
            else // SUICIDE
            {
                enemyPrefab = suicideEnemy;
            }
            
            GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);

            
            if (spawnInfo.enemyType == EnemyType.NORMAL||spawnInfo.enemyType==EnemyType.RANGE)
            {
                enemy.GetComponent<EnemyFSM>().SetUp3(target);
            }
            else if (spawnInfo.enemyType == EnemyType.SUICIDE)
            {
                enemy.GetComponent<SuicideFSM>().SetUp3(target);
            }
            else if (spawnInfo.enemyType == EnemyType.COVER)
            {
                enemy.GetComponent<CoverFSM>().SetUp3(target, cover[spawnInfo.coverIndex]);
            }
        }
    }
    // public void EnemyDie(GameObject enemy)
    // {
    //     enemyMemoryPool.DeactivatePoolItem(enemy);
    //     curWaveEnemyCnt--;
    // }
}
