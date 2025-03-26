using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractionObject : MonoBehaviour
{
    [Header("Interaction Object")]
    [SerializeField]
    protected int maxHP = 100;
    protected int curHP;
    public void Awake()
    {
        curHP = maxHP;
    }

    public abstract void TakeDamage(int damage);
}
