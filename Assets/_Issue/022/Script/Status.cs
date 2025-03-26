using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HPEvent : UnityEngine.Events.UnityEvent<int, int> { }
public class Status : MonoBehaviour
{
    [HideInInspector]
    public HPEvent onHPEvent = new HPEvent();

    [Header("Walk, Run Speed")]
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float runSpeed;

    [Header("HP")]
    [SerializeField]
    private int maxHP = 100;
    private int currentHP;

    public float WalkSpeed => walkSpeed;
    public float RunSpeed => runSpeed;

    public int MaxHP => maxHP;
    public int CurrentHP => currentHP;

    private void Awake()
    {
        currentHP = maxHP;
    }

    public bool DecreaseHP(int damage)
    {
        int previousHP = currentHP;
        currentHP = (currentHP - damage) > 0 ? (currentHP - damage) : 0;
        onHPEvent.Invoke(previousHP, currentHP);

        if(currentHP == 0)
        {
            SoundManager.instance.PlaySound("Night1_EnemyDied_1", SoundType.SFX);
            return true;
        }
        return false;
    }
}
