using System.Collections.Generic;
using System.Linq;
using AlanSartorio.GridPathGenerator;
using UnityEngine;

[RequireComponent(typeof(MapGenerator))]
public class EnemySpawner : MonoBehaviour
{
    private List<Path<Vector2Int>> _paths;
    private MapGenerator _mapGenerator;
    [SerializeField] private float spawnInterval = 5;
    private float _spawnTimer = 0;
    [SerializeField] private GameObject enemyPrefab;

    void Awake()
    {
        _mapGenerator = GetComponent<MapGenerator>();
        _mapGenerator.OnMapChanged.AddListener(OnMapChanged);
    }

    private void OnDestroy()
    {
        _mapGenerator.OnMapChanged.RemoveListener(OnMapChanged);
    }

    private void OnMapChanged(GridPathGenerator<Vector2Int> generator)
    {
        Debug.Log("AAAA");
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

    private void SpawnEnemies()
    {
        foreach (var path in _paths)
        {
            var pos = path.Nodes[0];
            var origin = _mapGenerator.GetNodeOrigin(pos);
            var enemy = Instantiate(enemyPrefab);
            enemy.transform.position = origin;
            // enemy.GetComponent<EnemyBehaviour>().SetPath();
        }
    }
}