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
    public float Timer = 0;
    public int NodeIndex = 0;
    public float Health = 2;
    [NonSerialized] public UnityEvent OnDeath = new();

    private void Start()
    {
        // transform.localScale = Vector3.one * Health;
    }

    void Update()
    {
        Timer += Time.deltaTime;
        while (Timer > timeBetweenNodes)
        {
            Timer -= timeBetweenNodes;
            NodeIndex++;

            // Reached base
            if (NodeIndex >= Path.Nodes.Count - 1)
            {
                Destroy(gameObject);
                gameStateManager.EnemyReachedBase();
                return;
            }
        }

        var progress = Timer / timeBetweenNodes;

        var position = Vector3.Lerp(
            mapGenerator.GetNodeOrigin(Path.Nodes[NodeIndex]),
            mapGenerator.GetNodeOrigin(Path.Nodes[NodeIndex + 1]),
            progress
        );

        transform.position = position;
    }

    public void Damage(float damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            OnDeath.Invoke();
            Destroy(gameObject);
        }
    }
}