using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.UI;
public class WeaponBase : MonoBehaviour
{
    [SerializeField] protected WeaponState state;
    public WeaponState State => state;

    protected DamageType damageType;

    [SerializeField] private WeaponData weapon;
    public WeaponData _weapon => weapon;
    public WeaponType weaponType;

    [Header("WeaponPoints")]
    public Transform rayPoint;
    public Transform projectilePoint;
    public Transform laserPointPos;
    public Transform weaponUIPosition;
    public Transform muzzlePoint;

    [Space]
    [SerializeField] protected Transform reloadPoint;
    [SerializeField] private GameObject bullet;
    [SerializeField] private List<GameObject> bullets = new List<GameObject>();

    [SerializeField] private int magazine;
    public int _magazine => magazine;
    private int ammoMax;
    public int _ammoMax => ammoMax;
    private int damage;
    public int _damage => damage;

    public bool isGrabed {  get; set; }

    private LayerMask enemyLayer;
    void Start()
    {
        enemyLayer = 1 >> 8;
        ammoMax = 2000;
        magazine = weapon._maxMagazine;
        state = WeaponCollection.instance.endTutorial ? WeaponState.Idle : WeaponState.None;
        damage = (int)weapon._damage;

        GlobalEvent.CallEvent(EventType.UpdateWeaponUI);
    }
    public void ActiveGrab()
    {
        if (isGrabed == false)
        {
            isGrabed = true;
            return;
        }
        else 
        { 
            isGrabed = false;
        }

    }
    public void OnEnable()
    {
        damageType = weapon._damageType;
    }

    public void EndTutorial()
    {
        state = WeaponState.Idle;
        WeaponCollection.instance.endTutorial = true;
    }
    /// <summary>
    /// ㅠㅠ
    /// </summary>
    /// <param name="damageType"></param>
    /// <param name="target"></param>
    /// <param name="enemy"></param>
    public void DecreaseHP(DamageType damageType, Transform target = null, EnemyFSM enemy = null)
    {
        if (damageType == DamageType.Normal)
        {
            enemy.TakeDamage(damage);
        }
        else if (damageType == DamageType.DistanceBase)
        {
            int damage = CalculateDamage(Vector3.Distance(rayPoint.position, enemy.transform.position));
            enemy.TakeDamage(damage);
        }
    }
    public void DecreaseHP(DamageType damageType, Transform target = null, CoverFSM enemy = null)
    {
        if (damageType == DamageType.Normal)
        {
            enemy.TakeDamage(damage);
        }
        else if (damageType == DamageType.DistanceBase)
        {
            int damage = CalculateDamage(Vector3.Distance(rayPoint.position, enemy.transform.position));
            enemy.TakeDamage(damage);
        }
    }
    public void DecreaseHP(DamageType damageType, Transform target = null, SuicideFSM enemy = null)
    {
        if (damageType == DamageType.Normal)
        {
            enemy.TakeDamage(damage);
            Debug.Log(enemy.name);
        }
        else if (damageType == DamageType.DistanceBase)
        {
            int damage = CalculateDamage(Vector3.Distance(rayPoint.position, enemy.transform.position));
            enemy.TakeDamage(damage);
        }
    }
    #region Other
    private int CalculateDamage(float distance)
    {
        float minDamage = weapon._minDamage;
        float maxDamage = weapon._maxDamage;
        float maxDistance = weapon._range; // 무기의 최대 사거리

        float damage = Mathf.Lerp(maxDamage, minDamage, distance / maxDistance);
        return Mathf.Clamp(Mathf.RoundToInt(damage), Mathf.RoundToInt(minDamage), Mathf.RoundToInt(maxDamage));
    }

    #endregion

    #region Fire
    public GameObject GetPooledBullet(BulletType type = BulletType.Normal)
    {
        foreach (GameObject bullet in bullets)
        {
            if (!bullet.activeInHierarchy)
            {
                bullet.SetActive(true);
                bullet.transform.position = projectilePoint.position;
                bullet.GetComponent<BulletBase>().bulletType = type;
                bullet.transform.SetParent(this.transform);
                return bullet;
            }
        }

        GameObject bulletObj = Instantiate(bullet, projectilePoint.position, Quaternion.identity);
        bulletObj.GetComponent<BulletBase>().bulletType = type;
        bullets.Add(bulletObj);
        bulletObj.transform.SetParent(this.transform);
        return bulletObj;
    }

    /// <summary>
    /// 1발 사격
    /// </summary>
    /// <returns></returns>
    public IEnumerator SingleShot(WeaponBase weapon, BulletType type = BulletType.Normal)
    {
        state = WeaponState.Shooting;
        HapticManager.instance.StartHaptic(40, 2 , 255, OVRInput.Controller.RTouch);
        if (weapon._weapon.WeaponSound is not null) SoundManager.instance.PlaySound(weapon._weapon.WeaponSound.name, SoundType.SFX);
        if (Physics.Raycast(rayPoint.position, rayPoint.forward, out var hit, Mathf.Infinity))
        {
            //...a
            if (hit.transform.GetComponentInParent<EnemyFSM>())
            {
                var enemy = hit.transform.GetComponentInParent<EnemyFSM>();
                weapon.DecreaseHP(weapon.damageType, hit.transform, enemy);
            }
            else if (hit.transform.GetComponentInParent<CoverFSM>())
            {
                var enemy = hit.transform.GetComponentInParent<CoverFSM>();
                weapon.DecreaseHP(weapon.damageType, hit.transform, enemy);
            }
            else if (hit.transform.GetComponentInParent<SuicideFSM>())
            {
                var enemy = hit.transform.GetComponentInParent<SuicideFSM>();
                weapon.DecreaseHP(weapon.damageType, hit.transform, enemy);
            }
        }
        Vector3 offsetPoint = hit.point + hit.normal * 0.01f;

        var b = GetPooledBullet(type);
        BulletBase newBullet = b.GetComponent<BulletBase>();
        newBullet.weaponBase = this;
        newBullet.OnWeaponTrigger(projectilePoint.forward, offsetPoint);
        magazine--;
        GlobalEvent.CallEvent(EventType.UpdateWeaponUI);
        if (magazine == 0) yield return Reload();

        yield return new WaitForSeconds(weapon._weapon._fireRate);

        state = WeaponState.Idle;
    }

