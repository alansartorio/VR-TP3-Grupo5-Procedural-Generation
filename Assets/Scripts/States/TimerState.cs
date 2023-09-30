using UnityEngine;

[RequireComponent(typeof(GameStateManager))]
public class TimerState : MonoBehaviour
{
    public float duration = 2.5f;
    private GameStateManager _gameStateManager;
    private float _timer;

    private void Start()
    {
        _gameStateManager = GetComponent<GameStateManager>();
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer > duration) _gameStateManager.TimerFinishedCounting();
    }

    private void OnEnable()
    {
        ResetTimer();
    }

    private void ResetTimer()
    {
        _timer = 0;
    }
}