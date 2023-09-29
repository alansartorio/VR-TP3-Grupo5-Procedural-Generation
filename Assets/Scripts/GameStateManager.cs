using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
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
    public GameState State { get; private set; }

    [SerializeField] private TMP_Text text;
    
    void Start()
    {
        SetState(GameState.ExpandMap);
    }

    void SetState(GameState state)
    {
        State = state;
        GetComponent<ExpandMapState>().enabled = state == GameState.ExpandMap;
        GetComponent<PlayState>().enabled = state == GameState.PlayState;
        GetComponent<TimerState>().enabled = state == GameState.TimerState;

        var message = state switch
        {
            GameState.ExpandMap => "Find and Press a Button\nto Expand map",
            GameState.TimerState => "Round Starting",
            GameState.PlayState => "Burn The Spiders",
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
    }

    public void PlayerExpandedMap()
    {
        SetState(GameState.TimerState);
    }
}