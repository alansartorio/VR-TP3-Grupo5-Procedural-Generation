using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayState : MonoBehaviour
{
    [SerializeField] private EnemySpawner enemySpawner;

    private void OnEnable()
    {
        enemySpawner.ResetTimer();
        enemySpawner.enabled = true;
    }

    private void OnDisable()
    {
        enemySpawner.enabled = false;
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemy in enemies)
        {
            Destroy(enemy);
        }
    }
}