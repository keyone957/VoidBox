using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum TutorialPhase
{
    Intro,
    DoorSpawn,
    BoxOpen,
    WristBand,
    VRHeadSet,
    End,
}
public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;

    public TutorialPhase tutorialPhase;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }
    void Start()
    {
        tutorialPhase = TutorialPhase.Intro;
    }

    public void PhaseUpdate()
    {
        switch (tutorialPhase)
        {
            case TutorialPhase.Intro:
                tutorialPhase = TutorialPhase.DoorSpawn;
                break;
            case TutorialPhase.DoorSpawn:
                tutorialPhase = TutorialPhase.BoxOpen;
                break;
            case TutorialPhase.BoxOpen:
                tutorialPhase = TutorialPhase.WristBand;
                break;
            case TutorialPhase.WristBand:
                tutorialPhase = TutorialPhase.VRHeadSet;
                break;
            case TutorialPhase.VRHeadSet:
                tutorialPhase = TutorialPhase.End;
                break;
            case TutorialPhase.End:
                break;
        }
        Debug.Log(tutorialPhase);
    }
}
