using Cysharp.Threading.Tasks.Triggers;
using JetBrains.Annotations;
using Oculus.Interaction.DebugTree;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.UI;

public class DirectionDialManager : MonoBehaviour
{
    [SerializeField] private Safes safes;
    [SerializeField] private TextMeshProUGUI[] directionHint;
    private Dictionary<int, string> hintString = new Dictionary<int, string>();
    private List<string> firstString = new List<string>()
    {
        "B","C","D","E","F","G","H","I"
    };

    private int x = 1, y = 1; //���� ��ġ
    public List<string> answer = new List<string>();
    public int safeAnswerIdx;
    public SecretSafe secretSafe;

    public static DirectionDialManager instance;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        InitText();
        GetAnswer();
        SetDirectionHint();
        GetStartIndex();
    }
    private void SetDirectionHint()
    {
        for (int i = 0; i < firstString.Count; i++)
        {
            hintString.Add(i, firstString[i]);
        }
    }
    private void InitText()
    {
        foreach (var text in directionHint)
        {
            text.text = null;
        }
    }
    private void GetStartIndex()
    {
        FillDirectionHint_Test();
        //DirectionHint();
        //StartCoroutine(dirCo());
    }
    /// <summary>
    /// �ӽ� ���� ��Ʈ ����
    /// ���� ������ �ö�� �� �������δ� ��Ʈ�� ������ �� ������ ���� ���ܼ� �켱 �̷��� �۾��߽��ϴ�.
    /// A�� ���������� 3 x 3 �׸����� �߾ӿ� ��ġ, �Է��ؾ� �� ���� ������� ��濡 B,C,D,E ������ ��ġ
    /// ������ ĭ�� �������� ��ġ
    /// </summary>
    private void FillDirectionHint_Test()
    {
        directionHint[4].text = "A";
        for (int i = 0;i < answer.Count;i++)
        {
            switch (answer[i])
            {
                case "Left":
                    directionHint[3].text = firstString[0];
                    firstString.Remove(firstString[0]);
                    break;
                case "Right":
                    directionHint[5].text = firstString[0];
                    firstString.Remove(firstString[0]);
                    break;
                case "Up":
                    directionHint[1].text = firstString[0];
                    firstString.Remove(firstString[0]);
                    break;
                case "Down":
                    directionHint[7].text = firstString[0];
                    firstString.Remove(firstString[0]);
                    break;
            }
        }
        foreach (var t in directionHint)
        {
            if (t.text == null)
            {
                string a = firstString[Random.Range(0, firstString.Count)];
                t.text = a;
                firstString.Remove(a);
            }
        }
    }
    private void DirectionHint()
    {
        int n = 0, m = 0;
        int startPos = 0;
        for (int i = 0; i < answer.Count; i++)
        {
            if (answer[i] == "Down") n = i;
            if (answer[i] == "Right") m = i;
        }
        for (int i = 0; i < answer.Count; i++)
        {
            switch (answer[i])
            {
                case "Left":
                    if (i < m) startPos += 1;
                    else if (i >= (m - 2)) startPos += 2;
                    break;
                case "Up":
                    if (i < n) startPos += 3;
                    else if (i >= (n - 2)) startPos += 6;
                    break;
            }
        }
        directionHint[startPos].text = "A";
        int idx = startPos;
        for (int i = 0; i < answer.Count; i++)
        {
            switch (answer[i])
            {
                case "Left":
                    idx -= 1;
                    if (directionHint[idx].text != null)
                    {
                        directionHint[idx - 1].text = firstString[i];
                        idx -= 1;
                        Debug.Log(idx);
                    }
                    else directionHint[idx].text = firstString[i];
                    break;
                case "Right":
                    idx += 1;
                    if (directionHint[idx].text != null)
                    {
                        directionHint[idx + 1].text = firstString[i];
                        idx += 1;
                        Debug.Log(idx);
                    }
                    else directionHint[idx].text = firstString[i];
                    break;
                case "Up":
                    idx -= 3;
                    if (directionHint[idx].text != null)
                    {
                        directionHint[idx - 3].text = firstString[i];
                        idx -= 3;
                        Debug.Log(idx);

                    }
                    else directionHint[idx + 3].text = firstString[i];
                    break;
                case "Down":
                    idx += 3;
                    if (directionHint[idx].text != null)
                    {
                        directionHint[idx + 3].text = firstString[i];
                        idx += 3;
                        Debug.Log(idx);

                    }
                    else directionHint[idx].text = firstString[i];
                    break;
            }
        }
    }
    IEnumerator dirCo()
    {
        int n = 0, m = 0;
        int startPos = 0;
        for (int i = 0; i < answer.Count; i++)
        {
            if (answer[i] == "Down") n = i;
            if (answer[i] == "Right") m = i;
        }
        for (int i = 0; i < answer.Count; i++)
        {
            switch (answer[i])
            {
                case "Left":
                    if (i < m) startPos += 1;
                    break;
                case "Up":
                    if (i < n) startPos += 3;
                    break;
            }
        }
        yield return new WaitForSeconds(2);
        directionHint[startPos].text = "A";
        int idx = startPos;
        for (int i = 0; i < answer.Count; i++)
        {
            switch (answer[i])
            {
                case "Left":
                    idx -= 1;
                    if (directionHint[idx].text != null)
                    {
                        directionHint[idx - 1].text = firstString[i];
                        idx -= 1;
                        Debug.Log(idx);
                    }
                    else directionHint[idx].text = firstString[i];
                    break;
                case "Right":
                    idx += 1;
                    if (directionHint[idx].text != null)
                    {
                        directionHint[idx + 1].text = firstString[i];
                        idx += 1;
                        Debug.Log(idx);
                    }
                    else directionHint[idx].text = firstString[i];
                    break;
                case "Up":
                    idx -= 3;
                    if (directionHint[idx].text != null)
                    {
                        directionHint[idx - 3].text = firstString[i];
                        idx -= 3;
                        Debug.Log(idx);

                    }
                    else directionHint[idx + 3].text = firstString[i];
                    break;
                case "Down":
                    idx += 3;
                    if (directionHint[idx].text != null)
                    {
                        directionHint[idx + 3].text = firstString[i];
                        idx += 3;
                        Debug.Log(idx);

                    }
                    else directionHint[idx].text = firstString[i];
                    break;
            }
            yield return new WaitForSeconds(2);
        }
    }
    //���� �ߺ� X
    private void GetAnswer()
    {
        var enumValues = System.Enum.GetValues(enumType: typeof(Direction));
        List<string> directions = new List<string>();
        for (int i = 0; i < enumValues.Length; i++)
        {
            directions.Add(enumValues.GetValue(i).ToString());
        }
        for (int i = 0; i < 4; i++)
        {
            string a = directions[Random.Range(0, directions.Count)];
            answer.Add(a);
            directions.Remove(a);
            Debug.Log(answer[i]);
        }
    }
    //���� �ߺ� O
    public Direction GetRandomEnumValue()
    {
        var enumValues = System.Enum.GetValues(enumType: typeof(Direction));
        
        return (Direction)enumValues.GetValue(Random.Range(0, enumValues.Length));
    }
}