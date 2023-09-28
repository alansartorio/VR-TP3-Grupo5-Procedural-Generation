using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

enum GameState
{
    ExpandMap,
    PlayState,
    TimerState
}

public class GameStateManager : MonoBehaviour
{
    void Start()
    {
        SetState(GameState.ExpandMap);
    }

    void SetState(GameState state)
    {
        GetComponent<ExpandMapState>().enabled = state == GameState.ExpandMap;
        GetComponent<PlayState>().enabled = state == GameState.PlayState;
        GetComponent<TimerState>().enabled = state == GameState.TimerState;
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