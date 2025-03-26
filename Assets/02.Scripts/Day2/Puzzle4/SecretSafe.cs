using System.Collections;
using UnityEngine;
public class SecretSafe : MonoBehaviour
{
    [SerializeField] private int safeIdx;
    public int _safeIdx;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public IEnumerator OpenSafe()
    {
        animator.SetTrigger("DoorOpen");
        yield return null;
    }
}
