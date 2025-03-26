using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPadControll : MonoBehaviour
{
    public int correctCombination;
    public bool accessGranted = false;

    public bool answer;
    // Start is called before the first frame update
    void Start()
    {
        answer = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(accessGranted == true)
        {
            answer = true;
            accessGranted = false;
        }
    }

    public bool CheckIfCorrect(int combination)
    {
        if(correctCombination == combination)
        {
            accessGranted = true;
            return true;
        }
        return false;
    }
}
