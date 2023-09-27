using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using CommonUsages = UnityEngine.XR.CommonUsages;
using InputDevice = UnityEngine.XR.InputDevice;

public class ShootingController : MonoBehaviour
{
    public float fireRate = 10f;
    public int damagePerShot = 1;
    public float weaponRange = 100f;
    public Transform gunBarrel;

    private float nextFireTime;
    [SerializeField] private InputActionReference triggerAction;
    
    private void Update()
    {
        if (triggerAction.action.ReadValue<float>() > 0.5f && Time.time > nextFireTime)
        {
            nextFireTime = Time.time + 1f / fireRate;
            Shoot();
        }
    }
    
    
    private void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(gunBarrel.position, gunBarrel.forward, out hit, weaponRange))
        {
            // Verificar si el objeto impactado tiene el nombre "enemy"
            if (hit.transform.CompareTag("Enemy"))
            {
                // Intenta obtener el componente de daño en el objeto impactado
                EnemyBehaviour target = hit.transform.GetComponent<EnemyBehaviour>();

                if (target != null)
                {
                    // Si el objeto impactado tiene un componente de daño, aplica daño
                    target.Damage(damagePerShot);
                }
            }
        }
    }
}
