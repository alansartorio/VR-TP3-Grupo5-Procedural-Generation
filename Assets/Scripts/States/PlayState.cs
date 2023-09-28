using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayState : MonoBehaviour
{
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private EnemyWatcher enemyWatcher;
    // [SerializeField] private InputActionReference pointerPosition, attack;

    private void OnEnable()
    {
        enemyWatcher.enabled = true;
        enemySpawner.enabled = true;

        FindObjectOfType<ShootingController>().enabled = true;
        // pointerPosition.action.performed += PointerMoved;
        // attack.action.performed += AttackPerformed;
    }

    private void OnDisable()
    {
        // pointerPosition.action.performed -= PointerMoved;
        // attack.action.performed -= AttackPerformed;
        FindObjectOfType<ShootingController>().enabled = false;

        enemySpawner.enabled = false;
        enemyWatcher.enabled = false;

        var enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemy in enemies)
        {
            Destroy(enemy);
        }
    }

    // private void AttackPerformed(InputAction.CallbackContext ctx)
    // {
    //     var pos = pointerPosition.action.ReadValue<Vector2>();
    //     var ray = Camera.main.ScreenPointToRay(pos);
    //
    //     if (!Physics.Raycast(ray, out var hitInfo))
    //         return;
    //
    //     var hitTransform = hitInfo.transform;
    //     if (hitTransform == null || !hitTransform.CompareTag("Enemy"))
    //         return;
    //
    //     var enemy = hitTransform.GetComponent<EnemyBehaviour>();
    //     enemy.Damage(2);
    // }
}