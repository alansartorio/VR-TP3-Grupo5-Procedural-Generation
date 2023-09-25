using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


public class EnemyController : MonoBehaviour
{
    private EnemySpawner _enemySpawner;
    public bool collided = false;
    
    private void Start()
    {
        _enemySpawner = FindObjectOfType<EnemySpawner>();
    }

    public void OnTriggerEnter(Collider collider)
    {
        // Verifica si el objeto alcanzado por el raycast es otro enemigo.
        if (collider.gameObject.CompareTag("Enemy") && !collided)
        {
            collided = true;
            collider.GetComponent<EnemyController>().collided = true;
            _enemySpawner.CombineEnemies(gameObject, collider.gameObject);
        }
    }
}