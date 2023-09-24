using System;
using System.Collections;
using System.Collections.Generic;
using AlanSartorio.GridPathGenerator;
using UnityEngine;
using UnityEngine.Events;

public class EnemyBehaviour : MonoBehaviour
{
    public GameStateManager gameStateManager;
    public MapGenerator mapGenerator;
    public Path<Vector2Int> Path { get; set; }
    [SerializeField] private float timeBetweenNodes = 2;
    private float _timer = 0;
    private int _nodeIndex = 0;
    private float _health = 2;
    [NonSerialized] public UnityEvent OnDeath = new();

    private void Start()
    {
        transform.localScale = Vector3.one * _health;
    }

    void Update()
    {
        _timer += Time.deltaTime;
        while (_timer > timeBetweenNodes)
        {
            _timer -= timeBetweenNodes;
            _nodeIndex++;

            // Reached base
            if (_nodeIndex >= Path.Nodes.Count - 1)
            {
                Destroy(gameObject);
                gameStateManager.EnemyReachedBase();
                return;
            }
        }

        var progress = _timer / timeBetweenNodes;

        var position = Vector3.Lerp(
            mapGenerator.GetNodeOrigin(Path.Nodes[_nodeIndex]),
            mapGenerator.GetNodeOrigin(Path.Nodes[_nodeIndex + 1]),
            progress
        );

        transform.position = position;
    }

    public void Damage(float damage)
    {
        _health -= damage;
        if (_health <= 0)
        {
            OnDeath.Invoke();
            Destroy(gameObject);
        }
    }
}