using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DayOneManager : MonoBehaviour
{
    public enum GameState
    {
        NONE,
        Tutorial,
        EndTutorial,
        PUZZLE1,
        PUZZLE2,
        PUZZLE3,
        PUZZLECLEAR,
        GAMECLEAR,
        GAMEOVER
    }
    [SerializeField] private Transform[] tutorialImagePos;
    [SerializeField]private GameObject puzzle2MonitorCanvas;
    [SerializeField] private GameObject monitorCanvas;
    [SerializeField]
    private GameState curState = GameState.NONE;
    public GameState CurState { get { return curState; } }

    [SerializeField] private GameState nextState = GameState.NONE;
    public GameState NextState { set { nextState = value; } }

    public ScreenChanger screenChanger;

    OVRCameraRig cam;
    private void Awake()
    {
        cam = FindObjectOfType<OVRCameraRig>();
        nextState = GameState.Tutorial;
        //nextState = GameState.PUZZLE1;
    }

    

    #region sceneLoadCheck
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Loaded scene " + scene.name);
        SoundManager.instance.PlaySound("Day1_BGM", SoundType.BGM, true);
    }
    #endregion

    void Update()
    {
        if (curState != nextState)
        {
            curState = nextState;
            switch (curState)
            {
                case GameState.Tutorial:
                    StartCoroutine(StartTutorial());
                    screenChanger.ChangeScreen(1);
                    Debug.Log("Tutorial");
                    break;
                case GameState.EndTutorial:
                    StartCoroutine(StartDialog());
                    TutorialUI.Instance.ImageObj.gameObject.SetActive(false);
                    Debug.Log("TutorialEnd");
                    break;
                case GameState.PUZZLE1:
                    Debug.Log("Puzzle1");
                    break;
                case GameState.PUZZLE2:
                    screenChanger.ChangeScreen(2);
                    DialogManager.instance.ShowDialog("B4"); // "이 소리는... 제길"
                    SoundManager.instance.PlaySound("Day1_BGM2", SoundType.BGM, true);
                    puzzle2MonitorCanvas.SetActive(true);
                    Debug.Log("Puzzle2");
                    break;
                case GameState.PUZZLE3:
                    Debug.Log("Puzzle3");
                    puzzle2MonitorCanvas.SetActive(false);
                    monitorCanvas.SetActive(true);
                    screenChanger.ChangeScreen(3);
                    break;
                case GameState.PUZZLECLEAR: // 문을 두드리는 소리부터 PUZZLECLEAR state
                    Debug.Log("Puzzle Clear");
                    screenChanger.ChangeScreen(1);
                    // TODO Glitch
                    cam.centerEyeAnchor.GetComponent<CameraRendererSwitcher>().SetSituation(1);
                    SoundManager.instance.PlaySound("BangingOnDoor", SoundType.SFX, false);
                    DialogManager.instance.ShowDialog("E5"); // "이제 비밀공간으로 가서..."
                    //머리의 헤드셋을 잡고 벗는다?
                    StartCoroutine(PuzzleClearProcess());
                    break;
                case GameState.GAMECLEAR:
                    cam.centerEyeAnchor.GetComponent<CameraRendererSwitcher>().EndSituation(1);
                    GameClearProcess();
                    break;
                case GameState.GAMEOVER:
                    GameOverProcess();
                    break;
            }
        }
    }
    void GameClearProcess()
    {
        // �� Scene Load
        SoundManager.instance.FadeOutBGM(1f);
        SceneManager.LoadScene("New_NightScene_XMC");
        SceneDataManager.instance.WaveNum = 0;
        Debug.Log("GameClear!"); // TODO 이후 MR씬으로 넘어가도록
    }

    void GameOverProcess()
    {
        // Ʃ�丮�� Scene Load
        Debug.Log("GameOver!");
        DialogManager.instance.ShowDialog("C3_b_1"); // "시간 내에 연결하진 못했지만... 괜찮아."
        nextState = GameState.PUZZLE3;
    }

    static DayOneManager _Instance;
    public static DayOneManager Instance
    {
        get
        {
            if (_Instance == null)
            {
                //GameObject dayOneManager = GameObject.Find("@DayOneManager");
                //if(dayOneManager = null) {
                //    dayOneManager = new GameObject { name = "DayOneManager" };
                //    dayOneManager.AddComponent<DayOneManager>();
                //}

                //DontDestroyOnLoad(dayOneManager);
                //_Instance = dayOneManager.GetComponent<DayOneManager>();
                _Instance = FindAnyObjectByType<DayOneManager>();
                 
            }
            return _Instance;
        }
    }

    private IEnumerator StartDialog()
    {
        yield return new WaitForSeconds(5f);
        DialogManager.instance.ShowDialog("A_4A"); // "일단 책상 위에 아저씨가 놓고 간 총이 있을 거야." TODO: "미안한데, 자세한 건 설명할 시간이 없어 ~"
        nextState = GameState.PUZZLE1;
    }

    private IEnumerator PuzzleClearProcess()
    {
        yield return new WaitForSeconds(5f); // TODO 머리의 헤드셋을 잡고 벗는다?
        nextState = GameState.GAMECLEAR;
    }
    #region Tutorial
    private IEnumerator StartTutorial()
    {
        var wristWatch = new WristWatchUI();
        var tutorialUI = new TutorialUI();
        if (WristWatchUI.Instance is not null) { wristWatch = WristWatchUI.Instance; }
        if (TutorialUI.Instance is not null) tutorialUI = TutorialUI.Instance;
        DialogManager.instance.ShowDialog("A1_1"); // "아아, 잘 들려?"
        tutorialUI.Init();
        tutorialUI.ShowImage(tutorialImagePos[1], "Pointer", true);
        while (!wristWatch.gameObject.activeInHierarchy) yield return null;
        DialogManager.instance.ShowDialog("A1_2"); // "손목시계에 손을 가까이 대면 손목 시계가 커질 거야."
        tutorialUI.ShowImage(tutorialImagePos[1], "Grip", true);

        yield return new WaitForSeconds(1f);

        DialogManager.instance.ShowDialog("A1_3"); // "내 도움이 필요할 땐 ~"
        var watchInventory = wristWatch.inventoryUI;

        while (!watchInventory.haveItem) yield return null;
        tutorialUI.ShowImage(tutorialImagePos[1], "Pointer");
        // DialogManager.instance.ShowDialog("A1_4"); // "일단 책상 위에 아저씨가 놓고 간 총이 있을 거야. ~"
        nextState = GameState.EndTutorial;
    }
    #endregion
}
