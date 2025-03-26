using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialUI : MonoBehaviour
{
    public static TutorialUI Instance;

    private Dictionary<string, TutorialImage> tutorialImagePairs = new Dictionary<string, TutorialImage>();
    [SerializeField] private List<TutorialImage> images = new List<TutorialImage>();
    public List<TutorialImage> _Images => images;
    [SerializeField] private Image imageObj;
    public Image ImageObj => imageObj;
    public Transform defaultPosition;
    public bool stopCoroutine { get; set; }
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }
    private void Start()
    {
        //Init();
        //imageObj.gameObject.SetActive(false);
    }
    public void Init()
    {
        foreach (var image in images)
        {
            tutorialImagePairs.Add(image.imageName, image);
            Debug.Log(tutorialImagePairs[image.imageName] + " : image : " + image.imageName);
            image.Init(imageObj);
        }
    }

    public void ShowImage(Transform spawnPos, string name, bool loop = false)
    {
        StartCoroutine(ShowImageRoutine(spawnPos, name, loop));
    }
    public IEnumerator ShowImageRoutine(Transform spawnPos, string name, bool loop = false)
    {
        var waitSeconds = new WaitForSeconds(3f);

        imageObj.gameObject.SetActive(true);
        imageObj.transform.position = spawnPos.position;
        Debug.Log("oooopompo : " + name);
        var image = tutorialImagePairs[name];
        imageObj.sprite = image._sprite;

        if (loop)
        {
            while (!stopCoroutine)
            {
                yield return image.ShowTutorialUI(0, 1);

                yield return waitSeconds;

                yield return image.ShowTutorialUI(1, 0);

                yield return waitSeconds;
            }
        }
        else
        {
            yield return image.ShowTutorialUI(0, 1);

            yield return waitSeconds;

            yield return image.ShowTutorialUI(1, 0);
        }

        //imageObj.gameObject.SetActive(false);
        stopCoroutine = false;
    }
}
