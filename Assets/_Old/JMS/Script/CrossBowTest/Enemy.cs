using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float hp = 100; // TODO: need to be replaced to SO's Speed
    public float speed = 1.0f; // TODO: need to be replaced to SO's Speed

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Damaged(Arrow arrow)
    {
        hp -= arrow.arrowData.Dmg;
        switch(arrow.arrowData.Att)
        {
            case 0:     //Normal
                break;
            case 1:     //Fire
                Dot(arrow.arrowData.Dot, arrow.arrowData.Level);
                break;
            case 2:     //Ice
                Slow(arrow.arrowData.Level);
                break;
        }

    }

    private async void Dot(float dmg, int level)
    {
        await DotUniTask(dmg, level);
    }

    private async void Slow(int level)
    {
        await SlowUniTask(level);
        
    }

    private async UniTask SlowUniTask(int level)
    {
        speed *= (0.3f * (float)level);
        await UniTask.Delay(3000);  // Zero Allocation
        Debug.Log("UniTask running every 1 second");
    }

    private async UniTask DotUniTask(float dmg, int level)
    {
        for (int i = 0; i < 3; i++) { 
            hp -= dmg + 0.5f * (float)level;
            await UniTask.Delay(3000);  // Zero Allocation
        }
    }
}
