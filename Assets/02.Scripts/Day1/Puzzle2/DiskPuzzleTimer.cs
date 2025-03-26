using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiskPuzzleTimer : MonoBehaviour
{
    public Slider timerSlider;
    public Text timerText;
    public float gameTime;

    public bool stopTimer;
    public bool isGameOver;

    void Awake()
    {
        timerSlider.maxValue = gameTime;
        timerSlider.value = gameTime;

        isGameOver = false;
    }

    void Update()
    {
        if (DayOneManager.Instance.CurState == DayOneManager.GameState.PUZZLE2)
        {
            TimerOn();
        }
    }
    private void TimerOn()
    {

        float time = gameTime - Time.deltaTime;

        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time - minutes * 60f);

        string textTime = string.Format("{0:0}:{1:00}", minutes, seconds);

        if (time <= 0)
        {
            stopTimer = true;
            GameOver();
        }

        if (stopTimer == false)
        {
            timerText.text = textTime;
            timerSlider.value = time;
            gameTime = time;
        }
    }
    public void ClearSoundPlay()
    {
        StartCoroutine(SoundCoroutine());
    }
    private IEnumerator SoundCoroutine()
    {
        float ReleaseWarningDuration = 5f;
        yield return new WaitForSeconds(1f);

        SoundManager.instance.StopSoundByClip("Siren");
        SoundManager.instance.PlaySound("ReleaseWarning", SoundType.SFX);

        yield return new WaitForSeconds(ReleaseWarningDuration);
        SoundManager.instance.StopSoundByClip("ReleaseWarning");
        yield return null;
        SoundManager.instance.PlaySound("Day1_BGM", SoundType.BGM, true);
    }
    private void GameOver()
    {
        DayOneManager.Instance.NextState = DayOneManager.GameState.GAMEOVER;
    }

}
