using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
     private static EnemyManager instance = null;

    //[HideInInspector] public GameManager player;

    public static EnemyManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<EnemyManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject("EnemyManager");
                    instance = obj.AddComponent<EnemyManager>();
                }
            }
            return instance;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Slow()
    {
        
    }
}
