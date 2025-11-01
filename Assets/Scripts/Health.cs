using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;

    private int currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            if (transform.CompareTag("Player"))
            {
                Messenger.Broadcast(GameEvent.PLAYER_DEAD);
            }
            else if (transform.CompareTag("Enemy"))
            {
                Messenger.Broadcast(GameEvent.ENEMY_DEAD);

                Destroy(gameObject);
            }
        }

        if (transform.CompareTag("Player"))
        {
            Messenger<int>.Broadcast(GameEvent.PLAYER_HURT, currentHealth);
        }
    }
}
