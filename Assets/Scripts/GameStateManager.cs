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
        SetState(GameState.PlayState);
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
}
