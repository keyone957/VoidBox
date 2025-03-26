using UnityEngine;

//단순 바닥 확인용
public class Floor : MonoBehaviour
{
    private void Start()
    {
        transform.localScale = transform.localScale * 1.5f;
    }
}
