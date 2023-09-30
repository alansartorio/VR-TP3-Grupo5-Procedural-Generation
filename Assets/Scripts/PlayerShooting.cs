using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public Transform firePoint; // Punto desde donde se dispararán los proyectiles.
    public GameObject projectilePrefab; // Prefab del proyectil a disparar.
    public float fireRate = 0.5f; // Velocidad de disparo en segundos.
    private float nextFireTime;

    private void Update()
    {
        // Detectar si el jugador quiere disparar (por ejemplo, usando un botón o el mouse).
        if (Input.GetButtonDown("Fire1") && Time.time >= nextFireTime)
        {
            Shoot(); // Llamar a la función de disparo.
            nextFireTime = Time.time + 1f / fireRate; // Configurar el tiempo de espera antes del próximo disparo.
        }
    }

    private void Shoot()
    {
        // Instanciar un proyectil desde el prefab en el punto de disparo.
        Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
    }
}