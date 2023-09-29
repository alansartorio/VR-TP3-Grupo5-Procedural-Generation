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
    public float Timer = -1;
    public int NodeIndex = -1;
    public float MaxHealth = 2;
    public float Health = 2;
    public UnityEvent<EnemyBehaviour> OnHealthChange;
    [NonSerialized] public UnityEvent OnDeath = new();

    private void Start()
    {
        // transform.localScale = Vector3.one * Health;
        if (Timer < 0)
        {
            Timer = timeBetweenNodes;
        }
    }

    public void FixRotation()
    {
        var delta = Path.Nodes[NodeIndex + 1] - Path.Nodes[NodeIndex];
        var angle = Mathf.Rad2Deg * Mathf.Atan2(delta.x, delta.y);
        transform.rotation = Quaternion.Euler(0, angle, 0);
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

            FixRotation();
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
            Health = 0;
            OnDeath.Invoke();
            Destroy(gameObject);
        }
        OnHealthChange.Invoke(this);
    }
}