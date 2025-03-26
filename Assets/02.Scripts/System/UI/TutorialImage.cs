using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;

[CreateAssetMenu(fileName = "TutorialUI_Watch", menuName = "Scriptable/TutorialUI_Watch", order = int.MaxValue)]
public class TutorialImage : ScriptableObject
{
    private Image image;
    [SerializeField] private string _name;
    public string imageName => _name;
    [SerializeField] private Sprite sprite;
    public Sprite _sprite => sprite;

    public bool haveNext;
    public Color startColor;
    public void Init(Image baseImage)
    {
        this.image = baseImage;
        this.image.color = startColor;
    }

    public IEnumerator ShowTutorialUI(float alphaIn, float alphaOut, float fadeDuration = 2f)
    {
        float timer = 0;
        float a = image.color.a;

        while (timer <= fadeDuration)
        {
            Color newColor = image.color;
            newColor.a = Mathf.Lerp(alphaIn, alphaOut, timer / fadeDuration);
            image.color = newColor;

            timer += Time.deltaTime;
            yield return null;
        }

        Color newColor2 = image.color;
        newColor2.a = alphaOut;
        image.color = newColor2;
    }
}
