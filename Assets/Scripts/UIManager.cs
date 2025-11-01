using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerHP;

    private void Awake()
    {
        Messenger<int>.AddListener(GameEvent.PLAYER_HURT, OnPlayerHurt);
    }

    private void OnDestroy()
    {
        Messenger<int>.RemoveListener(GameEvent.PLAYER_HURT, OnPlayerHurt);
    }

    private void OnPlayerHurt(int currentHealth)
    {
        playerHP.text = "Health: " + currentHealth;
    }
}
