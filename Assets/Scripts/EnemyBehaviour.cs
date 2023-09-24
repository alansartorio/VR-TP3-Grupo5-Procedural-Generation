using System.Collections;
using System.Collections.Generic;
using AlanSartorio.GridPathGenerator;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public GameStateManager gameStateManager;
    public MapGenerator mapGenerator;
    public Path<Vector2Int> Path { get; set; }
    [SerializeField] private float timeBetweenNodes = 2;
    private float _timer = 0;
    private int _nodeIndex = 0;

    void Start()
    {
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
}