using System;
using AlanSartorio.GridPathGenerator;
using UnityEngine;
using UnityEngine.Events;

public class EnemyBehaviour : MonoBehaviour
{
    public GameStateManager gameStateManager;
    public MapGenerator mapGenerator;
    [SerializeField] private float timeBetweenNodes = 2;
    public float Timer = -1;
    public int NodeIndex = -1;
    public float MaxHealth = 2;
    public float Health = 2;
    public UnityEvent<EnemyBehaviour> OnHealthChange;

    [SerializeField] private AudioSource hitAudioSource;
    [SerializeField] private AudioClip deathAudioClip;
    [NonSerialized] public UnityEvent OnDeath = new();
    public Path<Vector2Int> Path { get; set; }

    private void Start()
    {
        // transform.localScale = Vector3.one * Health;
        if (Timer < 0) Timer = timeBetweenNodes;
    }

    private void Update()
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

    public void FixRotation()
    {
        var delta = Path.Nodes[NodeIndex + 1] - Path.Nodes[NodeIndex];
        var angle = Mathf.Rad2Deg * Mathf.Atan2(delta.x, delta.y);
        transform.rotation = Quaternion.Euler(0, angle, 0);
    }

    public void Damage(float damage)
    {
        if (!hitAudioSource.isPlaying)
            hitAudioSource.Play();
        Health -= damage;
        if (Health <= 0)
        {
            Health = 0;
            OnDeath.Invoke();
            AudioSource.PlayClipAtPoint(deathAudioClip, transform.position);
            Destroy(gameObject);
        }

        OnHealthChange.Invoke(this);
    }
}