using UnityEngine;

public class ShootingСharacter : MonoBehaviour
{
    [SerializeField] private float shootRate = 1f;

    private Transform firePoint;
    private float shootTimer;
    private ObjectPool bulletPool;

    private void Awake()
    {
        firePoint = transform.Find("Fire Point");
        shootTimer = shootRate;
    }

    private void Start()
    {
        bulletPool = FindAnyObjectByType<ObjectPool>();
    }

    private void Update()
    {
        shootTimer += Time.deltaTime;
    }

    public void Shoot()
    {
        if (shootTimer >= shootRate)
        {
            shootTimer = 0f;

            GameObject bullet = bulletPool.GetBullet();

            bullet.transform.SetPositionAndRotation(firePoint.position, firePoint.rotation);

            var notifier = bullet.GetComponent<IObjectPoolNotifier>();
            notifier.OnCreatedOrDequeuedFromPool();
        }
    }  
}
