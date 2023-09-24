using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayState : MonoBehaviour
{
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private EnemyWatcher enemyWatcher;

    private void OnEnable()
    {
        enemyWatcher.enabled = true;
        enemySpawner.enabled = true;
    }

    private void OnDisable()
    {
        enemySpawner.enabled = false;
        enemyWatcher.enabled = false;
        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemy in enemies)
        {
            Destroy(enemy);
        }
    }
}