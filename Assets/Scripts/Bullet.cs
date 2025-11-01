using UnityEngine;

public class Bullet : MonoBehaviour, IObjectPoolNotifier
{
    [SerializeField] private float speed = 20f;
    [SerializeField] private int damage = 20;

    public ObjectPool owner;

    private Rigidbody rb;

    public void OnCreatedOrDequeuedFromPool()
    {
        rb.linearVelocity = transform.forward * speed;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Health health = collision.gameObject.GetComponent<Health>();

        if (health != null)
        {
            health.TakeDamage(damage);
        }

        owner.ReturnBullet(gameObject);
    }
}
