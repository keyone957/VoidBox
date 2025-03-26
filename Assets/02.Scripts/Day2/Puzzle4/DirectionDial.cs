using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Direction
{
    Right,
    Left,
    Down,
    Up
}
public class DirectionDial : MonoBehaviour
{
    [SerializeField] private GameObject dial;
    [SerializeField] private List<string> password = new List<string>();
    [SerializeField] private List<string> answer = new List<string>();
    private SmoothMoveObject smooth;
    private Rigidbody rb;

    /* audioClip 필요
    private AudioSource audio;
    private AudioClip failClip;
    private AudioClip successClip;
    */

    private SecretSafe secretSafe;
    [SerializeField] private int safeIndex;
    private bool fail = false;
    private float DialTimer = 25.0f;
    void Start()
    {
        Init();
        StartCoroutine(DialMode(DialTimer));
        //audio = GetComponent<AudioSource>();
    }
    private void Init()
    {
        smooth = GetComponent<SmoothMoveObject>();
        rb = GetComponent<Rigidbody>();
        answer = DirectionDialManager.instance.answer;
        secretSafe = DirectionDialManager.instance.secretSafe;
        safeIndex = DirectionDialManager.instance.safeAnswerIdx;
        //SafeDialCheck();
    }
    void SafeDialCheck()
    {
        if (safeIndex != DirectionDialManager.instance.safeAnswerIdx)
        {
            //audio.PlayOneShot(failClip);
            rb.isKinematic = true;
        }
        else rb.isKinematic = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (safeIndex != DirectionDialManager.instance.safeAnswerIdx)
        { 
            //audio.PlayOneShot(failClip);
            StartCoroutine(smooth.ObjectMove());
            return;
        }
        StartCoroutine(smooth.ObjectMove());
        string _password = other.name;
        password.Add(_password);
        StartCoroutine(EnabledTrigger());
        if (password.Count >= 4)
        {
            StartCoroutine(DialAnswerCheck());
        }
    }
    IEnumerator EnabledTrigger()
    {
        this.GetComponent<SphereCollider>().enabled = false;
        yield return new WaitForSeconds(1);
        this.GetComponent<SphereCollider>().enabled = true;
    }
    IEnumerator DialAnswerCheck()
    {
        for (int i = 0; i < password.Count; i++)
        {
            Debug.Log(password[i]);
            if (password[i] != DirectionDialManager.instance.answer[i].ToString())
            {
                //audio.PlayOneShot(failClip);
                fail = true;
            }
            else
            {
                //audio.PlayOneShot(successClip);
            }
        }
        yield return null;
        if (fail)
        {
            password.Clear();
            //audio.PlayOneShot(failClip);
            Debug.Log("실패");
            fail = false;
        }
        else StartCoroutine(secretSafe.OpenSafe());

        yield return null;
    }
    public IEnumerator DialMode(float timer)
    {
        rb.isKinematic = false;

        float dialTime = timer;

        while (dialTime >= 0)
        {
            dialTime -= Time.deltaTime;

            float x = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick, OVRInput.Controller.RTouch).x;
            float y = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick, OVRInput.Controller.RTouch).y;

            float xMove = Input.GetAxis("Horizontal");
            float yMove = Input.GetAxis("Vertical");
            rb.velocity = new Vector3(xMove, yMove, 0);
            transform.rotation = Quaternion.Euler(0, 0, 0);
            yield return null;
        }
    }
}
