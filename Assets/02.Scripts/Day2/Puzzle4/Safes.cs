using UnityEngine;

public class Safes : MonoBehaviour
{
    public int password;
    public GameObject[] safeObjs;
    public GameObject safe;
    public bool filterMode;
    
    private void Start()
    {
        SetSafeIdx();
    }
    private void SetSafeIdx()
    {
        safe = GetSecretSafe();
        DirectionDialManager.instance.safeAnswerIdx = GetSafeIndex(safe);
        DirectionDialManager.instance.secretSafe = safe.GetComponent<SecretSafe>();
    }
    //·£´ýÇÑ safe
    public GameObject GetSecretSafe()
    {
        int safeNum = Random.Range(0, safeObjs.Length);
        return safeObjs[safeNum];
    }
    //safeÀÇ Index
    public int GetSafeIndex(GameObject go)
    {
        return go.GetComponent<SecretSafe>()._safeIdx;
    }
}
