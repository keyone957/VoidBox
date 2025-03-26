using Knife.Effects.SimpleController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCollection : MonoBehaviour
{
    public static WeaponCollection instance {  get; private set; }

    [SerializeField] private Transform newWeaponPos;
    public Transform NewWeaponPos => newWeaponPos;

    private WeaponBase prevWeapon;
    public WeaponBase currentWeapon;
    
    [SerializeField] private List<WeaponBase> _weapons; //all weapons
    public List<WeaponBase> weapons => _weapons;

    //waveClear시 무기 해금기능이 활성화 된다면 _weapons에서 이것으로 변경 예정
    public List<WeaponBase> availableWeapons;
    [SerializeField] private LaserPointer laser;

    public WeaponUI weaponUI;
    public bool endTutorial { get; set; }
    private bool isSwitching = false;

    public void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != null) Destroy(this);

    }
    private void Start()
    {
        Init();
    }
    public void Init()
    {
        availableWeapons.Clear();
        availableWeapons.Add(weapons[0]);
        GlobalEvent.CallEvent(EventType.UpdateWeaponUI);
    }
    private void Update()
    {
        SwitchWeapon();
    }
    public void SwitchWeapon()
    {
        if (availableWeapons.Count <= 1) return;

        if (!isSwitching)
        {
            if (OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick).x >= 0.5f ||
                OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).x >= 0.5f)
            {
                Debug.Log("무기 교체 ++");
                StartCoroutine(SwitchWeaponToStick(true));
            }
            else if (OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick).x <= -0.5f ||
                OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).x <= -0.5f)
            {
                Debug.Log("무기 교체 --");
                StartCoroutine(SwitchWeaponToStick(false));
            }
        }
    }
    public IEnumerator SwitchWeaponToStick(bool isNext)
    {
        isSwitching = true;
        prevWeapon = currentWeapon;
        var nextWeapon = new WeaponBase();

        if (isNext)
        {
            nextWeapon = availableWeapons.IndexOf(currentWeapon) == availableWeapons.Count - 1 ?
                availableWeapons[0] : availableWeapons[availableWeapons.IndexOf(currentWeapon) + 1];
        }
        else
        {
            nextWeapon = currentWeapon == availableWeapons[0] ? 
                availableWeapons[availableWeapons.Count - 1] : availableWeapons[availableWeapons.IndexOf(currentWeapon) - 1];
        }
        SwitchWeapon(nextWeapon);
        yield return new WaitForSeconds(2f);
        Debug.Log("endChangeWeapon");
        isSwitching = false;
    }
    public void GainNewWeapon()
    {
        var currentWeapon = availableWeapons[availableWeapons.Count - 1];
        var newWeapon = weapons[weapons.IndexOf(currentWeapon) + 1];
        availableWeapons.Add(newWeapon);
    }
    public void GainNewWeapon(WeaponBase weapon)
    {
        availableWeapons.Add(weapon);
    }
    public void SwitchWeapon(WeaponBase weapon)
    {
        if (currentWeapon is not null) currentWeapon.transform.gameObject.SetActive(false);
        currentWeapon = weapon;
        currentWeapon.transform.gameObject.SetActive(true);
        currentWeapon.transform.position = newWeaponPos.position;
        laser.bulletPoint = currentWeapon.laserPointPos;

        weaponUI.ActiveUI(false);
        weaponUI.SetWeaponUIPosition(currentWeapon.weaponUIPosition);
        GlobalEvent.CallEvent(EventType.UpdateWeaponUI);
    }
    public void ThrowWeapon(WeaponBase weapon)
    {
        weapons.Remove(weapon);
        if (currentWeapon == weapon.transform) currentWeapon = weapons[0];
    }
}
