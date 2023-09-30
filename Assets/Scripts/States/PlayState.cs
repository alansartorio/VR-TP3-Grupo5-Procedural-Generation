using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayState : MonoBehaviour
{
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private EnemyWatcher enemyWatcher;

    private void OnEnable()
    {
        enemyWatcher.enabled = true;
        enemySpawner.enabled = true;

        FindObjectOfType<ShootingController>().enabled = true;
    }

    private void OnDisable()
    {
        FindObjectOfType<ShootingController>().enabled = false;

        enemySpawner.enabled = false;
        enemyWatcher.enabled = false;

        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemy in enemies)
        {
            Destroy(enemy);
        }
    }
}