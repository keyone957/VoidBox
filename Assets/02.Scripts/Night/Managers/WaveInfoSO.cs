using System;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    NORMAL,
    COVER,
    SUICIDE,
    RANGE
}

[Serializable]
public class EnemySpawnInfo
{
    public EnemyType enemyType;
    public int coverIndex;
}

[CreateAssetMenu(fileName = "WaveInfo", menuName = "SO/WaveInfoSO")]
public class WaveInfoSO : ScriptableObject
{
    [Header("총 몬스터 수")]
    public int perWaveMonsterCnt; // 웨이브당 소환해야 할 몬스터 수

    [Header("첫번째 그룹 몬스터 소환")]
    public List<EnemySpawnInfo> firstGroupEnemies;  
    [Header("두번째 그룹 몬스터 소환")]
    public List<EnemySpawnInfo> secondGroupEnemies;
    [Header("그룹간 딜레이 시간")]
    public float delayBetweenGroups;
    
    private void OnValidate()
    {
        ValidateEnemyCounts();
    }
    private void ValidateEnemyCounts()
    {
        int firstGroupCount = firstGroupEnemies.Count;
        int secondGroupCount = secondGroupEnemies.Count;
        int totalSpawnedEnemies = firstGroupCount + secondGroupCount;
        if (totalSpawnedEnemies != perWaveMonsterCnt)
        {
            Debug.LogError("각 그룹에 알맞은 몬스터수를 세팅해주세요");
        }
    }
}