    /// <summary>
    /// 샷건
    /// </summary>
    /// <param name="bulletIdx"> 산탄 총알 숫자 </param>
    /// <param name="spreadAngle"> 퍼지는 정도 </param>
    /// <returns></returns>
    public IEnumerator ScatterShot(WeaponBase weapon, int bulletIdx, float spreadAngle)
    {
        state = WeaponState.Shooting;
        HapticManager.instance.StartHaptic(80, 3, 255 * 2, OVRInput.Controller.RTouch);
        if (weapon._weapon.WeaponSound is not null) SoundManager.instance.PlaySound(weapon._weapon.WeaponSound.name, SoundType.SFX);
        RaycastHit[] hitPoints = new RaycastHit[bulletIdx];

        for (int i = 0; i < 5; i++)
        {
            float randomIdx = Random.Range(-180, 180);

            float angle = (360f + randomIdx) / 5 * i;

            float angleX = Mathf.Sin(angle * Mathf.Deg2Rad) * spreadAngle;
            float angleY = Mathf.Cos(angle * Mathf.Deg2Rad) * spreadAngle;

            Vector3 dir = Quaternion.Euler(angleX, angleY, 0) * rayPoint.forward;

            if (Physics.Raycast(rayPoint.position, dir, out var hit, Mathf.Infinity, enemyLayer))
            {
                Debug.Log(hit.transform.name);
                hitPoints[i] = hit;

                if (hit.transform.GetComponent<EnemyFSM>())
                {
                    var enemy = hit.transform.GetComponentInParent<EnemyFSM>();
                    weapon.DecreaseHP(weapon._weapon._damageType, hit.transform, enemy);
                }
                else if (hit.transform.GetComponent<CoverFSM>())
                {
                    var enemy = hit.transform.GetComponentInParent<CoverFSM>();
                    weapon.DecreaseHP(weapon._weapon._damageType, hit.transform, enemy);
                }
                else if (hit.transform.GetComponent<SuicideFSM>())
                {
                    var enemy = hit.transform.GetComponentInParent<SuicideFSM>();
                    weapon.DecreaseHP(weapon._weapon._damageType, hit.transform, enemy);
                }
                else Instantiate(bullet, hit.transform.position, Quaternion.identity); //
            }
        }

        GlobalEvent.CallEvent(EventType.UpdateWeaponUI);

        List<BulletBase> bullets = new List<BulletBase>();

        for (int i = 0; i < bulletIdx; i++)
        {
            float angleX = Random.Range(-spreadAngle, spreadAngle);
            float angleY = Random.Range(-spreadAngle, spreadAngle);

            Vector3 dir = Quaternion.Euler(angleX, angleY, 0) * projectilePoint.forward;
            Quaternion rot = hitPoints[i].transform.rotation;

            var b = GetPooledBullet();
            BulletBase newBullet = b.GetComponent<BulletBase>();
            newBullet.weaponBase = this;
            var point = hitPoints[i];
            Vector3 offsetPoint = point.point + point.normal * 0.01f;
            newBullet.OnWeaponTrigger(dir, offsetPoint);
            bullets.Add(newBullet);
        }

        magazine--;
        yield return new WaitForSeconds(weapon._weapon._fireRate);

        if (magazine == 0) yield return PumpReload();

        state = WeaponState.Idle;

    }
    #endregion

    #region Reload
    private IEnumerator Firearmjam()
    {
        while (state == WeaponState.Reloding)
        {
            yield return new WaitForSeconds(2f);
            break;
        }
        state = WeaponState.Idle;
    }
    public IEnumerator Reload()
    {
        state = WeaponState.Reloding;
        StartCoroutine(Firearmjam());
        if (weapon._maxMagazine > magazine && ammoMax > 0)
        {
            int dis = weapon._maxMagazine - magazine;
            yield return new WaitForSeconds(weapon._reloadTime);

            magazine = weapon._maxMagazine;
            ammoMax -= dis;
            state = WeaponState.Idle;
            GlobalEvent.CallEvent(EventType.UpdateWeaponUI);
        }
    }
    public IEnumerator PumpReload()
    {
        state = WeaponState.Reloding;

        while (weapon._maxMagazine > magazine && ammoMax > 0)
        {
            yield return new WaitForSeconds(weapon._reloadTime);

            if (magazine > 0)
            {
                state = WeaponState.Idle;

                if (state == WeaponState.Shooting) break;
            }
            magazine++;
            ammoMax--;
            GlobalEvent.CallEvent(EventType.UpdateWeaponUI);
        }
    }

    #endregion
}
