using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[System.Serializable]
public struct PlayerStatus
{
    public int maxHP;
    public int hp;
    public bool isInvincible;
    public bool isDied;
    public void Init(int maxHp)
    {
        maxHP = maxHp;
        hp = maxHp;
        isInvincible = false;
        isDied = false;
    }

    public bool DecreaseHP(int damage)
    {
        if (isInvincible) return false;

        hp -= damage;
        if (hp <= 0)
        {
            hp = 0;
            return true;
        }
        return false;
    }
}

public class Player : MonoBehaviour
{
    [Header("PlayerStatus")]
    [SerializeField] private PlayerStatus playerStatus;
    public PlayerStatus PlayerStatus => playerStatus;
    [SerializeField] private GameObject postProssecing;
    [SerializeField] private FadeScreen fadeScreen;
    [SerializeField] private DyingLightController dyingLight;
    [SerializeField] private WeaponUI weaponUI;
    [SerializeField] private int maxHp;
    [SerializeField] private UniversalAdditionalCameraData centerEye;
    private Vector3 startPos;
    private Quaternion startRot;
    void Start()
    {
        startPos = transform.position;
        startRot = transform.rotation;
        playerStatus.Init(maxHp);
        GlobalEvent.RegisterEvent<int>(EventType.OnPlayerHealthDecreased, DecreaseHP);
    }

    private void DecreaseHP(int damage)
    {
        if (playerStatus.isInvincible || playerStatus.isDied) return;
        StartCoroutine(DecreaseHPRoutine(damage));
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            DecreaseHP(3);
        }
    }
    private IEnumerator DecreaseHPRoutine(int damage)
    {
        GlobalEvent.CallEvent(EventType.OnPlayerHitEffect);

        if (playerStatus.DecreaseHP(damage))
        {
            GlobalEvent.CallEvent(EventType.UpdateWeaponUI);
            Debug.Log("Player died");
            yield return new WaitForSeconds(1f);
            yield return DieRoutine();
        }
        else
        {
            Debug.Log($"Player HP: {playerStatus.hp}");
            GlobalEvent.CallEvent(EventType.UpdateWeaponUI);
            playerStatus.isInvincible = true;

            yield return new WaitForSeconds(1.5f);

            playerStatus.isInvincible = false;
        }
    }

    private IEnumerator DieRoutine()
    {
        playerStatus.isDied = true;
        postProssecing.SetActive(false);
        WeaponCollection.instance.weaponUI.ActiveUI(true);

        fadeScreen.fadeDuration = 0.1f;
        fadeScreen.FadeOut();
        yield return new WaitForSeconds(fadeScreen.fadeDuration * 2);

        playerStatus.isInvincible = true;

        yield return new WaitForSeconds(2f);

        fadeScreen.fadeDuration = 0.01f;
        fadeScreen.FadeIn();

        yield return dyingLight.FlashCoroutine();
        WeaponCollection.instance.weaponUI.ActiveUI(false);
        fadeScreen.fadeDuration = 2f;
        postProssecing.SetActive(true);
        GlobalEvent.CallEvent(EventType.UpdateWeaponUI);
        yield return new WaitForSeconds(2f);
        playerStatus.Init(maxHp);

    }

    public void SetInvincible(bool value)
    {
        playerStatus.isInvincible = value;
    }

    public void RechargeBarrier()
    {
        // barrierManager.ChargeBarrier();
    }
}
