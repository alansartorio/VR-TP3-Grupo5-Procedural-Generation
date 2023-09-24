using UnityEngine;

[RequireComponent(typeof(EnemySpawner))]
public class EnemyWatcher : MonoBehaviour
{
    private int _aliveEnemyCount;
    private EnemySpawner _spawner;
    [SerializeField] private GameStateManager gameStateManager;

    private void Awake()
    {
        _spawner = GetComponent<EnemySpawner>();
    }

    void OnEnable()
    {
        _aliveEnemyCount = 0;
        _spawner.OnEnemyDeath.AddListener(OnEnemyDeath);
        _spawner.OnEnemySpawn.AddListener(OnEnemySpawn);
    }

    void OnDisable()
    {
        _spawner.OnEnemyDeath.RemoveListener(OnEnemyDeath);
        _spawner.OnEnemySpawn.RemoveListener(OnEnemySpawn);
    }

    private void OnEnemySpawn()
    {
        _aliveEnemyCount++;
    }

    private void OnEnemyDeath()
    {
        _aliveEnemyCount--;
        if (_aliveEnemyCount == 0 && _spawner.DidFinishSpawning)
        {
            gameStateManager.AllEnemiesKilled();
        }
    }
}