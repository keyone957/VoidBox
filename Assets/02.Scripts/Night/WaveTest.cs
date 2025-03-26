using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveTest : MonoBehaviour
{
    private bool waveCleared = false;
    private bool isInBossStage = false; // 보스 스테이지 여부를 확인하기 위한 플래그

    private void Start()
    {
        NightWaveManager.instance.OnWaveStart += HandleWaveStart;
        NightWaveManager.instance.OnWaveEnd += HandleWaveEnd;
        NightWaveManager.instance.OnGameClear += HandleGameClear;
        NightWaveManager.instance.OnGameOver += HandleGameOver;
        NightWaveManager.instance.OnBossSpawn += HandleBossSpawn;
    }

    private void Update()
    {
        // 테스트 코드: k를 누르면 몬스터 처치
        if (Input.GetKeyDown(KeyCode.K))
        {
            // 현재 웨이브의 몬스터 수가 0보다 클 경우에만 처리
            if (NightWaveManager.instance.waveMonsterCnt > 0)
            {
                // 몬스터를 하나 죽임
                NightWaveManager.instance.waveMonsterCnt--;
                Debug.Log("남은 몬스터 수: " + NightWaveManager.instance.waveMonsterCnt);

                // 몬스터가 모두 죽었는지 확인
                if (NightWaveManager.instance.waveMonsterCnt <= 0)
                {
                    Debug.Log("모든 몬스터가 죽었습니다!");
                    waveCleared = true;
                }
            }
            else
            {
                Debug.Log("현재 웨이브에 몬스터가 없습니다!");
            }

            // 몬스터가 모두 죽었을 경우 다음 웨이브로 이동
            if (waveCleared)
            {
                NightWaveManager.instance.ClearWave();
                waveCleared = false;
            }
        }

        // i 버튼을 누르면 보스 스테이지 클리어
        if (Input.GetKeyDown(KeyCode.I) && isInBossStage)
        {
            NightWaveManager.instance.BossStageClear();
            isInBossStage = false; // 보스 스테이지 상태를 초기화
        }
    }

    private void HandleWaveStart(int waveNumber)
    {
        Debug.Log("웨이브 " + waveNumber + " 시작! 몬스터 수: " + NightWaveManager.instance.waveMonsterCnt);
        waveCleared = false;
        isInBossStage = false;
    }

    private void HandleWaveEnd(int waveNumber)
    {
        Debug.Log($"웨이브 {waveNumber} 클리어!");
    }

    private void HandleGameClear()
    {
        Debug.Log("게임 클리어!");
    }

    private void HandleGameOver()
    {
        Debug.Log("게임 오버!");
    }

    private void HandleBossSpawn()
    {
        Debug.Log("보스 몬스터 소환!");
        isInBossStage = true;
    }

    private void OnDestroy()
    {
        NightWaveManager.instance.OnWaveStart -= HandleWaveStart;
        NightWaveManager.instance.OnWaveEnd -= HandleWaveEnd;
        NightWaveManager.instance.OnGameClear -= HandleGameClear;
        NightWaveManager.instance.OnGameOver -= HandleGameOver;
        NightWaveManager.instance.OnBossSpawn -= HandleBossSpawn;
    }

}
