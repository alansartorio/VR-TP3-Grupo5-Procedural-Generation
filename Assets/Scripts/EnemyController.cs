using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


public enum EnemyLevel
{
    Enemy = 0,
    Boss = 1,
    SuperBoss = 2,
}

public class EnemyController : MonoBehaviour
{
    private EnemySpawner _enemySpawner;
    public bool collided = false;
    public EnemyLevel enemyLevel = EnemyLevel.Enemy;

    private void Start()
    {
        _enemySpawner = FindObjectOfType<EnemySpawner>();
    }

    public void OnTriggerEnter(Collider collider)
    {
        // Verifica si el objeto alcanzado por el raycast es otro enemigo.
        if (collider.gameObject.CompareTag("Enemy") && !collided)
        {
            var otherEnemy = collider.GetComponent<EnemyController>();
            if ((int)enemyLevel >= (int)EnemyLevel.SuperBoss ||
                (int)otherEnemy.enemyLevel >= (int)EnemyLevel.SuperBoss) return;
            collided = true;
            collider.GetComponent<EnemyController>().collided = true;
            _enemySpawner.CombineEnemies(gameObject, collider.gameObject);
        }
    }
}