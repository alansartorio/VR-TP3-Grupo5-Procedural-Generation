using UnityEngine;

public class ShootingController : MonoBehaviour
{
    public float fireRate = 0.1f;
    public int damagePerShot = 1;
    public float weaponRange = 100f;
    public Transform gunBarrel;

    private float nextFireTime;

    private void Update()
    {
        if (Input.GetButtonDown("Fire1") && Time.time > nextFireTime)
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
