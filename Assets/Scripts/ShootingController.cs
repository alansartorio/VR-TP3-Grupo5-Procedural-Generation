using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using CommonUsages = UnityEngine.XR.CommonUsages;
using InputDevice = UnityEngine.XR.InputDevice;

public class ShootingController : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem part;
    [SerializeField]
    private ParticleSystem OnFireSystemPrefab;
    public float fireRate = 10f;
    public int damagePerShot = 1;
    public float weaponRange = 10f;
    public Transform gunBarrel;

    private float nextFireTime;

    [SerializeField]
    private AudioClip shootingSound; // Variable serializada para el clip de audio.
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>(); // Obtener el componente AudioSource.
        
        // Asegúrate de que el componente de Audio Source esté configurado.
        if (audioSource == null)
        {
             Debug.LogError("El componente AudioSource no está asignado en el Inspector.");
        }
        else
        {
            // Asigna el clip de audio si se proporciona en el Inspector.
            if (shootingSound != null)
            {
                audioSource.clip = shootingSound;
            }
        }
    }
 
    
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
        part.Play();
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.Play();
        }
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
        Destroy(gameObject, part.main.duration);
    }
}
