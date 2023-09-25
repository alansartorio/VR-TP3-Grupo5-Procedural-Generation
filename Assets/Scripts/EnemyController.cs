using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyController : MonoBehaviour
{
    private EnemySpawner _enemySpawner;

    private void Start()
    {
        _enemySpawner = FindObjectOfType<EnemySpawner>();
    }

    private void Update()
    {
        // Raycast hacia adelante desde el enemigo.
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 1f))
        {
            // Verifica si el objeto alcanzado por el raycast es otro enemigo.
            if (hit.collider.CompareTag("Enemy") && hit.collider.gameObject != null)
            {
                _enemySpawner.CombineEnemies(gameObject, hit.collider.gameObject);
            }
        }
    }
}