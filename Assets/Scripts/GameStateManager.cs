using System;
using TMPro;
using UnityEngine;

public enum GameState
{
    ExpandMap,
    PlayState,
    TimerState
}

public class GameStateManager : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private AudioClip roundFinishedSound;
    private AudioSource _audioSource;
    private int _roundNumber = 1;
    public GameState State { get; private set; }

    private void Start()
    {
        SetState(GameState.ExpandMap);
        _audioSource = GetComponent<AudioSource>();
    }

    private void SetState(GameState state)
    {
        State = state;
        GetComponent<ExpandMapState>().enabled = state == GameState.ExpandMap;
        GetComponent<PlayState>().enabled = state == GameState.PlayState;
        GetComponent<TimerState>().enabled = state == GameState.TimerState;

        var message = state switch
        {
            GameState.ExpandMap => $"Find and Press a Button\nto start Round {_roundNumber}",
            GameState.TimerState => $"Round {_roundNumber} Starting",
            GameState.PlayState => "Burn The Spiders",
            _ => throw new ArgumentOutOfRangeException(nameof(state), state, null)
        };

        text.SetText(message);
    }


    public void EnemyReachedBase()
    {
        SetState(GameState.TimerState);
    }

    public void TimerFinishedCounting()
    {
        SetState(GameState.PlayState);
    }

    public void AllEnemiesKilled()
    {
        SetState(GameState.ExpandMap);
        _audioSource.PlayOneShot(roundFinishedSound);
        _roundNumber++;
    }

    public void PlayerExpandedMap()
    {
        SetState(GameState.TimerState);
    }
}