using System;
using System.Collections.Generic;
using System.Linq;
using AlanSartorio.GridPathGenerator;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
    private List<Path<Vector2Int>> _paths;
    [SerializeField] private MapGenerator mapGenerator;
    [SerializeField] private GameStateManager gameStateManager;
    [SerializeField] private float spawnInterval = 5;
    private float _spawnTimer = 0;
    [SerializeField] private int targetSpawnAmount = 5;
    private int _spawnCount = 0;
    [NonSerialized] public UnityEvent OnEnemyDeath = new();
    [NonSerialized] public UnityEvent OnEnemySpawn = new();
    [SerializeField] private List<GameObject> enemyPrefabs;

    public bool DidFinishSpawning => _spawnCount >= targetSpawnAmount;

    void Awake()
    {
        mapGenerator.OnMapChanged.AddListener(OnMapChanged);
    }

    private void OnDestroy()
    {
        mapGenerator.OnMapChanged.RemoveListener(OnMapChanged);
    }

    private void OnMapChanged(GridPathGenerator<Vector2Int> generator)
    {
        _paths = generator.GetPathsFromLeaves().ToList();
    }

    void Update()
    {
        _spawnTimer += Time.deltaTime;
        while (_spawnTimer > spawnInterval)
        {
            _spawnTimer -= spawnInterval;
            SpawnEnemies();
        }
    }

    private void OnEnable()
    {
        ResetTimer();
        ResetSpawnCount();
    }

    private GameObject GetEnemyPrefab(EnemyLevel enemyLevel)
    {
        return enemyPrefabs[(int)enemyLevel];
    }

    private void SpawnEnemies()
    {
        _spawnCount++;
        if (DidFinishSpawning)
            enabled = false;

        foreach (var path in _paths)
        {
            var pos = path.Nodes[0];
            var origin = mapGenerator.GetNodeOrigin(pos);
            var enemy = Instantiate(GetEnemyPrefab(EnemyLevel.Enemy));
            enemy.transform.position = origin;
            var enemyBehaviour = enemy.GetComponent<EnemyBehaviour>();
            OnEnemySpawn.Invoke();
            enemyBehaviour.OnDeath.AddListener(OnDeath);
            enemyBehaviour.Path = path;
            enemyBehaviour.gameStateManager = gameStateManager;
            enemyBehaviour.mapGenerator = mapGenerator;
        }
    }

    public void CombineEnemies(GameObject enemy1, GameObject enemy2)
    {
        var scriptEnemy1 = enemy1.GetComponent<EnemyBehaviour>();
        // var scriptEnemy2 = enemy2.GetComponent<EnemyBehaviour>();

        var path = scriptEnemy1.Path;
        var nodeIndex = scriptEnemy1.NodeIndex;
        var timer = scriptEnemy1.Timer;
        var mapGenerator = scriptEnemy1.mapGenerator;
        var gameStateManager = scriptEnemy1.gameStateManager;

        var enemy1level = enemy1.GetComponent<EnemyController>().enemyLevel;
        var enemy2level = enemy2.GetComponent<EnemyController>().enemyLevel;

        // Destruye el enemigo actual.
        Destroy(enemy1);

        // Destruye al enemigo alcanzado por el raycast.
        Destroy(enemy2);

        var combinedLevel = (EnemyLevel)(Math.Max((int)enemy1level, (int)enemy2level) + 1);

        // Genera un nuevo jefe.
        var bossObject = Instantiate(GetEnemyPrefab(combinedLevel), transform.position, Quaternion.identity);
        var boss = bossObject.GetComponent<EnemyBehaviour>();
        boss.Path = path;
        boss.NodeIndex = nodeIndex;
        boss.Timer = timer;
        boss.mapGenerator = mapGenerator;
        boss.gameStateManager = gameStateManager;
        boss.FixRotation();
        
        OnDeath();
    }

    private void OnDeath()
    {
        OnEnemyDeath.Invoke();
    }

    private void ResetTimer()
    {
        _spawnTimer = 0;
    }

    private void ResetSpawnCount()
    {
        _spawnCount = 0;
    }
}