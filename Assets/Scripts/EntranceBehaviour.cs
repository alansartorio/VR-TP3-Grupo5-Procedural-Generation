using System;
using UnityEngine;

public class EntranceBehaviour : MonoBehaviour
{
    [NonSerialized] public Vector2Int NodePosition;

    public void Expand()
    {
        var gameStateManager = FindObjectOfType<GameStateManager>();
        if (gameStateManager.State != GameState.ExpandMap) return;
        var mapGenerator = FindObjectOfType<MapGenerator>();
        mapGenerator.ExpandNode(NodePosition);
        gameStateManager.PlayerExpandedMap();
    }
}