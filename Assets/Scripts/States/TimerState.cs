using System;
using UnityEngine;

[RequireComponent(typeof(GameStateManager))]
public class TimerState : MonoBehaviour
{
    private GameStateManager _gameStateManager;
    public float duration = 2.5f;
    private float _timer;
    
    void Start()
    {
        _gameStateManager = GetComponent<GameStateManager>();
    }

    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer > duration)
        {
            _gameStateManager.TimerFinishedCounting();
        }
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
