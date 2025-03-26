using UnityEngine;

public enum WeaponType
{
    None,
    Pistol,
    Shotgun,
    ChargiedRifle,
}
public enum WeaponState
{
    None,
    Idle,
    Shooting,
    Reloding,
}
public enum DamageType
{
    None,
    Normal,
    DistanceBase,
}
[CreateAssetMenu(fileName = "WeaponData", menuName = "Scriptable/WeaponData", order = int.MaxValue)]
public class WeaponData : ScriptableObject
{
    //무기 타입
    [SerializeField] private WeaponType weaponType;
    public WeaponType _weaponType
    {
        get { return weaponType; } 
        set { weaponType = value; } 
    }
    //데미지 타입
    [SerializeField] private DamageType damageType;
    public DamageType _damageType
    {
        get { return damageType; }
        set { damageType = value; }
    }

    //장전시간
    [SerializeField] private float reloadTime;
    public float _reloadTime
    {
        get { return reloadTime; }
        set { reloadTime = value; }
    }

    //기본 데미지
    [SerializeField] private int damage;
    public int _damage
    {
        get { return damage; }
        set { damage = value; }
    }

    //최대 데미지
    [SerializeField] private int maxDamage;
    public int _maxDamage
    {
        get { return maxDamage; }
        set { maxDamage = value; }
    }

    //최소 데미지
    [SerializeField] private int mixDamage;
    public int _minDamage
    {
        get { return mixDamage; }
        set { mixDamage = value; }
    }

    //연사 속도
    [SerializeField] private float fireRate;
    public float _fireRate
    {
        get { return fireRate; }
        set { fireRate = value; }
    }

    //반동
    [SerializeField] private float recoil;
    public float _recoil
    {
        get { return recoil; }
        set { recoil = value; }
    }
    
    //최대 탄창
    [SerializeField] private int maxMagazine;
    public int _maxMagazine
    {
        get { return maxMagazine; }
        set { maxMagazine = value; } 
    }

    //최대 사거리
    [SerializeField] private int range;
    public int _range
    {
        get { return range; }
        set { range = value; }
    }
    [SerializeField] private AudioClip weaponSound;
    public AudioClip WeaponSound => weaponSound;
}
