using UnityEngine;
public class NightWaveManager : MonoBehaviour
{
    public static NightWaveManager instance { get; private set; }
    [SerializeField] private int maxWave = 4;
    [SerializeField] private int currentWave;
    [SerializeField] private bool isClearStage;
    [SerializeField] public int waveMonsterCnt;
    private bool isBossSpawned;

    public delegate void GameStateChangeHandler();
    public delegate void WaveChangeHandler(int waveNumber);

    public event GameStateChangeHandler OnGameOver;
    public event GameStateChangeHandler OnGameClear;
    public event WaveChangeHandler OnWaveStart;
    public event WaveChangeHandler OnWaveEnd;
    public event GameStateChangeHandler OnBossSpawn;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        //초기화
        currentWave = 0;
        isBossSpawned = false;
        isClearStage = false;
        waveMonsterCnt = 0;
        StartNextWave();
    }

    private void StartNextWave()
    {
        if (currentWave < maxWave)
        {
            currentWave++;
            waveMonsterCnt = CalculateWaveMonsterCount(currentWave);
            OnWaveStart?.Invoke(currentWave);
            SpawnWaveMonsters();
        }
        else if (!isBossSpawned)
        {
            SpawnBossMonster();
        }
    }

    public void ClearWave()
    {
        OnWaveEnd?.Invoke(currentWave);
        WeaponCollection.instance.GainNewWeapon();
        Debug.Log( "클리어! 남은 몬스터 수:" + waveMonsterCnt);
        if (currentWave < maxWave)
        {
            StartNextWave();
        }
        else if (!isBossSpawned)
        {
            SpawnBossMonster();
        }
    }

    private int CalculateWaveMonsterCount(int wave)
    {
        return wave == 1 ? 2 : 2 + (wave - 1) * 2;
    }

    private void SpawnWaveMonsters()
    {
        // 몬스터 소환 로직
        Debug.Log("몬스터 "+waveMonsterCnt+"마리 소환");
    }

    public void ClearNightStage()
    {
        //밤 기믹 완전 클리어
        isClearStage = true;
        Debug.Log("게임 승리!");
        OnGameClear?.Invoke();
    }

    public void SpawnBossMonster()
    {
        //보스 몬스터 소환
        isBossSpawned = true;
        OnBossSpawn?.Invoke();
    }

    public void BossStageClear()
    {
        //보스 몬스터 처치완료
        Debug.Log("보스를 처치했습니다!");
        ClearNightStage();
    }

    public void GameOver()
    {
        //게임 오버
        //TODO: 플레이어 게임오버 로직에 넣기.
        Debug.Log("게임 오버!");
        OnGameOver?.Invoke();
    }
}