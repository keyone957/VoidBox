# Void Box

진행 기간: 2024. 07 ~ 2025. 02

사용한 기술 스택: C#, MR, Meta Quest 2, Meta Quest 3, Unity, VR

한 줄 설명: VR / MR 환경을 이용한 퍼즐 / 슈팅게임

개발 인원(역할): 개발자 6명 + 기획자 3명 + 디자이너 3명

비고: 팀 프로젝트 /뉴콘텐츠 아카데미 단기과정 2기


## 프로젝트 소개
---

[![image.png](image.png)](https://github.com/user-attachments/assets/dbda22af-ba1c-47c4-ad78-5069ed7938f4)

[![image.png](image%201.png)](https://github.com/user-attachments/assets/4356d2b6-959e-4e79-9ea0-efbae171bfbc)

## 나의 역할

---

- 낮 기믹
    - 컨트롤러 오브젝트 인터렉션을 이용한 퍼즐 해결 로직 구현(핸드 트래킹 및 컨트롤러 사용)
    - Snap 인터렉터를 이용한 퍼즐 기믹 구현
- 밤 기믹
    - 밤 전투씬 시스템 구현
    - FSM을 사용한 몬스터 상태 및 피격 공격 구현
    - 오브젝트 풀링을 사용한 몬스터 소환
        - 오브젝트 풀링 코드
<details>

<summary>MemoryPool.cs</summary>

```csharp

            using System.Collections;
            using System.Collections.Generic;
            using UnityEngine;
            
            public class MemoryPool : MonoBehaviour
            {
                // Start is called before the first frame update
                private class PoolItem
                {
                    public bool isActive;
                    public GameObject gameObject;
                }
            
                private int increaseCount = 5;
                private int maxCount;
                private int activeCount;
            
                private GameObject poolObject;
                private List<PoolItem> poolItemList;
            
                public MemoryPool(GameObject poolObject)
                {
                    maxCount = 0;
                    activeCount = 0;
                    this.poolObject = poolObject;
            
                    poolItemList = new List<PoolItem>();
            
                    InstantiateObjects();
                }
            
                public void InstantiateObjects()
                {
                    maxCount += increaseCount;
            
                    for (int i = 0; i < increaseCount; ++ i)
                    {
                        PoolItem poolItem = new PoolItem();
            
                        poolItem.isActive = false;
                        poolItem.gameObject = GameObject.Instantiate(poolObject);
                        poolItem.gameObject.SetActive(false);
            
                        poolItemList.Add(poolItem);
                    }
                }
            
                public void DestroyObjects()
                {
                    if (poolItemList == null) return;
            
                    int count = poolItemList.Count;
                    for(int i = 0; i < count; ++ i)
                    {
                        GameObject.Destroy(poolItemList[i].gameObject);
                    }
            
                    poolItemList.Clear();
                }
            
                public GameObject ActivatePoolItem()
                {
                    if (poolItemList == null) return null;
            
                    if (maxCount == activeCount)
                    {
                        InstantiateObjects();
                    }
            
                    int count = poolItemList.Count;
                    for (int i = 0; i < count; ++i)
                    {
                        PoolItem poolItem = poolItemList[i];
            
                        if(poolItem.isActive == false)
                        {
                            activeCount ++;
            
                            poolItem.isActive = true;
                            poolItem.gameObject.SetActive(true);
            
                            return poolItem.gameObject;
                        }
                    }
                    return null;
                }
            
                public void DeactivatePoolItem(GameObject removeObject)
                {
                    if (poolItemList == null || removeObject == null) return;
            
                    int count = poolItemList.Count;
                    for(int i = 0; i < count; ++i)
                    {
                        PoolItem poolItem = poolItemList[i];
            
                        if(poolItem.gameObject ==  removeObject)
                        {
                            activeCount --;
            
                            poolItem.isActive = false;
                            poolItem.gameObject.SetActive(false);
            
                            return;
                        }
                    }
                }
            
                public void DeactivateAllPoolItems()
                {
                    if (poolItemList == null) return;
            
                    int count = poolItemList.Count;
                    for (int i = 0; i < count; ++i)
                    {
                        PoolItem poolItem = poolItemList[i];
            
                        if(poolItem.gameObject != null && poolItem.isActive == true)
                        {
                            poolItem.isActive = false;
                            poolItem.gameObject.SetActive(false);
                        }
                    }
                    activeCount = 0;
                }
            }


```
</details>
            
      
           

            

## 개발 내용 및 플레이 영상

---

### [인트로 씬]

[https://youtu.be/GOowSH8tOuY](https://youtu.be/GOowSH8tOuY)

### [낮 기믹]

![베어핸즈.mp4_20250121_171741.gif](%25EB%25B2%25A0%25EC%2596%25B4%25ED%2595%25B8%25EC%25A6%2588.mp4_20250121_171741.gif)

낮 퍼즐 1 → 금고 돌리기

맵에 숨겨져 있는 힌트를 찾아 금고의 비밀번호를 알아내 금고를 열어라!

![베어핸즈.mp4_20250121_171907.gif](%25EB%25B2%25A0%25EC%2596%25B4%25ED%2595%25B8%25EC%25A6%2588.mp4_20250121_171907.gif)

낮 퍼즐 2 → 보안 시스템 연결

조이스틱과 버튼을 이용해서 컴퓨터에 있는 보안 시스템을 작동시켜 연결하자

![베어핸즈.mp4_20250121_172227.gif](%25EB%25B2%25A0%25EC%2596%25B4%25ED%2595%25B8%25EC%25A6%2588.mp4_20250121_172227.gif)

낮 퍼즐 3→ 컴퓨터 화면에 있는 단서를 활용하여  상질물을 나열하여 기믹을 풀어보자

[https://www.youtube.com/watch?v=uDfg89vevY4](https://www.youtube.com/watch?v=uDfg89vevY4)

### [밤 MR+VR 환경 전투 씬]

[https://youtu.be/jV8vliRP2kM](https://youtu.be/jV8vliRP2kM)

=⇒ 앞에서 다가오는 적들을 물리쳐서 게임을 클리어하라!

## 프로젝트 사용기술

---

### ⚒️ 클라이언트

- Unity
- C#
- XR Interaction Toolkit
- Oculus Interaction SDK

### ⚒️ 버전 관리 및 협업

- Git
- Notion
- Figma
- Jira

### ⚒️ 개발 환경

- Visual Studio
