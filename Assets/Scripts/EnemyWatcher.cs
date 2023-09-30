using UnityEngine;

[RequireComponent(typeof(EnemySpawner))]
public class EnemyWatcher : MonoBehaviour
{
    [SerializeField] private GameStateManager gameStateManager;
    private int _aliveEnemyCount;
    private EnemySpawner _spawner;

    private void Awake()
    {
        _spawner = GetComponent<EnemySpawner>();
    }

    private void OnEnable()
    {
        _aliveEnemyCount = 0;
        _spawner.OnEnemyDeath.AddListener(OnEnemyDeath);
        _spawner.OnEnemySpawn.AddListener(OnEnemySpawn);
    }

    private void OnDisable()
    {
        _spawner.OnEnemyDeath.RemoveListener(OnEnemyDeath);
        _spawner.OnEnemySpawn.RemoveListener(OnEnemySpawn);
    }

    public void OnEnemySpawn()
    {
        _aliveEnemyCount++;
    }

    public void OnEnemyDeath()
    {
        _aliveEnemyCount--;
        if (_aliveEnemyCount == 0 && _spawner.DidFinishSpawning) gameStateManager.AllEnemiesKilled();
    }
}