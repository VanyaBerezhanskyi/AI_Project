using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform playerSpawnPoint;
    [SerializeField] private Transform[] enemySpawnPoints;

    private int enemyCount = 0;

    private void Awake()
    {
        Messenger.AddListener(GameEvent.PLAYER_DEAD, OnPlayerDead);
        Messenger.AddListener(GameEvent.ENEMY_DEAD, OnEnemyDead);
    }

    private void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.PLAYER_DEAD, OnPlayerDead);
        Messenger.RemoveListener(GameEvent.ENEMY_DEAD, OnEnemyDead);
    }

    private void Start()
    {
        SpawnPlayer();
        StartCoroutine(SpawnEnemies(0f));
    }

    private void SpawnPlayer()
    {
        Instantiate(playerPrefab, playerSpawnPoint.position, playerSpawnPoint.rotation);
    }

    private IEnumerator SpawnEnemies(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        foreach (var spawnPoint in enemySpawnPoints)
        {
            Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        }
    }

    private void OnPlayerDead()
    {
        StartCoroutine(RestartLevel());
    }

    private IEnumerator RestartLevel()
    {
        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene("SampleScene");
    }

    private void OnEnemyDead()
    {
        enemyCount++;

        if (enemyCount == enemySpawnPoints.Length)
        {
            enemyCount = 0;

            StartCoroutine(SpawnEnemies(2f));
        }
    }
}

