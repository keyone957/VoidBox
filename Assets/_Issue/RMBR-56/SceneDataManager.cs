using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneDataManager : MonoBehaviour
{
    public int WaveNum;
    public static SceneDataManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
}