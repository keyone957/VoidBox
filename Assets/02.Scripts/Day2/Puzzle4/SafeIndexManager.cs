using UnityEngine;

public class SafeIndexManager : MonoBehaviour
{
    [SerializeField] private Safes safes;
    void Awake()
    {
        SetSafeNumber();
    }
    void SetSafeNumber()
    {
        int firstNum = 101;
        for (int i = 0; i < safes.safeObjs.Length; i++)
        {
            if (firstNum % 10 != 0)
            {
                safes.safeObjs[i].GetComponent<SecretSafe>()._safeIdx = firstNum;
                firstNum++;
            }
            else
            {
                firstNum += 1;
                safes.safeObjs[i].GetComponent<SecretSafe>()._safeIdx = firstNum;
                firstNum++;
            }
        }
    }

}
