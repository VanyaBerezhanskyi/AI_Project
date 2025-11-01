using System.Collections.Generic;
using UnityEngine;

public interface IObjectPoolNotifier
{
    public void OnCreatedOrDequeuedFromPool();
}

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;

    private Queue<GameObject> bulletPool = new Queue<GameObject>();

    public GameObject GetBullet()
    {
        if (bulletPool.Count > 0)
        {
            GameObject bullet = bulletPool.Dequeue();

            bullet.SetActive(true);
            bullet.transform.parent = null;

            return bullet;
        }
        else
        {
            GameObject newBullet = Instantiate(bulletPrefab);

            newBullet.GetComponent<Bullet>().owner = this;

            return newBullet;
        }
    }

    public void ReturnBullet(GameObject bullet)
    {
        bullet.SetActive(false);
        bullet.transform.parent = transform;

        bulletPool.Enqueue(bullet);
    }
}
