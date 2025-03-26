using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenChanger : MonoBehaviour
{
    [SerializeField] DayOneManager manager;
    public MeshRenderer[] screenMat;
    [SerializeField] public Material[] puzzle2Mat;
    [SerializeField] public Material offMat;
    [SerializeField] public Material rebootMat;
    [SerializeField] public Material[] puzzle3Mat;
    // Start is called before the first frame update
    void Start()
    {
        manager = FindObjectOfType<DayOneManager>();
    }

    public void ChangeScreen(int puzzle)
    {
        if (puzzle == 2)
        {
            for (int i = 0; i < screenMat.Length; i++)
            {
                screenMat[i].material = puzzle2Mat[i];
            }

        }
        else if (puzzle == 3)
        {
            for (int i = 0; i < screenMat.Length; i++)
            {
                screenMat[i].material = offMat;
            }
            screenMat[5].material = rebootMat;
            Debug.Log("systemChange");
            StartCoroutine(SystemReBoot());
        }
        else
        {
            for (int i = 0; i < screenMat.Length; i++)
                screenMat[i].material = offMat;
        }
    }
    IEnumerator SystemReBoot()
    {

        yield return new WaitForSeconds(2.0f);
        for (int i = 0; i < screenMat.Length; i++)
        {
            screenMat[i].material = puzzle3Mat[i];
        }
    }
}
