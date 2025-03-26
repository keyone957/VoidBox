using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    public OVRPassthroughLayer oVRPassthroughLayer;
    public enum PuzzleState
    {
        NONE,
        COFFEE,
        PHONECALL,
        BURNDOC,
        OPENDOOR
    }
    public PuzzleState nowState = PuzzleState.NONE;
    public PuzzleState nextState = PuzzleState.NONE;

    //public PhoneCall phoneCall;
    public GameObject photo;
    public Animator doorAnimator;
    // Start is called before the first frame update
    void Start()
    {
        nextState = PuzzleState.COFFEE;
        oVRPassthroughLayer.textureOpacity = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (nowState != nextState)
        {
            nowState = nextState;
            switch (nowState)
            {
                case PuzzleState.COFFEE:
                    break;
                case PuzzleState.PHONECALL:
                    //phoneCall.PhoneRinging();
                    break;
                case PuzzleState.BURNDOC:
                    break;
                case PuzzleState.OPENDOOR:
                    doorAnimator.SetTrigger("doorOpen");
                    photo.SetActive(true);
                    break;
            }
        }
    }

    public void WrongAnswer()
    {
        oVRPassthroughLayer.textureOpacity += 0.25f;
        if (oVRPassthroughLayer.textureOpacity >= 0.99f)
        {
            oVRPassthroughLayer.textureOpacity = 1;
            GameOver();
        }
    }
    void GameOver()
    {

    }

    private static PuzzleManager _Instance;
    public static PuzzleManager Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = FindAnyObjectByType<PuzzleManager>();
            }
            return _Instance;
        }
    }

}


