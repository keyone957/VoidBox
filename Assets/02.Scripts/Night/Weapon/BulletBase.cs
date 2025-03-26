using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public enum BulletType
{
    None,
    Normal,
    Pierce,
    Spread,
}

public class BulletBase : MonoBehaviour
{
    public BulletType bulletType;
    public WeaponBase weaponBase { get; set; }

    private float explosionScale = 0.5f;
    private Rigidbody rb;
    private Coroutine coroutine;

    private List<GameObject> explosionList = new List<GameObject>();
    private List<GameObject> bulletMarkerList = new List<GameObject>();

    [SerializeField] private float bulletSpeed;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private GameObject bulletMarker;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void OnEnable()
    {
        if (this.gameObject.activeInHierarchy) coroutine = StartCoroutine(Invisible(3f));
    }
    private void OnTriggerEnter(Collider other)
    {
        this.gameObject.SetActive(false);
    }
    private void OnCollisionEnter(Collision other)
    {
        this.gameObject.SetActive(false);
    }
    private void OnDisable()
    {
        StopCoroutine(coroutine);
    }
    public void OnWeaponTrigger(Vector3 direction, Vector3 hitPoint)
    {
        rb.velocity = direction * bulletSpeed;
        transform.rotation = weaponBase.projectilePoint.rotation;
        SpawnExplosion();
        SpawnBulletMark(hitPoint);
    }
    private void SpawnExplosion()
    {
        foreach (GameObject explosionEffect in explosionList)
        {
            if (!explosionEffect.activeInHierarchy)
            {
                explosionEffect.SetActive(true);
                explosionEffect.transform.position = weaponBase.muzzlePoint.position;
                explosionEffect.transform.localScale *= explosionScale;
                return;
            }
        }

        GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        explosionList.Add(explosion);
        explosion.SetActive(true);
        explosion.transform.position = weaponBase.muzzlePoint.position;
        explosion.transform.localScale *= explosionScale;
    }
    private void SpawnBulletMark(Vector3 hitPoint)
    {
        foreach (GameObject marker in bulletMarkerList)
        {
            if (!marker.activeInHierarchy)
            {
                marker.SetActive(true);
                marker.transform.position = hitPoint;
                return;
            }
        }
        GameObject mark = Instantiate(bulletMarker, hitPoint, Quaternion.identity);
        bulletMarkerList.Add(mark);
        mark.SetActive(true);
    }

    public IEnumerator Invisible(float timer)
    {
        yield return new WaitForSeconds(timer);
        this.gameObject.SetActive(false);
    }
}
